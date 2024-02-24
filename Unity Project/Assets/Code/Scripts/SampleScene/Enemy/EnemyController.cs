using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace Code.Scripts.SampleScene
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] GameObject FirstEnemyTerritory;

        [SerializeField] GameObject LastPlayerPosition;

        [SerializeField] GameObject Player;

        [SerializeField] GameObject PauseMenu;

        [SerializeField] GameObject InventoryScript;

        [SerializeField] LayerMask WhatIsGround;

        Animator animator;

        Transform target;
        NavMeshAgent agent;



        public float lookRadius = 10;

        private float distance;

        public GameObject RestartMenu;

        bool InTerritory;

        //State Machine  

        [Header("Chasing state")]

        public float ChasingSpeed;



        public float EnemySpeed;



        // WanderState
        [Header("Wander state")]

        public GameObject WanderPosition;    

        public float WanderDistance;

        public float RandomUpperWanderWaitTime;

        public float UpperWanderOutsideTerritory;

        public float LowerWanderOutsideTerritory;

        public float WanderSpeed;



        private bool IsChasing = false;

        private bool IsWandering = true;

        private bool IsGoingToTeritory = false;

        private bool CanGoBackToTerritory;

        private bool CanStartCoroutine;

        public float Speed;

        public float Acceleration = 0.1f;

        //Check if enemy and player collide
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
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
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            target = PlayerManager.instance.player.transform;
            agent = GetComponent<NavMeshAgent>();

            CanStartCoroutine = true;

            CanGoBackToTerritory = false;

            //Making sure Player is set to true? I guess
            Player.SetActive(true);

            //this script I think
            this.enabled = true;
        }

        // Update is called once per frame
        void FixedUpdate()
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

            agent.speed = EnemySpeed;

            Animator animator = GetComponentInChildren<Animator>();

            
            
            if (agent.velocity == new Vector3(0, 0, 0)) 
            {
                animator.SetFloat("Speed", 0);
            }
            else
            {
                animator.SetFloat("Speed", EnemySpeed);
            }
        }

        private void ChasingState()
        {
            EnemySpeed = ChasingSpeed;

            if (agent.velocity == new Vector3(0, 0, 0))
            {
                IsWandering = true;

                IsChasing = false;
            }

            //Enemy Is in i'ts territory
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
            EnemySpeed = WanderSpeed;

            if (distance <= lookRadius)
            {
                IsChasing = true;

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
                WanderPosition.transform.position = UnityEngine.Random.insideUnitSphere * WanderDistance + transform.position;
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
            EnemySpeed = ChasingSpeed;

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
        }

        void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3 (direction.x, 0, direction.z));
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
