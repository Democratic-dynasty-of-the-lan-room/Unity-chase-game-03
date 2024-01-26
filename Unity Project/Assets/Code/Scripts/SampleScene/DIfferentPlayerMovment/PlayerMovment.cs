using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using Code.Scripts;
using UnityEngine.Animations;



public class PlayerMovment : MonoBehaviour
{
    RaycastHit hit;

    [Header("Ghost")]

    [Header("General")]

    private float maxSpeed;
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

    private bool jump;
    public float jumpForce;
    public float jumpCooldown;
    bool readyToJump;
    public float airMultiplier;
    public bool jumped;
    public bool isJumping = false;
    float jumpStartTime = 0f;

    [Header("Crouch")]

    private float controlForce;
    public float ChrouchMovement;
    public float CrouchDrag;

    public bool Crouched;

    [Header("Speed limiting and adjust")]

    public float adjustmentReduction;

    [Header("AirSpeed")]
    public float AirMovement;

    [Header("Sprint")]
    private bool SprintKeyPressed;
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
    public bool groundedcast;
    public bool Touchedground;
    public float SphereCastRadius;
    public float SphereCastDistance;
    public Vector3 SphereCastPostitionY;
    public float desiredHeight;
    public float interpolationTime = 0.1f;
    private float CurrentHeight;
    public float HeightOffset;
    public float GroundedHeight = -1.3f;
    public float LandedHeight;

    private bool groundingHasRun;

    [Header("SlopeMovement")]
    public Vector3 correctHitNormal;
    private Vector3 lastHitNormal;
    public float normalchangeThreshold;

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

        TranslateVelocityforSlope();
        if (jump == true)
        {
            Jump();
            jump = false;
        }
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
        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded && (Mathf.Abs(desiredHeight - (transform.position.y - 0.2f)) < 0.33f))
        {
            isJumping = true;
            jumpStartTime = Time.time;

            readyToJump = false;

            jump = true;

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    //translate velocity when entering a slope
    private void TranslateVelocityforSlope()
    {
        if (grounded)
        {
            if (Vector3.Angle(lastHitNormal, correctHitNormal) > normalchangeThreshold && Vector3.Angle(lastHitNormal, correctHitNormal) < 40 && correctHitNormal.y > 0.78)
            {
                rb.velocity = Vector3.ProjectOnPlane(rb.velocity, correctHitNormal);
            }
            lastHitNormal = correctHitNormal;
        }
        else
        {
            lastHitNormal = new Vector3(0, 1, 0);
        }
    }
    private void MovePlayer()
    {
        // calculate movment direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // If no active movement input but there's horizontal velocity, auto-counter-strafe
        if (Mathf.Approximately(verticalInput, 0) && Mathf.Approximately(horizontalInput, 0) && (Mathf.Abs(rb.velocity.x) > 0.8f || Mathf.Abs(rb.velocity.z) > 0.8f) && grounded == true)
        {
            moveDirection = -new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized;
        }
        //Debug.Log("Not Grounded");

        if (SprintKeyPressed == true)
        {
            maxSpeed = (Mathf.Approximately(verticalInput, 1f) && Mathf.Approximately(horizontalInput, 0f)) ? SprintLimit : WalkLimit;
        }
        else
        {
            maxSpeed = WalkLimit;
        }

        // Calculate the horizontal component of speed
        Vector3 currentHorizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        float currentHorizontalSpeed = currentHorizontalVelocity.magnitude;
        Vector3 normalizedCurrentHorizontalVelocity = currentHorizontalVelocity.normalized;

        // Calculate the attempted movement direction
        Vector3 normalizedAttemptedDirection = moveDirection.normalized;
        Vector3 forceDirection = grounded ? GetSlopeMoveDirection() : moveDirection.normalized;

        // Calculate the dot product to get the cosine of the angle between vectors
        float dotProduct = Vector3.Dot(normalizedAttemptedDirection, normalizedCurrentHorizontalVelocity);
        // Calculate the angle in degrees
        float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
        if (angle < 90f && angle > 0.01 && currentHorizontalSpeed > maxSpeed)
        {
            // Move in the opposite direction of the current velocity
            Vector3 oppositeDirection = -normalizedCurrentHorizontalVelocity;
            rb.AddForce(oppositeDirection * moveSpeed * 1.05f * 10f, ForceMode.Force);

            rb.AddForce(forceDirection * moveSpeed * 10f, ForceMode.Force);
        }

        // Only apply force if the attempted movement direction is more than 90 degrees off-axis from the current movement direction
        if (angle > 90f || currentHorizontalSpeed < maxSpeed)
        {
            rb.AddForce(forceDirection * moveSpeed * 10f, ForceMode.Force);
        }

        //turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            SprintKeyPressed = true;
        }
        else
        {
            SprintKeyPressed = false;
        }

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
        if (grounded && !groundingHasRun && (Mathf.Abs(desiredHeight - (transform.position.y + 0.32f)) < 0.3f))
        {

            GroundedHeight = LandedHeight;
            // Debug.Log("Velocity zero");

            Vector3 killVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            killVelocity = Vector3.ProjectOnPlane(killVelocity, SlopeHit.normal);

            rb.velocity = killVelocity;

            groundingHasRun = true;

            Touchedground = true;
        }
        else if (!grounded)
        {
            groundingHasRun = false;

            Touchedground = false;
        }

        // ground check            
        groundedcast = Physics.SphereCast(transform.position + SphereCastPostitionY, SphereCastRadius, Vector3.down, out hit, SphereCastDistance, whatIsGround);

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
            controlForce = proportionalTerm * proportionalGain + derivativeTerm * derivativeGain;

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
        rb.velocity = rb.velocity.normalized * (hit.normal.y * rb.velocity.magnitude);

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
            Crouched = true;
            HeightOffset = 0.35f;
            playerHeight = 0.5f;

        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {

            HeightOffset = 1f;
            playerHeight = 2f;
            Crouched = false;
        }
    }

    private bool OnSlope()
    {
        if (Physics.SphereCast(transform.position + SphereCastPostitionY, SphereCastRadius, Vector3.down, out SlopeHit, playerHeight * 2f + 0.5f))
        {
            //Debug.DrawLine(transform.position, SlopeHit.point, Color.red);
            correctHitNormal = hit.GetCorrectNormalForSphere(Vector3.down);

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