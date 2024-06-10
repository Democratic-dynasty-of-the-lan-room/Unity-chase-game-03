using Code.Scripts.SampleScene.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.SampleScene.MenuScripts
{
    public class PauseMenu : MonoBehaviour, IDataPersistence
    {
        public static bool GameIsPaused = false;

        public GameObject pauseMenuUI;

        [SerializeField] GameObject InventoryScript;

        [SerializeField] PlayerStartPos playerStartPos;

        [SerializeField] PlayerHealth playerHealth;

        private InventoryScript inventory;

        public GameData GameDataScript;

        public PlayerMovment PlayerMovementScript;

        // Start is called before the first frame update
        void Start()
        {           
            //GameIsPaused = false;
            pauseMenuUI.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            // This isn't on the new input system yet.
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPaused)
                {
                    Resume();               
                }
                else
                {
                    Pause();
                }
            }
        }

        public void Resume()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GameIsPaused = false;
            //InventorfyScript.SetActive(true);
            //PlayerMovementScript.enabled = false;
            InventoryScript.GetComponent<InventoryScript>().enabled = true;
        }

        void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //InventoryScript.SetActive(false);
            //PlayerMovementScript.enabled = false;
            InventoryScript.GetComponent<InventoryScript>().enabled = false;
        }

        public void Restart()
        {
            // Set Player health to full
            playerHealth.Health = playerHealth.SetHealthAmount;

            DataPersistenceManager.instance.RestartLoadGame();

            DataPersistenceManager.instance.SaveGame();

            Time.timeScale = 1f;
            GameIsPaused = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadMenu()
        {       
            playerStartPos.SceneToLoad = SceneManager.GetActiveScene().buildIndex;

            //save the game anytime before loading a new scene check this works
            DataPersistenceManager.instance.SaveGame();

            SceneManager.LoadScene(0);
            Time.timeScale = 1f;
            GameIsPaused = false;
        }

        public void LoadData(GameData data)
        {
           
        }

        public void SaveData(ref GameData data)
        {
           
        }

        public void RestartLoadData(CheckPointData CheckPointLoadData)
        {
            
        }

        public void RestartSaveData(ref CheckPointData CheckPointSaveData)
        {

        }

    }
}
