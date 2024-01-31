using Code.Scripts.SampleScene.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.SampleScene.MenuScripts
{
    public class PauseMenu : MonoBehaviour
    {
        //public bool Restarted;

        public static bool GameIsPaused = false;

        public GameObject pauseMenuUI;

        //[SerializeField] GameObject Player;

        [SerializeField] GameObject InventoryScript;

        private InventoryScript inventory;

        public GameData GameDataScript;

        public PlayerMovment PlayerMovementScript;

        //[SerializeField] GameObject pauseMenu;


        // Start is called before the first frame update
        void Start()
        {           
            //GameIsPaused = false;
            pauseMenuUI.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
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
            InventoryScript.SetActive(true);         
        }

        void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            InventoryScript.SetActive(false);
        }

        public void Restart()
        {
            // This is bad remove this at some point. This is just for now ease of use.
            PlayerMovementScript.transform.position = GameDataScript.RestartPosition;

            GameDataScript.RestartLevel();

            DataPersistenceManager.instance.SaveGame();

            Time.timeScale = 1f;
            GameIsPaused = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadMenu()
        {
            //save the game anytime before loading a new scene check this works
            DataPersistenceManager.instance.SaveGame();

            SceneManager.LoadScene(0);
            Time.timeScale = 1f;
            GameIsPaused = false;
        } 

    }
}
