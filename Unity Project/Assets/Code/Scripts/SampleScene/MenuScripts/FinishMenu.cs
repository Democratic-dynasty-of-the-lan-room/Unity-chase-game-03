using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.SampleScene.MenuScripts
{
    public class FinishMenu : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        //Restarts scene
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //Loads next scene
        public void NextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        //Loads Main Menu scene
        public void BackToMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
