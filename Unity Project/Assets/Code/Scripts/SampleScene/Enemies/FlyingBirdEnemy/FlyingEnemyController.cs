using UnityEngine;
using System.Collections;


public class FlyingEnemyController : MonoBehaviour
{
    [SerializeField] GameObject MovementPlayer;

    Rigidbody rb;

    public float LookRadius;


    [Header("FlyingMachinery")]
    RaycastHit hit;

    public Transform Target;

    public float Speed;

    public float DistanceToGroundRay;
    public float DistancetoRoofRay;

    public float ForwardsRay;
    public float FortyFiveDegreRay;
    public float MinusFortyFiveDegreRay;

    public float AccuriteFaceDistance;

    [Header("Obstacle avoidance")]
    public float GroundAvoidance;
    public float RoofAvoidance;

    [Header("Target")]
    public float stopingDistance;

    public float distanceToTarget;


    [Header("States")]
    public bool IsWandering;

    public bool IsChasing;
    public bool IsSwooping;
    public bool IsPecking;


    [Header("Wander State")]

    [Tooltip("Can only be less than LookRadius")]
    public float WanderDistance;

    private float DistanceToWanderPos;

    [Tooltip("Minimum distance that can be flown to when picking a positiong to fly to")]
    public float MinimumWanderDistance;

    public bool CanFindWanderPos;

    //private float distanceToWanderPos;

    private IEnumerator coroutine;

    public Transform WanderPosition;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Setting bools
        IsWandering = true;

        CanFindWanderPos = true;
    }

    private void FixedUpdate()
    {
        ObstacleAvoidance();

        TargetPosition();


        if (distanceToTarget > AccuriteFaceDistance)
        {
            FaceTargetThreeDSlow();
        }
        else
        {
            FaceTargetThreeD();
        }
        //FaceTarget();

        //FaceTargetThreeD();

        //FaceTargetThreeDSlow();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO - Get The distance to the floor.
        // Get the height that the player is at. 
        // Get the distance to the player.

        // TODO - Make a waypoint Target movement system. Using Addforce with ristrictions and a good way to change direction zeroing other axis velocity.

        // TODO - object avoidance raycasts. depending how far or close you are to objects.

        // TODO - SetUpStateMachine bools

        if (IsWandering)
        {

            WanderState();

        }
        else if (IsChasing)
        {
            ChasingState();
        }
    }


    // State Machine Functions
    private void WanderState()
    {     
        if (CanFindWanderPos == true)
        {
            WanderPosition.transform.position = Random.insideUnitSphere * WanderDistance + transform.position;// What is transform.position for here?

            // Limit how far positions can spawn backwards.
        }

        DistanceToWanderPos = Vector3.Distance(transform.position, WanderPosition.transform.position);

        Vector3 directionNormalized = (WanderPosition.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, directionNormalized, out hit, DistanceToWanderPos))
        {
            CanFindWanderPos = true;

            //Debug.Log("Hit: " + hit.collider.name);

            //Debug.Log("HitWanderPosition?");
        }
        else
        {
            // Get new position to fly to if Has reached minimum wander distance
            if (DistanceToWanderPos > MinimumWanderDistance || distanceToTarget > MinimumWanderDistance)
            {        
                CanFindWanderPos = false;

                Target.position = WanderPosition.transform.position;

                //Debug.Log("NotHittinganything");
            }
            else
            {
                CanFindWanderPos = true;
            }

            if (distanceToTarget > LookRadius || DistanceToWanderPos > LookRadius)
            {
                CanFindWanderPos = true;

                Debug.Log("ItWen't out of view radius finding a new position to go to");
            }
        }

        if (DistanceToWanderPos <= 3)
        {
            //CanFindWanderPos = true;

            Debug.Log("CanFindWanderPos = " + CanFindWanderPos);
        }

        // TODO - set up an algorithm for finding paths that aren't blocked in the air.

    }

    private void HoverState()
    {

    }

    private void PerchState()
    {

    }

    private void RunningOnGroundState()
    {
        // for this a navmesh agent should suffice.
    }

    private void SearchingState()
    {
        if (CanFindWanderPos == true)
        {
            WanderPosition.transform.position = Random.insideUnitSphere * WanderDistance + transform.position;// What is transform.position for here?

            // Limit how far positions can spawn backwards.
        }

        DistanceToWanderPos = Vector3.Distance(transform.position, WanderPosition.transform.position);

        Vector3 directionNormalized = (WanderPosition.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, directionNormalized, out hit, DistanceToWanderPos))
        {
            CanFindWanderPos = true;

            //Debug.Log("Hit: " + hit.collider.name);

            //Debug.Log("HitWanderPosition?");
        }
        else
        {
            // Get new position to fly to if Has reached minimum wander distance
            if (DistanceToWanderPos > MinimumWanderDistance || distanceToTarget > MinimumWanderDistance)
            {
                CanFindWanderPos = false;

                Target.position = WanderPosition.transform.position;

                //Debug.Log("NotHittinganything");
            }
            else
            {
                CanFindWanderPos = true;
            }

            if (distanceToTarget > LookRadius || DistanceToWanderPos > LookRadius)
            {
                CanFindWanderPos = true;

                Debug.Log("ItWen't out of view radius finding a new position to go to");
            }
        }

        if (DistanceToWanderPos <= 3)
        {
            //CanFindWanderPos = true;

            Debug.Log("CanFindWanderPos = " + CanFindWanderPos);
        }

        // TODO - set up an algorithm for finding paths that aren't blocked in the air.
    }

    private void ChasingState()
    {
        Target.position = MovementPlayer.transform.position;
    }

    private void SwoopingAttackState()
    {

    }

    private void PeckingAttackState()
    {

    }






    // Flying Machinery This should probably be in a separate script from the stateMachine controller.
    // It should probably be an abstract class? That way I can call my functions from there.
    private void ObstacleAvoidance()
    {

        // avoidance of floor
        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            DistanceToGroundRay = hit.distance;

            if (DistanceToGroundRay <= GroundAvoidance)
            {
                rb.AddForce(transform.up);
            }
            else
            {
                /*
                // Get the current angular velocity
                Vector3 currentAngularVelocity = rb.linearVelocity;

                // Set the y component to zero
                currentAngularVelocity.y = 0f;

                // Assign the modified angular velocity back to the Rigidbody
                rb.linearVelocity = currentAngularVelocity;

                Debug.Log("Limitvelocity y?");
                */
            }
        }

        // avoidance of roof
        if (Physics.Raycast(transform.position, transform.up, out hit))
        {
            DistancetoRoofRay = hit.distance;

            if (DistancetoRoofRay <= RoofAvoidance)
            {
                rb.AddForce(-transform.up);
            }
            else
            {
                    
            }
        }

        // Forwards Ray
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {

        }

        // forty five right ray
        if (Physics.Raycast(transform.position, transform.right + transform.forward, out hit))
        {

        }

        // forty five left ray
        if (Physics.Raycast(transform.position, -transform.right + transform.forward, out hit))
        {

        }
    }

    private void StayUpright()
    {
        // Unless the bird is diving it could be cool without.\

        // TODO - Limit Rotataionon x and z so that it is impossible to turn upside down.

        // TODO - add a weight at the bottom that the bird tends to rotate towards. Something like rotate slowly towards level position if you arent currently level.

    }

    private void NavigationSystem()
    {
        // Like Navmehs agent navigation path making except for the air.

        // The idea is to have set waypoints spread between the target position and the bird. Use an algorithm that smoothly places Waypoints through the air.
        // Then using gradual rotation, Angular speed. Acceleration to realisticly Move between waypoints. The waypoints being a guide line that isn't followed perfectly.
        
    }

    private void TargetPosition()
    {
        distanceToTarget = Vector3.Distance(Target.position, transform.position);

        if (distanceToTarget < LookRadius && distanceToTarget > stopingDistance)
        {
            // TODO - Make The Bird Lerp position and rotation To the right direction. So that is is smoother. Angular momentum.



            Vector3 direction = (Target.position - transform.position).normalized;

            //rb.AddForce(direction * Speed, ForceMode.Force);

            rb.AddForce(transform.forward * Speed, ForceMode.Force);






            /*
            // Only apply force in the forward direction.
            // Get the current velocity in world space
            Vector3 currentVelocity = rb.linearVelocity;

            // Convert the world space velocity to local space
            Vector3 localVelocity = transform.InverseTransformDirection(currentVelocity);

            // Modify the local velocity components
            localVelocity.y = 0f;
            localVelocity.x = 0f;

            // Convert the modified local velocity back to world space
            Vector3 modifiedVelocity = transform.TransformDirection(localVelocity);

            // Assign the modified velocity back to the Rigidbody
            rb.linearVelocity = modifiedVelocity;

            Debug.Log("Limitvelocity?");
            */
        }
        else
        {
            // Turn away
        }

        // TODO - Handle Rotation
    }

    void FaceTarget()
    {
        Vector3 direction = (Target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void FaceTargetThreeD()
    {
        Vector3 direction = (Target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    void FaceTargetThreeDSlow()
    {
        Vector3 direction = (Target.position - transform.position).normalized;

        Vector3 DirectionNotNormalized = (Target.position - transform.position);

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, LookRadius);

        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            DistanceToGroundRay = hit.distance;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, -transform.up * DistanceToGroundRay);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Target.position, 1);


        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(WanderPosition.transform.position, 1);


        Gizmos.color = Color.green;
        Vector3 direction = (WanderPosition.transform.position - transform.position).normalized;
        Gizmos.DrawRay(transform.position, direction * DistanceToWanderPos);
    }
}
