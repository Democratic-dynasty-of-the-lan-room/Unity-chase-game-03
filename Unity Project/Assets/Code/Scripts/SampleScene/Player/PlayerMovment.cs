using UnityEngine;

namespace Code.Scripts.SampleScene.Player
{
    public class PlayerMovment : MonoBehaviour
    {
        [Header("Movment")]
        public float moveSpeed;

        public float groundDrag;

        [Header("Ground Check")]
        public float playerHeight;
        public LayerMask whatIsGround;
        bool grounded;

        public Transform orientation;

        float horizontalInput;
        float verticalInput;

        Vector3 moveDirection;

        Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        private void Update()
        {
            MyInput();
        }

        private void LateUpdate()
        {
            // ground check
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

            MyInput();
            SpeedControl();

            // handle drag
            if (grounded)
                rb.linearDamping = groundDrag;
            else
                rb.linearDamping = 0;
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void MyInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }

        private void MovePlayer()
        {
            // calculate movment direction
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        private void SpeedControl()
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
    }
}