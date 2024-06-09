using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

namespace Code.Scripts.SampleScene
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] GameObject FirstEnemyTerritory;

        [SerializeField] GameObject LastPlayerPosition;

        [SerializeField] GameObject Player;

        [SerializeField] GameObject PauseMenu;

        [SerializeField] GameObject InventoryScript;

        [SerializeField] AttackPlayerTrigger attackPlayerTrigger;

        [SerializeField] LayerMask WhatIsGround;

        [SerializeField] ParticleSystem particleSystem;
 
        //Animator animator;

        Transform target;
        NavMeshAgent agent;

        public float angleThresholdTerritory = 30.0f;



        public float lookRadius = 10;

        private float distance;

        public GameObject RestartMenu;

        bool InTerritory;

        public bool CanStopEnemySpeed;

       //State Machine  

       [Header("Chasing state")]

        public float ChasingSpeed;



        public float EnemySpeed;

        [Header("Attacking State")]

        public float AttackSlowDistance = 15;

        public float AttackingSpeed;

        private bool PlayParticles = true;



        // WanderState
        [Header("Wander state")]

        public GameObject WanderPosition;    

        public float WanderDistance;

        public float RandomUpperWanderWaitTime;

        public float UpperWanderOutsideTerritory;

        public float LowerWanderOutsideTerritory;

        public float WanderSpeed;



        public bool IsChasing = false;

        public bool IsWandering = true;

        public bool IsGoingToTeritory = false;

        public bool IsAttacking;


        private bool CanGoBackToTerritory;

        private bool CanStartCoroutine;

        





        //public float Speed;

        public float Acceleration = 1f;

        public float Deceleration = -1f;

        public float DesiredEnemySpeed;

        public float DecelerationDistance;

        //Check if enemy and player collide
        void OnCollisionEnter(Collision collision)
        {
            /*if (collision.gameObject.tag == "Player")
            {
                //If player touches chaser game end
                RestartMenu.SetActive(true);
                // setting the player to false so that you can't move after the end
                Player.SetActive(false);
                //Enabling this
                this.enabled = false;

                //Showscursor so that you can click restart
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                PauseMenu.SetActive(false);
                InventoryScript.SetActive(false);
            }*/
        }

        // Start is called before the first frame update
        void Start()
        {            
            target = PlayerManager.instance.player.transform;
            agent = GetComponent<NavMeshAgent>();

            if (particleSystem == null)
            {
                //particleSystem = GetComponentInChildren<ParticleSystem>();
            }
            particleSystem.Stop();

            CanStartCoroutine = true;

            CanGoBackToTerritory = false;

            //Making sure Player is set to true? I guess
            Player.SetActive(true);

            //this script I think
            this.enabled = true;
        }

        void FixedUpdate()
        {
            // Controls Enemy speeding up and slowing down. I'm not very happy with this code
            if (agent.remainingDistance <= DecelerationDistance && distance > lookRadius && EnemySpeed > -1)
            {
                EnemySpeed -= Deceleration;
            }
            else if (EnemySpeed <= DesiredEnemySpeed)
            {
                EnemySpeed += Acceleration;
            }
            else if (EnemySpeed > DesiredEnemySpeed && EnemySpeed > 0)
            {
                EnemySpeed -= Deceleration;
            }
        }

        // Update is called once per frame
        void Update()
        {
            distance = Vector3.Distance(target.position, transform.position);

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

            Animator animator = GetComponentInChildren<Animator>();
            
            // animation is set to 0 when there is no velocity
            if (agent.velocity == new Vector3(0, 0, 0) && CanStopEnemySpeed)
            {
                //Debug.Log("Agent isn't moving");

                CanStopEnemySpeed = false;

                animator.SetFloat("Speed", 0);
                //animator.SetFloat("Speed", agent.velocity.magnitude);// why not this line here?

                //EnemySpeed = 0;
            }
            else
            {
                //CanStopEnemySpeed = true;

                animator.SetFloat("Speed", agent.velocity.magnitude);
            }

            if (agent.velocity != new Vector3(0, 0, 0))
            {
                CanStopEnemySpeed = true;
            }


            //FaceDirection();
        }

        private void ChasingState()
        {
            DesiredEnemySpeed = ChasingSpeed;

            //EnemySpeed = ChasingSpeed;


            if (agent.velocity == new Vector3(0, 0, 0) && distance > lookRadius)
            {              
                IsWandering = true;

                IsChasing = false;
            }
            
            if (distance <= AttackSlowDistance)
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
                    agent.SetDestination(target.position);

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
                    agent.SetDestination(LastPlayerPosition.transform.position);                        
                }
            }
        }

        private void WanderingState()
        {
            DesiredEnemySpeed = WanderSpeed;

            //EnemySpeed = WanderSpeed;


            if (distance <= lookRadius)
            {
                IsChasing = true;

                IsWandering = false;
            }
           
            if (distance <= AttackSlowDistance)
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

                agent.SetDestination(WanderPosition.transform.position);                    
            }        
        }

        private void GoingToTerritoryState()
        {
            DesiredEnemySpeed = ChasingSpeed;

            //EnemySpeed = ChasingSpeed;

            agent.SetDestination(FirstEnemyTerritory.transform.position);

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
            
            
            if (distance <= AttackSlowDistance)
            {
                IsAttacking = true;

                IsGoingToTeritory = false;
            }                      
        }

        private void AttackingState()
        {
            DesiredEnemySpeed = AttackingSpeed;

            FaceTarget();

            // The problem is that I didn't add this line here lol.
            agent.SetDestination(target.position);

            if (distance <= AttackSlowDistance)
            {               
                if (PlayParticles)
                {
                    particleSystem.Play();

                    PlayParticles = false;
                }
            }
            else
            {
                particleSystem.Stop();

                PlayParticles = true;

                if (distance < lookRadius)
                {
                    IsChasing = true;

                    IsAttacking = false;

                    Debug.Log("Chasing");
                }
                else if (distance > lookRadius)
                {
                    IsWandering = true;

                    IsAttacking = false;

                    Debug.Log("Exit Attack");
                }
            }
        }
        

        void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3 (direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);  
        }

        void FaceDirection()
        {
            Vector3 direction = (agent.destination - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        //Enemy Enters i'ts territory
        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.tag == "EnemyTerritory")
            {
                InTerritory = true;                                                              
            }
        }

        //Enemy Leaves it's Territory
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

        //Wait Random amount of time before coroutine can be started again
        private IEnumerator WanderWaitTimeCoroutine()
        { 
            yield return new WaitForSeconds(Random.Range(0, RandomUpperWanderWaitTime));

            CanStartCoroutine = true;      
        }

        private IEnumerator WaitBeforeGoingToTerritory()
        {
            yield return new WaitForSeconds(Random.Range(LowerWanderOutsideTerritory, UpperWanderOutsideTerritory));

            CanGoBackToTerritory = true;
        }
    }  
}
