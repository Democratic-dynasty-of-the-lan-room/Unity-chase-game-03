using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEditor;

public class LadderScript1Test : MonoBehaviour
{
    [Header("Ladder Movement")]

    [SerializeField] PlayerMovment playermovement;

    public LayerMask WhatIsGround;

    public Transform orientation;

    Rigidbody rb;

    RaycastHit hit;

    public float Ladderspeed;
    public float MinusHeightOffset;
    public float RotateAngle;
    public float SwitchAmountVertical;

    private float horizontalInput;
    private float verticalInput;
    private float verticalThreshold = 0.2f;

    public bool CanLadderMovement;

    Vector3 HorizontalMoveDirection;
    Vector3 VerticalMoveDirection;
    Vector3 LadderMoveDirection;

    Vector3 LadderNormal;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (CanLadderMovement)
        {
            // Switch to ladder Movement
            LadderMovement();

            if (Physics.Raycast(transform.position, Vector3.down, out hit, playermovement.HeightOffset - MinusHeightOffset, WhatIsGround))
            {
                // Switch to player movement
                CanLadderMovement = false;
                playermovement.enabled = true;
            }
        }
    }

    // Enter Ladder Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ladder" && playermovement.Crouched == false || other.CompareTag("CrouchLadderCol") && playermovement.Crouched)
        {
            // Switch to ladder movement
            playermovement.enabled = false;
            CanLadderMovement = true;

            // Stop speeding. stop gravity. Add drag.
            rb.linearVelocity = new Vector3(0, 0, 0);
            rb.linearDamping = playermovement.groundDrag;
            rb.useGravity = false;

            // Get Ladder Normal.
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1f) ||
                Physics.Raycast(transform.position, -transform.forward, out hit, 1f) ||
                Physics.Raycast(transform.position, transform.right, out hit, 1f) ||
                Physics.Raycast(transform.position, -transform.right, out hit, 1f))
            {
                LadderNormal = hit.normal.normalized;
            }
        }
    }

    // Exit Ladder Trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ladder")
        {
            // Switch to player movement
            CanLadderMovement = false;
            playermovement.enabled = true;
        }
    }

    // Handle all ladder movement in here.
    private void LadderMovement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Calculate Up Down Movement.
        Vector3 forward = Camera.main.transform.forward;
        if (forward.y < SwitchAmountVertical)
        {
            VerticalMoveDirection = -orientation.up;
        }
        else
        {
            VerticalMoveDirection = orientation.up;
        }

        // Calculate Right Left Movement.
        Vector3 rightDirection = Vector3.Cross(LadderNormal, Vector3.up);
        Vector3 ladderNormalProjected = new Vector3(LadderNormal.x, 0, LadderNormal.z).normalized;
        if (Mathf.Abs(forward.y) > verticalThreshold)// Make sure that when you are looking straight up or down right left movement is still calculated correctly.
        {
            Vector3 right = Camera.main.transform.right; 

            Vector3 forwardHorizontal = new Vector3(right.x, 0, right.z).normalized;

            Vector3 RotateVectorBy90Degrees(Vector3 vector)
            {
                return new Vector3(-vector.z, vector.y, vector.x);
            }

            Vector3 rotatedForward = RotateVectorBy90Degrees(forwardHorizontal);

            float dotProduct = Vector3.Dot(rotatedForward, ladderNormalProjected);
            if (dotProduct > 0)
            {
                HorizontalMoveDirection = -rightDirection;
            }
            else
            {
                HorizontalMoveDirection = rightDirection;
            }
        }
        else
        {
            Vector3 ForwardProjected = new Vector3(forward.x, 0, forward.z).normalized;

            if (Vector3.Angle(ForwardProjected, ladderNormalProjected) > RotateAngle)
            {
                HorizontalMoveDirection = rightDirection;
            }
            else
            {
                HorizontalMoveDirection = -rightDirection;
            }
        }

        // apply Force to LadderMoveDirection
        LadderMoveDirection = VerticalMoveDirection * verticalInput + HorizontalMoveDirection * horizontalInput;

        if (horizontalInput > 0 || horizontalInput < 0 || verticalInput > 0 || verticalInput < 0)
        {
            rb.AddForce(LadderMoveDirection * Ladderspeed, ForceMode.Force);
        }
    }
}
