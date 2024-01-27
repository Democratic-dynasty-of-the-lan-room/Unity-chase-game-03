using UnityEngine;

public class CameraFOVController : MonoBehaviour
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

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }
    }

    void Update()
    {
        if (player == null || mainCamera == null)
        {
            return;
        }

        // Calculate the velocity magnitude of the player considering only x and z components
        Vector3 playerVelocity = player.GetComponent<Rigidbody>().velocity;
        playerVelocity.y = 0f; // Ignore y component

        float magnitudeWithoutY = playerVelocity.magnitude;

        // Check if the player's velocity is above the threshold
        if (magnitudeWithoutY > velocityThreshold)
        {
            ADRENALINE = Mathf.SmoothStep(ADRENALINE, ADRENALINEMax, ADRENALINESpeed * Time.deltaTime);
            Debug.Log(ADRENALINE);
        }
        else
        {
            ADRENALINE = Mathf.SmoothStep(ADRENALINE, 0, ADRENALINEFallSpeed * Time.deltaTime);
            Debug.Log(ADRENALINE);
        }

        // Map the velocity to FOV within the specified range
        float targetFOV = Mathf.Clamp(minFOV + magnitudeWithoutY * velocityMultiplier, minFOV, maxFOV);

        // Apply FOV change
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, (targetFOV + (ADRENALINE / 2)), Time.deltaTime * interptime);

        // Apply swaying based on adrenaline
        float sway = Mathf.Sin(Time.time * swaySpeed) * ADRENALINE * swayAmount;
        transform.rotation = Quaternion.Euler(0f, sway, 0f) * player.rotation;
    }
}