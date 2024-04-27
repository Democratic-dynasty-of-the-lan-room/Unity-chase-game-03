using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Code.Scripts.SampleScene
{
    public class FinishScript : MonoBehaviour
    {
        [Header("SceneSelecting choose the scene list number of desired scene")]
        public int SceneNumber;


        [Header("If this is selected it will go to the next scene in the scene list")]
        public bool NextScene;

        [Header("Spawn Position in chosen level, You must ONLY have one of these selected")]
        public bool Spawn1;
        public bool Spawn2;
        public bool Spawn3;

        [SerializeField] PlayerStartPos playerStartPos;


        /*
        [SerializeField] GameObject Player;
        [SerializeField] NavMeshAgent navMeshAgent;
        [SerializeField] EnemyController EnemyController;

        //Getting all the menus
        [SerializeField] GameObject Enemy;
        [SerializeField] GameObject PauseMenu;
        [SerializeField] GameObject FinishMenu;
        [SerializeField] GameObject InventoryScript;

        */

        // Start is called before the first frame update
        void Start()
        {
            /*
            FinishMenu.SetActive(false);
            EnemyController.enabled = true;
            Player.SetActive(true);
            navMeshAgent.enabled = true;
            */
        }

        //Checks if the player is touching the door if so disable movment
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                if  (NextScene)
                {
                    SpawnPosition();

                    NextLevel();
                }
                else 
                {
                    SpawnPosition();                

                    LoadAnyScene();
                }

               
                
                // If we are going to have a menu that opens when you got through a door. I'm leaving this code here
                /*
                Player.SetActive(false);
            
                EnemyController.enabled = false;
                navMeshAgent.enabled = false;


                FinishMenu.SetActive(true);
                PauseMenu.SetActive(false);
                InventoryScript.SetActive(false);
                */
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;          
            }
        }

        public void NextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void LoadAnyScene()
        {
            SceneManager.LoadScene(SceneNumber, LoadSceneMode.Single);
        }

        public void SpawnPosition()
        {
            playerStartPos.CanSetSpawn = true;

            if (Spawn1)
            {
                playerStartPos.PositionToSpawn = 0;
            }
            else if (Spawn2)
            {
                playerStartPos.PositionToSpawn = 1;
            }
            else if (Spawn3)
            {
                playerStartPos.PositionToSpawn = 2;
            }

            DataPersistenceManager.instance.SaveGame();
        }
    }
}
