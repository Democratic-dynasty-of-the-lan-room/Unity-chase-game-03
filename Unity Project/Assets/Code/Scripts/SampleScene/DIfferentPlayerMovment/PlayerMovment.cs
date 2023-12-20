using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.Animations;



public class PlayerMovment : MonoBehaviour
{
    RaycastHit hit;

    [Header("Ghost")]
    
    [Header("General")]
    public float CurrentSpeedLimit;
    public float proportionalGain;
    public float derivativeGain;
    private float previousHeightError;

    [Header("Walk")]
    private float moveSpeed;
    public float GroundMovement;
    public float WalkLimit = 8.999998f;
    public float groundDrag;

    [Header("Jump/Fall")]
    public float jumpForce;
    public float jumpCooldown;
    bool readyToJump;
    public float airMultiplier;
    public bool jumped;
    public bool isJumping = false;
    float jumpStartTime = 0f;

    [Header("Crouch")]
    public float ChrouchMovement;
    public float CrouchDrag;

    [Header("AirSpeed")]
    public float AirMovement;

    [Header("Sprint")]
    public float SprintSpeed;
    public float SprintLimit;
    private bool IsSprinting;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public LayerMask PlayerLayer;
    public bool grounded;
    public float SphereCastRadius;
    public float SphereCastDistance;
    public Vector3 SphereCastPostitionY;
    public float desiredHeight;
    public float interpolationTime = 0.1f;
    private float CurrentHeight;
    public float HeightOffset;
    public float GroundedHeight =-1.3f;
    public float LandedHeight;

    private bool HasRun;

    [Header("SlopeMovement")]
    //private Vector3 SurfaceNormal;
    public float MaxSlopeAngle;
    //private Vector3 SlopeMoveDirection;
    private RaycastHit SlopeHit;
    public bool ExitingSlope;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private Animator anim;

    public GameObject PlayerFootCollider;

    private void Start()
    {
        //anim = PlayerFootCollider.GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        ResetJump();

        readyToJump = true;

        CurrentHeight = transform.position.y;
    }

    private void Update()
    {
        MyInput();

        Crouch();
    }

    private void LateUpdate()
    {


        if (isJumping && Time.time - jumpStartTime > jumpCooldown)
        {
            isJumping = false;
        }

        SpeedLimiting();

        // handle drag
        if (grounded && Input.GetKey(KeyCode.LeftControl))
        {
            rb.drag = CrouchDrag;
            moveSpeed = ChrouchMovement;
        }
        else if (grounded)
        {
            rb.drag = groundDrag;
            moveSpeed = GroundMovement;
        }
        else
        {
            rb.drag = 0;

            moveSpeed = AirMovement;
        }
    }

    private void FixedUpdate()
    {
        if (!isJumping)
        {
            MovePlayer();
        }

        Grounding();

        if (!isJumping)
        {
            Sprint();
        }

    }

    private void MyInput()
    {
        //set GroundedHeight to be equal to height offset when in the air
        if (!grounded || isJumping == true)
        {
            GroundedHeight = HeightOffset * -1.3f;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded && (Mathf.Abs(desiredHeight - (transform.position.y-0.2f)) < 0.33f))
        {
            isJumping = true;
            jumpStartTime = Time.time;

            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // calculate movment direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // if on slope
        if (OnSlope() && !ExitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 10f, ForceMode.Force);

            //Debug.Log("GetSlopeMoveDirection one" + GetSlopeMoveDirection());

            //Debug.Log("OnSlopeCheck");

            //if (rb.velocity.y > 0)
            //rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }       // on ground
        else if (grounded)
        {
            //Debug.Log("Grounded");

            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }//in air
        else if (!grounded)
        {
            //Debug.Log("Not Grounded");

            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        //turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedLimiting()
    {
        // Limiting slope walk speed
        if (OnSlope() && !ExitingSlope && !IsSprinting)
        {
            if (rb.velocity.magnitude > WalkLimit)
            {
                rb.velocity = rb.velocity.normalized * WalkLimit;
            }

        }
        // Limiting slope Sprint speed
        else if (OnSlope() && !ExitingSlope && IsSprinting == true)
        {
            if (rb.velocity.magnitude > SprintLimit)
            {
                rb.velocity = rb.velocity.normalized * SprintLimit;
            }
        }
        // Limiting speed on ground or air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > SprintLimit)
            {
                //Debug.Log("speed went over");

                Vector3 limitedVel = flatVel.normalized * SprintLimit;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Sprint()
    {

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && grounded)
        {
            IsSprinting = true;

            CurrentSpeedLimit = SprintLimit;

            //Debug.Log("Sprint");

            if (OnSlope())
            {
                rb.AddForce(GetSlopeMoveDirection() * SprintSpeed, ForceMode.Force);
            }
            else if (!OnSlope())
            {
                rb.AddForce(moveDirection * SprintSpeed, ForceMode.Force);
            }
        }
        else
        {
            CurrentSpeedLimit = WalkLimit;

            IsSprinting = false;
        }
    }

    public void Grounding()
    {
        if (grounded && !HasRun && (Mathf.Abs(desiredHeight - (transform.position.y+0.32f)) < 0.3f))
        {

           GroundedHeight = LandedHeight; 
           // Debug.Log("Velocity zero");

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            HasRun = true;
        }
        else if (!grounded)
        {
            HasRun = false;
        }

        // ground check            
        grounded = Physics.SphereCast(transform.position + SphereCastPostitionY, SphereCastRadius, Vector3.down, out hit, SphereCastDistance, whatIsGround);

        //SurfaceNormal = hit.normal;

        //Debug.Log(hit.distance);

        // This runs while on the ground
        if (hit.point.y > rb.transform.position.y + GroundedHeight == true && isJumping == false && hit.distance != 0)
        {
            ExitingSlope = false;



            // desired height to ground
            desiredHeight = hit.point.y + HeightOffset;
            
            float currentHeight = transform.position.y;

            float heightDifference = desiredHeight - currentHeight;

            // Calculate proportional and derivative terms
            float proportionalTerm = heightDifference;
            float derivativeTerm = heightDifference - previousHeightError;

            // Calculate the force based on PD control
            float controlForce = proportionalTerm * proportionalGain + derivativeTerm * derivativeGain;

            // Apply the force to the Rigidbody
            rb.AddForce(new Vector3(0f, controlForce, 0f));

            // Update the previous height error for the next iteration
            previousHeightError = heightDifference;
            grounded = true;
            rb.useGravity = false;

            // whatisground and PlayerLayer don't collide
            Physics.IgnoreLayerCollision(6, 9, true);
        }
        else
        {
            grounded = false;
            //Debug.Log("Spherecast Not grounded");
            rb.useGravity = true;

            // what is ground and PlayerLayer do collide
            Physics.IgnoreLayerCollision(6, 9, false);
        }
    }

    private void Jump()
    {

        //reset y velocity
        rb.velocity = rb.velocity.normalized * (hit.normal.y*rb.velocity.magnitude);

        rb.AddForce(hit.normal * jumpForce, ForceMode.Impulse);

        ExitingSlope = true;

        jumped = true;
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // Debug.Log("Crouch");

            HeightOffset = 0.35f;
            playerHeight = 0.5f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {

            HeightOffset = 1f;
            playerHeight = 2f;
        }
    }

    private bool OnSlope()
    {
        if (Physics.SphereCast(transform.position + SphereCastPostitionY, SphereCastRadius, Vector3.down, out SlopeHit, playerHeight * 2f + 0.5f))
        {
            //Debug.DrawLine(transform.position, SlopeHit.point, Color.red);

            float angle = Vector3.Angle(Vector3.up, SlopeHit.normal);
            return angle < MaxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, SlopeHit.normal).normalized;

        //Debug.Log("GetSlopeMoveDirection" + GetSlopeMoveDirection());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + SphereCastPostitionY, SphereCastRadius);

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, SphereCastDistance))
        {
            Gizmos.DrawLine(transform.position, hitInfo.point);
        }
        else
        {
            // Draw the sphere cast line up to the maximum distance
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * SphereCastDistance);
        }
    }
}