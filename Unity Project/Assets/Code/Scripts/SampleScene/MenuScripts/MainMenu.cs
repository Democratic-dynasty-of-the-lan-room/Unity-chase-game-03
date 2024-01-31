using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Scripts.SampleScene.MenuScripts
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button continueGameButton;

        private void Start()
        {
            if (!DataPersistenceManager.instance.HasGameData())
            {
                continueGameButton.interactable = false;
            }
        }


        //If play is clicked load next scene
        public void PlayGame()
        {
            DisableMenuButtons();

            // save the game anytime before loading a new scene
            DataPersistenceManager.instance.SaveGame();

            // Load the next scene - which will in turn load the game because of 
            // OnSceneloaded() in the DataPersistenceManager
            // this needs to change as the load doesn't work?"??????????
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void ButtonNewGame()
        {
            DisableMenuButtons();
            // create a new game - which will initialize our game data
            DataPersistenceManager.instance.NewGame();
            //SavingNewDataSoThatItDoesn'tJustLoadToTheOldDataWhenChanginScenes
            DataPersistenceManager.instance.SaveGame();
            // Load the gameplay scene - which will in turn save the game because of
            // OnSceneUnloaded() in the DataPersistenceManager
            // unload doesn't work
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        //Quits game 
        public void QuitGame()
        {
            Debug.Log("Quit!");
            Application.Quit();
        }

        private void DisableMenuButtons()
        {
            newGameButton.interactable = false;
            continueGameButton.interactable = false;
        }
    }
}
