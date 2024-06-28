using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.ProBuilder.MeshOperations;

namespace Code.Scripts.SampleScene
{
    public class ChargerScript : MonoBehaviour
    {
        [SerializeField] PlayerMovment MovementPlayer;

        [SerializeField] GameObject TerritoryCharger;

        [SerializeField] GameObject LastPlayerPosition;

        [SerializeField] GameObject chargerAttackBox;


        [SerializeField] GameObject ChargePosition;

        [SerializeField] LayerMask WhatIsGround;

        Rigidbody rb;

        private IEnumerator coroutine;

        private IEnumerator WaitChargedCoroutine;

        Transform target;
        NavMeshAgent agent;

        public float lookRadius = 10;

        [Tooltip("this distance to the target")]
        private float distance;
      
        private bool InTerritory;


       //State Machine  

        [Header("Chasing state")]

        public float ChasingSpeed;

        public float EnemySpeed;



        [Header("Attacking State")]

        [Tooltip("The distance at which attacking state starts")]
        public float AttackStateDistance = 15;

        public float AttackingSpeed;

        public float TimeWaitBeforeAttack;

        public float TimeWaitAfterCharged;

        [Tooltip("wait before checking if the enemy is on the ground or not after juming.")]
        public float TimeToWaitGrounding;

        public float DistanceToGround;

        public float ChargingSpeed;

        public bool CanStartWaitTimeCoroutine;

        public bool CanCharge;

        public bool WaitingState;

        public float AngularSpeed;

        public float ChargingAngularSpeed;

        public float NormalAngularSpeed;

        public float KnockBackForceUp;

        public float KnockBackForceBack;

        public bool CanKnockBack;




        // WanderState
        [Header("Wander state")]

        public GameObject WanderPosition;

        public float WanderDistance;

        [Tooltip("RandomUpperWanderWaitTime is how long the enemy waits before moving around to wander")]
        public float RandomUpperWanderWaitTime;

        [Tooltip("this is how long the enemy is able to wait before going back to it's territory")]
        public float UpperWanderOutsideTerritory;

        [Tooltip("this is The least amount of time that the enemy could take before going back into it's territory")]
        public float LowerWanderOutsideTerritory;

        public float WanderSpeed;

        
        // On/Off states booleans
        public bool IsChasing = false;

        public bool IsWandering = true;

        public bool IsGoingToTeritory = false;

        public bool IsAttacking;


        private bool CanGoBackToTerritory;

        private bool CanStartCoroutine;

        // Start is called before the first frame update
        void Start()
        {            
            target = PlayerManager.instance.player.transform;
            agent = GetComponent<NavMeshAgent>();

            //Making sure Player is set to true? I guess
            //MovementPlayer.SetActive(true);

            //this script I think
            this.enabled = true;

            rb = GetComponent<Rigidbody>();


            // setting Booleans
            CanStartCoroutine = true;

            CanGoBackToTerritory = false;

            CanCharge = false;

            CanStartWaitTimeCoroutine = true;

            WaitingState = false;

            CanKnockBack = true;
        }

        private void FixedUpdate()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            distance = Vector3.Distance(target.position, transform.position);

            // Setting which states are active or inactive.
            if (IsAttacking)
            {
                AttackingState();
            }
            else if (IsChasing)
            {
                ChasingState();
            }
            else if (IsWandering)
            {
                WanderingState();
            }
            else if (IsGoingToTeritory)
            {
                GoingToTerritoryState();
            }  
            else if (WaitingState)
            {
                WaitState();
            }

            //agent.angularSpeed = AngularSpeed;

            agent.speed = EnemySpeed;           
        }

        private void ChasingState()
        {
            EnemySpeed = ChasingSpeed;


            if (agent.velocity == new Vector3(0, 0, 0) && distance > lookRadius)
            {              
                IsWandering = true;

                IsChasing = false;
            }
            
            if (distance <= AttackStateDistance)
            {
                IsAttacking = true;

                IsChasing = false;
            }

            FaceTarget();

            //Enemy Is in it's territory
            if (InTerritory)
            {          
                if (distance <= lookRadius)
                {
                    agent.SetDestination(target.position);

                    LastPlayerPosition.transform.position = MovementPlayer.transform.position;

                    if (distance <= agent.stoppingDistance)
                    {
                        //Attack
                        //face target
                        FaceTarget();

                        //Debug.Log("ChasingFaceTarget");
                    }
                }
            }
            //Enemy Is out it's Territory
            else if (!InTerritory)
            {

                if (distance <= lookRadius)
                {
                    if (agent.enabled )
                    {
                        agent.SetDestination(target.position);
                    }                 
                   
                    LastPlayerPosition.transform.position = MovementPlayer.transform.position;

                    if (distance <= agent.stoppingDistance)
                    {
                        //Attack
                        //face target
                        FaceTarget();
                    }
                }
                else if (distance >= lookRadius)
                {
                    if (agent.enabled)
                    {
                        agent.SetDestination(LastPlayerPosition.transform.position);
                    }                                 
                }
            }
        }

        private void WanderingState()
        {
            EnemySpeed = WanderSpeed;

            if (distance <= lookRadius)
            {
                IsChasing = true;

                IsWandering = false;
            }
           
            if (distance <= AttackStateDistance)
            {
                IsAttacking = true;

                IsWandering = false;
            }                      

            if (!InTerritory)
            {
                StartCoroutine(WaitBeforeGoingToTerritory());

                if (CanGoBackToTerritory)
                {
                    IsGoingToTeritory = true;

                    IsWandering = false;

                    CanGoBackToTerritory = false;
                }         
            }

            if (CanStartCoroutine == true)
            {
                WanderPosition.transform.position = Random.insideUnitSphere * WanderDistance + transform.position;
            }
           
            if (Physics.Raycast(WanderPosition.transform.position, new Vector3(0, -0.5f, 0), WhatIsGround))
            {
                if (CanStartCoroutine == true)
                {
                    StartCoroutine(WanderWaitTimeCoroutine());
                }

                CanStartCoroutine = false;

                if (agent.enabled)
                {
                    agent.SetDestination(WanderPosition.transform.position);
                }
                else
                {
                    //Debug.Log("Agent isn't enabled in Wandering state");
                }
               
            }
        }

        private void GoingToTerritoryState()
        {
           EnemySpeed = ChasingSpeed;

            if (agent.enabled)
            {
                agent.SetDestination(TerritoryCharger.transform.position);
            }

            if (agent.velocity == new Vector3(0, 0, 0))
            {
                IsWandering = true;

                IsGoingToTeritory = false;
            }

            if(distance <= lookRadius)
            {
                IsChasing = true;

                IsGoingToTeritory = false;
            }
            
            
            if (distance <= AttackStateDistance)
            {
                IsAttacking = true;

                IsGoingToTeritory = false;
            }
        }

        private void AttackingState()
        {
            FaceTarget();

            if (CanCharge)
            {
                EnemySpeed = ChargingSpeed;

                CanKnockBack = true;

                agent.SetDestination(ChargePosition.transform.position);
            }
            else if (WaitingState)
            {
                IsAttacking = false;

                WaitingState = true;
            }
            else
            {
                EnemySpeed = AttackingSpeed;

                agent.SetDestination(target.position);

                FaceTarget();

                CanCharge = false;

                if (CanStartWaitTimeCoroutine)
                {
                    coroutine = WaitTimeBeforeAttack(TimeWaitBeforeAttack);
                    StartCoroutine(coroutine);

                    CanStartWaitTimeCoroutine = false;
                }
            }

            if (distance > AttackStateDistance)
            {
                if (distance < lookRadius)
                {
                    IsChasing = true;

                    IsAttacking = false;

                    CanCharge = false;

                    CanStartWaitTimeCoroutine = true;
                }
                else if (distance > lookRadius)
                {
                    IsWandering = true;

                    IsAttacking = false;

                    CanCharge = false;

                    CanStartWaitTimeCoroutine = true;
                }
            }
        }

        private void WaitState()
        {
            EnemySpeed = 0;

            rb.constraints = RigidbodyConstraints.FreezeRotation;
            //rb.constraints = RigidbodyConstraints.FreezePosition;

            //AngularSpeed = 0;

            //agent.SetDestination(ChargePosition.transform.position);

            CanCharge = false;
        }

        //Check if enemy and player collide
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                // Could be cool to deal damage from here.

                // Add KnockBack to the player
                if (CanKnockBack)
                {
                    collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * KnockBackForceBack + transform.up * KnockBackForceUp, ForceMode.Impulse);
                    Debug.Log("KnockBackForce");

                    CanKnockBack = false;
                }
            }

            if (CanCharge)
            {
                WaitChargedCoroutine = WaitTimeAfterCharged(TimeWaitAfterCharged);
                StartCoroutine(WaitChargedCoroutine);
                //Debug.Log("Started WaitTimeAfter Charged coroutine");
            }

            CanCharge = false;

            //Debug.Log("CanChargeFalse? = " + CanCharge);
        }

        /*
        // Waiting before checking if the enemy is on the ground after jumping.
        private IEnumerator WaitBeforeGrounding(float TimeToWaitGrounding)
        {
            yield return new WaitForSeconds(TimeToWaitGrounding);

            CanStartGroundCheck = true;

            //Debug.Log("CanStartGroundCheck");
        }
        */

        
        // Waiting before attacking the player
        private IEnumerator WaitTimeBeforeAttack(float TimeWaitBeforeAttack)
        {
            yield return new WaitForSeconds(TimeWaitBeforeAttack);

            print("WaitTimeCoroutine ended: " + Time.time + " seconds");

            CanCharge = true;

            //Debug.Log("CanCharge = " + CanCharge);
        }

        private IEnumerator WaitTimeAfterCharged(float TimeWaitAfterCharged)
        {
            WaitingState = true;

            //Debug.Log("Started afterCharged coroutine");

            yield return new WaitForSeconds(TimeWaitAfterCharged);

            rb.constraints = RigidbodyConstraints.None;

            WaitingState = false;

            if (distance <= AttackStateDistance)
            {
                IsAttacking = true;

                WaitingState = false;
            }
            else if (distance <= lookRadius)
            {
                IsChasing = true;

                WaitingState = false;
            }
            else if (agent.velocity == new Vector3(0, 0, 0))
            {
                IsWandering = true;

                WaitingState = false;
            }

            // Just testing
            CanStartWaitTimeCoroutine = true;

            //Debug.Log("Finished afterCharged coroutine");

            //CanChargeAgain = true;
        }

        void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        //Enemy Enters i'ts territory.
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "EnemyTerritory")
            {
                InTerritory = true;
            }
        }

        //Enemy Leaves it's Territory.
        private void OnTriggerExit(Collider other)
        {

            if (other.gameObject.tag == "EnemyTerritory")
            {
                InTerritory = false;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, AttackStateDistance);
        }

        //Wait Random amount of time before coroutine can be started again.
        private IEnumerator WanderWaitTimeCoroutine()
        {
            yield return new WaitForSeconds(Random.Range(0, RandomUpperWanderWaitTime));

            CanStartCoroutine = true;
        }

        // Time to wait in wander state before going back to it's territory.
        private IEnumerator WaitBeforeGoingToTerritory()
        {
            yield return new WaitForSeconds(Random.Range(LowerWanderOutsideTerritory, UpperWanderOutsideTerritory));

            CanGoBackToTerritory = true;
        }
    }  
}
