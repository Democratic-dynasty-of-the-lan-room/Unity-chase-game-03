using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

namespace Code.Scripts.SampleScene
{
    public class ScuttlerMovementScript : MonoBehaviour
    {
        Rigidbody rb;

        [SerializeField] GameObject FirstEnemyTerritory;

        [SerializeField] GameObject LastPlayerPosition;

        [SerializeField] GameObject Player;

        [SerializeField] LayerMask WhatIsGround;

        [SerializeField] GameObject RestartMenu;

        private IEnumerator coroutine;

        private IEnumerator GroundingCoroutine;

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

        [Tooltip("wait before checking if the enemy is on the ground or not after juming.")]
        public float TimeToWaitGrounding;

        public float DistanceToGround;

        public float UpwardsForce;

        public float ForwardForceAmount;

        private bool CanStartGroundCheck;

        private bool CanAddJumpForce;

        private bool CanAttackPlayer;


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
      
        //Check if enemy and player collide
        void OnCollisionEnter(Collision collision)
        {
            /*if (collision.gameObject.tag == "Player")
            {
                 // This could be useful to check
            }*/
        }

        // Start is called before the first frame update
        void Start()
        {            
            target = PlayerManager.instance.player.transform;
            agent = GetComponent<NavMeshAgent>();

            //Making sure Player is set to true? I guess
            Player.SetActive(true);

            //this script I think
            this.enabled = true;

            rb = GetComponent<Rigidbody>();


            // setting Booleans
            CanStartCoroutine = true;

            CanAttackPlayer = true;

            rb.isKinematic = true;

            CanAddJumpForce = false;

            CanGoBackToTerritory = false;
        }

        private void FixedUpdate()
        {
            // For Attacking state here.
            if (CanAddJumpForce)
            {
                rb.AddForce(this.transform.forward * ForwardForceAmount + this.transform.up * UpwardsForce, ForceMode.Impulse);

                CanAddJumpForce = false;
            }

            if (agent.enabled == false && CanStartGroundCheck)
            {
                Grounding();

                //Debug.Log("Agent Enabled");
            }
        }

        // Update is called once per frame
        private void Update()
        {
            distance = Vector3.Distance(target.position, transform.position);

            // Setting which states are active or inactive.
            if (IsChasing)
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

            if (IsAttacking)
            {
                AttackingState();
            }

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

            //Enemy Is in it's territory
            if (InTerritory)
            {          
                if (distance <= lookRadius)
                {
                    agent.SetDestination(target.position);

                    LastPlayerPosition.transform.position = Player.transform.position;

                    if (distance <= agent.stoppingDistance)
                    {
                        //Attack
                        //face target
                        FaceTarget();
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
                   
                    LastPlayerPosition.transform.position = Player.transform.position;

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
                agent.SetDestination(FirstEnemyTerritory.transform.position);
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
            EnemySpeed = AttackingSpeed;

            FaceTarget();

            if (agent.enabled == true)
            {
                agent.SetDestination(target.position);
            }          

            if (distance <= AttackStateDistance)
            {
                //Debug.Log("AttackingState");
                
                RaycastHit Hit;
                if (Physics.Raycast(transform.position, transform.forward, out Hit))
                {
                    if (Hit.transform.gameObject.CompareTag("Player"))
                    {
                        //Debug.Log("Player In Sight");
                       
                        if (CanAttackPlayer)
                        {
                            //Debug.Log("CanattackPlayer = " + CanAttackPlayer);

                            //Debug.Log("AlsoGrounded");

                            // Start Coroutine
                            coroutine = WaitTimeBeforeAttack(TimeWaitBeforeAttack);
                            StartCoroutine(coroutine);

                            CanAttackPlayer = false;
                        }                     
                    }              
                }
                else
                {
                    //Debug.Log("Player Not In Sight");
                }
                
            }
            else
            {
                if (distance < lookRadius)
                {
                    IsChasing = true;

                    IsAttacking = false;

                    //Debug.Log("Chasing");
                }
                else if (distance > lookRadius)
                {
                    IsWandering = true;

                    IsAttacking = false;

                    //Debug.Log("Exit Attack");
                }
            }
        }

        // Grounding must not set enemy to enabled right after the enemy has jumped
        private void Grounding()
        {
            RaycastHit Hit;
            if (Physics.Raycast(transform.position, Vector3.down, out Hit, DistanceToGround, WhatIsGround))
            {
                //Debug.Log("Racast Hit ground? Hit = " + Hit.transform.gameObject.name);

                agent.enabled = true;

                rb.isKinematic = true;

                rb.constraints = RigidbodyConstraints.None;

                //Debug.Log("agent.enabled = " + agent.enabled);

                CanStartGroundCheck = false;
            }
        }

        void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3 (direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);  
        }

        // Waiting before checking if the enemy is on the ground after jumping.
        private IEnumerator WaitBeforeGrounding(float TimeToWaitGrounding)
        {
            yield return new WaitForSeconds(TimeToWaitGrounding);

            CanStartGroundCheck = true;

            //Debug.Log("CanStartGroundCheck");
        }

        // Waiting before attacking the player
        private IEnumerator WaitTimeBeforeAttack(float TimeWaitBeforeAttack)
        {
            yield return new WaitForSeconds(TimeWaitBeforeAttack);

            print("WaitTimeCoroutine ended: " + Time.time + " seconds");

            //Debug.Log("WaitTimeBeforeAttack Couroutine");

            agent.enabled = false;

            rb.isKinematic = false;

            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            CanAttackPlayer = true;

            GroundingCoroutine = WaitBeforeGrounding(TimeToWaitGrounding);
            StartCoroutine(GroundingCoroutine);
            //Debug.Log("StartGroundingCoroutine");

            // This should probably only happen if grounded
            CanAddJumpForce = true;

            //Debug.Log("Jumped?");

            //Debug.Log("CanJump?: " + CanAddJumpForce);
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
