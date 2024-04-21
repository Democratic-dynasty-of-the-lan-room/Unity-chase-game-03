using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public Transform player; // Reference to your player's transform
    public float minFOV = 60f; // Minimum FOV
    public float maxFOV = 90f; // Maximum FOV
    public float velocityMultiplier = 1f; // Multiplier to control the impact of velocity on FOV
    public float interptime = 20f;
    public float velocityThreshold = 9f; // Velocity threshold for ADRENALINE
    private float ADRENALINE = 0f;
    public float ADRENALINEMax = 100f;
    public float ADRENALINESpeed = 0.3f;
    public float ADRENALINEFallSpeed = 0.3f;
    public float swayAmount = 0.2f; // Adjust the sway amount
    public float swaySpeed = 2f; // Adjust the sway speed

    public float sensX;
    public float sensY;

    public Transform orientation;

    private Camera mainCamera;
    private float targetSway = 0f;
    float xRotation;
    float yRotation;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        mouseMove();

        adrenalinAdder();
        if (player == null || mainCamera == null)
        {
            return;
        }
    }
    private void adrenalinAdder()
    {
        bool isPlayerGrounded = player.GetComponent<PlayerMovment>().Touchedground;

        // Calculate the velocity magnitude of the player considering only x and z components
        Vector3 playerVelocity = player.GetComponent<Rigidbody>().linearVelocity;
        playerVelocity.y = 0f; // Ignore y component

        float magnitudeWithoutY = playerVelocity.magnitude;

        // Check if the player's velocity is above the threshold
        if (magnitudeWithoutY > velocityThreshold)
        {
            ADRENALINE = Mathf.SmoothStep(ADRENALINE, ADRENALINEMax, ADRENALINESpeed * Time.deltaTime);
            //Debug.Log(ADRENALINE);

            if (isPlayerGrounded)
            {
                // Smoothly transition to the sway and downward rotation based on adrenaline
                targetSway = Mathf.Lerp(targetSway, swayAmount, Time.deltaTime * swaySpeed);

                // Apply a slow downward rotation on the camera when on the ground and above the velocity threshold
                float downwardRotationSpeed = ADRENALINE / 200; // Adjust the rotation speed as needed
                xRotation = Mathf.Lerp(xRotation, 45f, Time.deltaTime * downwardRotationSpeed);
            }
            else
            {
                targetSway = Mathf.Lerp(targetSway, 0f, Time.deltaTime * swaySpeed);
            }
        }
        else
        {
            ADRENALINE = Mathf.SmoothStep(ADRENALINE, 0, ADRENALINEFallSpeed * Time.deltaTime);

            // Smoothly transition out of the sway when below the velocity threshold
            targetSway = Mathf.Lerp(targetSway, 0f, Time.deltaTime * swaySpeed);
        }

        // Apply swaying based on the target sway
        float sway = Mathf.Sin(Time.time * swaySpeed) * ADRENALINE * targetSway;
        Vector3 swayRotation = new Vector3(0f, sway / 1.5f, sway);
        transform.Rotate(swayRotation);  // Rotate around the forward axis (X-axis)

        // Map the velocity to FOV within the specified range
        float targetFOV = Mathf.Clamp(minFOV + magnitudeWithoutY * velocityMultiplier, minFOV, maxFOV);

        // Apply FOV change
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, (targetFOV + (ADRENALINE / 2)), Time.deltaTime * interptime);


    }
    private void mouseMove()
    {
        // Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        // Rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
