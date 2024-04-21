using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace Code.Scripts.SampleScene.Player
{
    public class LeverScript : MonoBehaviour
    {       
        [SerializeField] GameObject EToPressLever;

        public float rayLength;

        public bool LeverIsUp;

        public bool IsPressed;

        // Start is called before the first frame update
        private void Start()
        {
            EToPressLever.SetActive(false);

            LeverIsUp = false;

            IsPressed = false;
        }

        //Update is called once per frame    
        //Checks if you are pressing on the Lever then changes Lever Bool on or of
        void Update()
        {

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayLength))
            {

                var selection = hit.transform;
                if (selection.CompareTag("LeverTag"))
                {

                    EToPressLever.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        IsPressed = true;

                        // Change bool of lever to be true or false               
                        if(!LeverIsUp)
                        {
                            LeverOn();

                            Debug.Log("LeverTrue");
                        }
                        else if (LeverIsUp)
                        {
                            LeverFalse();
                          
                            Debug.Log("LeverFalse");
                        }
                    }
                    else
                    {
                        IsPressed = false;
                    }
                }      
                else
                {
                    EToPressLever.SetActive(false);
                }
            }
            else
            {
                EToPressLever.SetActive(false);
            }
        }

        //Turns lever up and Lever isn't down
        public void LeverOn()
        {
            LeverIsUp = true;
        }

        //Turns lever down and Lever isn't up
        public void LeverFalse()
        {
            LeverIsUp = false;
        }
    }
}
