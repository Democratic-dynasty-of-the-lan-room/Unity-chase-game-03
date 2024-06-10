using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.SampleScene.MenuScripts
{
    public class RestartMenu : MonoBehaviour
    {

        [SerializeField] GameObject restartMenu;

        public GameData gameDataScript;

        public PlayerHealth playerHealth;

        public PlayerStartPos playerStartPos;

        // Start is called before the first frame update
        void Start()
        {
            // making sure RestartMenu is off at the start
            restartMenu.SetActive(false);
        }

        //Restarts when you click on restart
        public void Restart()
        {
           playerHealth.Health = playerHealth.SetHealthAmount;

            //Debug.Log("HealthRestart" + playerHealth.Health);

            DataPersistenceManager.instance.RestartLoadGame();

            DataPersistenceManager.instance.SaveGame();

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //Loads menu scene
        public void BackToMenu()
        {

            // I need to make it so that the restart menu isn't active when you go back to the level. Menu Active Manager script or something.
            playerStartPos.SceneToLoad = SceneManager.GetActiveScene().buildIndex;

            DataPersistenceManager.instance.RestartLoadGame();

            //save the game anytime before loading a new scene check this works
            DataPersistenceManager.instance.SaveGame();

            SceneManager.LoadScene(0);

            //Load The PlayerStartPos.SceneToLoad To load The saved scene.
            //SceneManager.LoadScene(playerStartPos.SceneToLoad, LoadSceneMode.Single);
        }
    }
}
