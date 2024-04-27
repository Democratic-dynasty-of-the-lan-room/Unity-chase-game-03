using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Scripts.SampleScene.MenuScripts
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button continueGameButton;

        [SerializeField] PlayerStartPos playerStartPos;

        private bool NoGameDataCheck;

        private void Start()
        {
            // Check if there is GameData in fixed update lol
            NoGameDataCheck = true;

            Debug.Log("Checking if there is Game Data: " + DataPersistenceManager.instance.HasGameData());
        }

        public void FixedUpdate()
        {
            if (!DataPersistenceManager.instance.HasGameData() && NoGameDataCheck)
            {
                Debug.LogWarning("Is There no Game Data?");

                continueGameButton.interactable = false;

                NoGameDataCheck = false;
            }
            else
            {
                //Debug.Log("Is There no Game Data?" + DataPersistenceManager.instance.HasGameData());
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


            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            Debug.Log("Scene Number To Load: " + playerStartPos.SceneToLoad);

            //Load The PlayerStartPos.SceneToLoad
            SceneManager.LoadScene(playerStartPos.SceneToLoad, LoadSceneMode.Single);
        }

        public void ButtonNewGame()
        {
            DisableMenuButtons();

            // create a new game - which will initialize our game data
            DataPersistenceManager.instance.NewGame();

            // This I have set in New Game data but it was always false for some reason. So I am setting it here.
            playerStartPos.CanSetSpawn = true;

            // I Think this Should be already set in the New GameData, but for some reason it sets spawn to 3 so I am setting it here.
            playerStartPos.PositionToSpawn = 0;

            //Restart Data
            //DataPersistenceManager.instance.FirstRestartPosition();


            //And Saving Save Data So that It won't load the old data when canging scenes
            DataPersistenceManager.instance.RestartSaveGame();

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
