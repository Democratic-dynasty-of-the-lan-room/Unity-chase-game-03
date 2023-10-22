using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.Animations;



public class PlayerMovment : MonoBehaviour
{
    [Header("Ghost")]

    [Header("Walk")]
    private float moveSpeed;
    public float GroundMovement;
    public float WalkLimit;
    public float groundDrag;

    [Header("Jump/Fall")]
    public float jumpForce;
    public float jumpCooldown;
    bool readyToJump;
    public float airMultiplier;

    [Header("Crouch")]
    public float ChrouchMovement;
    public float CrouchDrag;

    [Header("AirSpeed")]
    public float AirMovement;

    [Header("Sprint")]
    public float SprintSpeed;



    public float SprintLimit;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    public float SphereCastRadius;
    public float SphereCastDistance;





    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private Animator anim;

    public GameObject PlayerFootCollider;


    private void Start()
    {
        anim = PlayerFootCollider.GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        ResetJump();

        readyToJump = true;
    }

    private void Update()
    {
        MyInput();
    } 

    private void LateUpdate()
    {
        RaycastHit hit;

        // ground check       
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        Physics.SphereCast(transform.position, SphereCastRadius, Vector3.down, out hit, SphereCastDistance, whatIsGround);

        if (hit.transform == true)
        {
            print(hit.distance);
            Debug.Log("Spherecast check");
            grounded = true;
            rb.useGravity = false;
        }
        else 
        {
            grounded = false;
            Debug.Log("Spherecast Not grounded");
            rb.useGravity = true;
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

        Sprint();   
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
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

        // on ground
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        //in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedLimiting()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > SprintLimit)
        {
            Debug.Log("speed went over");

            Vector3 limitedVel = flatVel.normalized * SprintLimit;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && grounded)
        {
            

            Debug.Log("Sprint");
            
            rb.AddForce(moveDirection * SprintSpeed, ForceMode.Force);
        }    
    }    

    private void Jump()
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("Crouch");

            anim.SetTrigger("trCrouch");

            playerHeight = 0.5f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            anim.SetTrigger("trUncrouch");

            playerHeight = 2f;
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, SphereCastRadius);

        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, SphereCastRadius, Vector3.down, out hitInfo, SphereCastDistance))
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