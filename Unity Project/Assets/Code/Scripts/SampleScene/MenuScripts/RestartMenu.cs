using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.SampleScene.MenuScripts
{
    public class RestartMenu : MonoBehaviour
    {

        [SerializeField] GameObject restartMenu;

        public GameData gameDataScript;


        // Start is called before the first frame update
        void Start()
        {
            // making sure RestartMenu is off at the start
            restartMenu.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        //Restarts when you click on restart
        public void Restart()
        {
            DataPersistenceManager.instance.RestartLoadGame();

            DataPersistenceManager.instance.SaveGame();

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //Loads menu scene
        public void BackToMenu()
        {
            DataPersistenceManager.instance.RestartLoadGame();

            //save the game anytime before loading a new scene check this works
            DataPersistenceManager.instance.SaveGame();

            SceneManager.LoadScene(0);
        }
    }
}
