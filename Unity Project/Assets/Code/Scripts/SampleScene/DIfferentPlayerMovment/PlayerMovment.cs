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
    bool isJumping = false;
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
    public float DesiredHeight;
    public float interpolationTime = 0.1f;
    private float CurrentHeight;
    public float HeightOffset;
    public float GroundedHeight;

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
    }

    private void LateUpdate()
    {
       

        if (isJumping && Time.time - jumpStartTime > 0.5f)
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
        MovePlayer();

        MovementTesting();

        Sprint();
    }
    
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            isJumping = true;
            jumpStartTime = Time.time;

            readyToJump = false;

            Jump();

            Crouch();

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
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

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
            IsSprinting = false;       
        }
    }

    public void MovementTesting()
    {
        print(rb.transform.position.y + GroundedHeight);

        if (grounded && !HasRun)
        {
            //Debug.Log("Velocity zero");

            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            HasRun = true;
        }
        else if (!grounded)
        {
            HasRun = false;
        }

        // ground check            
        grounded = Physics.SphereCast(transform.position, SphereCastRadius, Vector3.down, out hit, SphereCastDistance, whatIsGround);

        //SurfaceNormal = hit.normal;

        //Debug.Log(hit.distance);

        // This runs when you hit the ground
        if (hit.point.y > rb.transform.position.y + GroundedHeight == true && isJumping == false && hit.distance != 0)
        {
            ExitingSlope = false;

           

            // desired height to ground
            DesiredHeight = hit.point.y + HeightOffset;

            CurrentHeight = Mathf.Lerp(transform.position.y, DesiredHeight, interpolationTime);

            // .. and increase the t interpolater
            //interpolationTime += 0.5f * Time.deltaTime;

            
            //Debug.Log("Spherecast check");
            grounded = true;
            rb.useGravity = false;

            Vector3 newPosition = transform.position;
            newPosition.y = CurrentHeight;
            transform.position = newPosition;

            // whatisground and player layer don't collide
            Physics.IgnoreLayerCollision(6, 9, true);
        }
        else
        {
            grounded = false;
            //Debug.Log("Spherecast Not grounded");
            rb.useGravity = true;

            // what is ground and player layer do collide
            Physics.IgnoreLayerCollision(6, 9, false);
        }
    }

    private void Jump()
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

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

            anim.SetTrigger("trCrouch");

            playerHeight = 0.5f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            //anim.SetTrigger("trUncrouch");

            playerHeight = 2f;
        }
    }

    private bool OnSlope()
    {
        if (Physics.SphereCast(transform.position, SphereCastRadius, Vector3.down, out SlopeHit, playerHeight * 2f + 0.5f))
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
        Gizmos.DrawSphere(transform.position, SphereCastRadius);

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