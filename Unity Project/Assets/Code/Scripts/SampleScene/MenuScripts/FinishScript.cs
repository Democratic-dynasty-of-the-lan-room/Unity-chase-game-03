using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Code.Scripts.SampleScene
{
    public class FinishScript : MonoBehaviour, IDataPersistence
    {
        [Header("SceneSelecting")]
        public int SceneNumber;

        public bool NextScene;

        [Header("Spawn Position, You must ONLY have one of these selected")]
        public bool Spawn1;
        public bool Spawn2;
        public bool Spawn3;

        [Header("PositionToSpawn")]
        public int PositionToSpawn;



        public bool CanSetSpawnpoint;

        public bool ThisFinish;

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

            //CanSetSpawnpoint = false;

            ThisFinish = false;
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
            // SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex.Bui);

            

            SceneManager.LoadScene(SceneNumber, LoadSceneMode.Single);
        }

        public void SpawnPosition()
        {
            CanSetSpawnpoint = true;

            ThisFinish = true;

            /*if (Spawn1)
            {
                PositionToSpawn = 0;
            }
            else if (Spawn2)
            {
                PositionToSpawn = 1;
            }
            else if (Spawn3)
            {
                PositionToSpawn = 2;
            }*/
        }

        // these are Loading and saving game data
        public void LoadData(GameData data)
        {
            //CanSetSpawnpoint = data.CanSetSpawn;
        }

        public void SaveData(ref GameData data)
        {
            //data.CanSetSpawn = CanSetSpawnpoint;
        }

        public void RestartLoadData(CheckPointData CheckPointData)
        {
           
        }

        public void RestartSaveData(ref CheckPointData CheckPointData)
        {
            
        }
    }
}
