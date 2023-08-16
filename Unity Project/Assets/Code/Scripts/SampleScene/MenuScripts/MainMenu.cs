using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.SampleScene.MenuScripts
{
    public class MainMenu : MonoBehaviour
    {
        //If play is clicked load next scene
        public void PlayGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        //Quits game 
        public void QuitGame()
        {
            Debug.Log("Quit!");
            Application.Quit();
        }
    }
}
