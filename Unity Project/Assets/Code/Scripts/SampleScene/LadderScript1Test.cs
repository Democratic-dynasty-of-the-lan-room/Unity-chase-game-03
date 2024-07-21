using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class LadderScript1Test : MonoBehaviour
{
    private IEnumerator coroutine;

    [SerializeField] PlayerMovment playermovement;

    //[SerializeField] GameObject PlayerObject;

    [Header("Ladder Movement")]

    Rigidbody rb;

    RaycastHit hit;

    float horizontalInput;
    float verticalInput;

    public Transform orientation;

    public float speed;

    public bool CanLadderMovement;

    Vector3 moveDirection;

    Vector3 EndLadderMoveDirection;

    Vector3 LadderMoveDirection;

    Vector3 LadderNormal;

    public LayerMask WhatIsGround;

    public bool Grounded;

    public float Amount;

    public float Amount2;

    public float Time;

    // sphere Cast stuff
    public float SphereCastRadius;
    public float MaxDistance;
    public Vector3 EndLadderNormal;

    public Vector3 LadderMoveDirectionTest;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        playermovement.CanLadderCrouch = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       if (CanLadderMovement)
       {
           LadderMovement();   
       }

        GroundCheck();

        //SphereCast();



        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Amount - Amount2, WhatIsGround))// Here if you crouch it thinks you shouldn't be on the ladder.
        {
            CanLadderMovement = false;

            playermovement.enabled = true;

            //Debug.Log("Out of ladder on ground");
        }

        /*
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Amount - Amount2) && playermovement.Crouched)
        {
            Debug.Log("Crouch");

            if (hit.collider == null)
            {
                playermovement.enabled = false;

                CanLadderMovement = true;

                Debug.Log("Crouch Ladder");
            }
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        // When Going down the ladder check if the player has triggered this and then enable ladder movement?

        //Debug.Log("OnTriggerEnter");

        if (other.tag == "Ladder")
        {
            Debug.Log("OnTriggerEnter Player");

            // to stop speeding up the ladder.
            rb.linearVelocity = new Vector3(0, 0, 0);
            rb.linearDamping = playermovement.groundDrag;
            rb.useGravity = false;

            playermovement.enabled = false;

            CanLadderMovement = true;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1f) ||
                Physics.Raycast(transform.position, -transform.forward, out hit, 1f) ||
                Physics.Raycast(transform.position, transform.right, out hit, 1f) ||
                Physics.Raycast(transform.position, -transform.right, out hit, 1f))
            {
                LadderNormal = hit.normal.normalized;

                //Debug.Log("Hit LadderNormal");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ladder")
        {
            Debug.Log("OnTriggerExit");

            CanLadderMovement = false;

            playermovement.enabled = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (collision.gameObject.CompareTag("Ladder"))
        {
            Debug.Log("On Ladder");

            // to stop speeding up the ladder.
            rb.linearVelocity = new Vector3(0, 0, 0);
            rb.linearDamping = playermovement.groundDrag;
            rb.useGravity = false;

            playermovement.enabled = false;

            CanLadderMovement = true;

            // I moved this code here instead of collision exit that would definitely break how it is supposed to work lol.

            //LadderNormal = Vector3.ProjectOnPlane(moveDirection, orientation.right);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1f) ||
                Physics.Raycast(transform.position, -transform.forward, out hit, 1f) ||
                Physics.Raycast(transform.position, transform.right, out hit, 1f) ||
                Physics.Raycast(transform.position, -transform.right, out hit, 1f))
            {      
                LadderNormal = hit.normal.normalized;

                //Debug.Log("Hit LadderNormal");
            }


            //LadderNormal = Vector3.ProjectOnPlane(transform.position, hit.normal).normalized;
        }
        */
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            /*
            Debug.Log("Off Ladder");

            CanLadderMovement = false;

            playermovement.enabled = true;

            rb.useGravity = true;

            if (rb.linearVelocity.y > 0)
            {
                coroutine = EndLadderCoroutine(Time);
                StartCoroutine(coroutine);
            }
            */
        }
    }


    private void PressEToInteract()
    {

        // TODO - lerp player to the start of the ladder.

    }

    private void LadderMovement()
    {
        // Handle all ladder movement in here.

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


        Vector3 rightDirection = Vector3.Cross(LadderNormal, Vector3.up);
        Vector3 movementDirection = rightDirection;

        //Vector3 HorizontalDirection = Vector3.ProjectOnPlane(hit.point, transform.position);
        //Vector3 NewLadderMoveDirection = HorizontalDirection;

        // TODO - Make the W move you in the direction you are looking. Up or down.


        //moveDirection = orientation.up * verticalInput + orientation.right * horizontalInput;

        LadderMoveDirection = orientation.up * verticalInput + movementDirection * horizontalInput;

        Vector3 ProjectedMovement = Vector3.ProjectOnPlane(LadderMoveDirection, hit.normal);

        //LadderMoveDirectionTest = orientation.up * verticalInput + movementDirection * horizontalInput;

        //EndLadderMoveDirection = NewLadderMoveDirection * verticalInput + movementDirection * horizontalInput;

        if (horizontalInput > 0 || horizontalInput < 0 || verticalInput > 0 || verticalInput < 0)
        {            
            rb.AddForce(LadderMoveDirection * speed, ForceMode.Force);

            //Debug.Log("Direction" + HorizontalDirection);

            //Debug.Log("Hit Normal" + hit.normal.y);
        }
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        if  (Physics.Raycast(transform.position, Vector3.down, out hit, Amount, WhatIsGround))
        {
            Grounded = true;

            /*
            if (hit.collider.CompareTag("Ladder"))
            {
                CanLadderMovement = true;

                playermovement.enabled = false;
            }
            */

            //Debug.Log("Grounded");
        }
        else
        {
            Grounded = false;

            /*
            CanLadderMovement = false;

            playermovement.enabled = true;
            */

            //Debug.Log("Not Grounded");
        }
    }

    /*
    private void SphereCast()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, SphereCastRadius, Vector3.down, out hit, MaxDistance))
        {
            //Debug.Log("SphereCast");

            if (hit.collider.gameObject.CompareTag("Ladder"))
            {
                //EndLadderNormal = hit.normal.normalized;

                //CanLadderMovement = true;

                //Debug.Log("CanLadder?" + CanLadderMovement);
            }
            else
            {
                //CanLadderMovement = false;

                //Debug.Log("CanLadder?" + CanLadderMovement);
            }
        }
        else
        {

        }
    }
    */

    /*
    private IEnumerator EndLadderCoroutine(float Time)
    {
        //playermovement.CanLadderCrouch = true;

        Debug.Log("coroutine started");

        yield return new WaitForSeconds(Time);

        playermovement.CanLadderCrouch = false;

        Debug.Log("coroutine done");
    }
    */

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SphereCastRadius);

    }
    */
}
