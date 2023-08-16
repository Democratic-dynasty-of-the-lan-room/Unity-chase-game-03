using UnityEngine;
using UnityEngine.AI;

namespace Code.Scripts.SampleScene
{
    public class EnemyController : MonoBehaviour
    {

        [SerializeField] GameObject Player;

        [SerializeField] GameObject PauseMenu;

        [SerializeField] GameObject InventoryScript;

        Transform target;
        NavMeshAgent agent;


        public float lookRadius = 10;

        public GameObject RestartMenu;

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

       

    

            //Making sure Player is set to true? I guess
            Player.SetActive(true);

            //this script I think
            this.enabled = true;

            ;
        }

        // Update is called once per frame
        void Update()
        {
            float distance = Vector3.Distance(target.position, transform.position);

            if (distance <= lookRadius)
            {
                agent.SetDestination(target.position);

                if (distance <= agent.stoppingDistance)
                {
                    //Attack
                    //face target
                    FaceTarget();
                }
            }
        }

        void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3 (direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);  
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);

        }

    }
}
