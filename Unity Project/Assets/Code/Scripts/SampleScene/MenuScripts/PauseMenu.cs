using Code.Scripts.SampleScene.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.SampleScene.MenuScripts
{
    public class PauseMenu : MonoBehaviour
    {


        public static bool GameIsPaused = false;

        public GameObject pauseMenuUI;

        [SerializeField] GameObject InventoryScript;

        private InventoryScript inventory;

        //[SerializeField] GameObject pauseMenu;


        // Start is called before the first frame update
        void Start()
        {

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

        public void LoadMenu()
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1f;
            GameIsPaused = false;

        }

    }
}
