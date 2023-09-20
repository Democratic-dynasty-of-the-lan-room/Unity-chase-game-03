using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.Animations;

public class PlayerMovment : MonoBehaviour
{
    [Header("Movment")]
    private float moveSpeed;
    public float GroundMovement;
    public float SprintSpeed;
    public float CrouchDrag;
    public float ChrouchMovement;


    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("AirSpeed")]
    public float AirMovement;
    public float LimitSpeed;
    public float SprintLimit;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;


    public bool SpeedLimitType;
    
    


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

        Crouch();
    } 

    private void LateUpdate()
    {
        // ground check      
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
       

        MyInput();
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

    private void Statesetter()
    {

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
        if (SpeedLimitType == false || !grounded)
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > LimitSpeed)
            {
                Debug.Log("speed went over");

                Vector3 limitedVel = flatVel.normalized * LimitSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
        else if (SpeedLimitType == true)
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
    }

    private void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && grounded)
        {
            SpeedLimitType = true;

            Debug.Log("Sprint");
            
            rb.AddForce(moveDirection * SprintSpeed, ForceMode.Force);
        }
        else
        {
            SpeedLimitType = false;
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
}