using UnityEngine;

namespace Code.Scripts.SampleScene
{
    public class FlashLight : MonoBehaviour
    {
    

        bool flash = false;

        private Light Torchlight;

        // private float testLightIntensity = 1;

        [SerializeField] double MaxLightRange = 0.001;
        [SerializeField] double MinLightRange = -0.001;

        // Start is called before the first frame update
        void Start()
        {      
        
            Torchlight = this.GetComponent<Light>();
            Torchlight.enabled = false;      
        }

        // Update is called once per frame
        //Turning torch on and off with two functions
        void Update()
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (Input.GetKeyDown(KeyCode.F)) 
            {
                Debug.Log("Hello from Flashlight");
                if(flash == false)
                {
                    On();
                }
                else
                {
                    off();
                }
        
            }
            if (scrollInput > 0 && Torchlight.range < MaxLightRange)
            {
                Torchlight.spotAngle --;
                Torchlight.range++;          

                Debug.Log("MouseWheel");
            }
            if (scrollInput < 0 && Torchlight.range > MinLightRange)
            {
                Torchlight.spotAngle++;
                Torchlight.range--;

                // Torchlight.intensity = testLightIntensity + 0.1;
            }


        
        }

        //Onfunction
        private void On()
        {
            flash = true;
            Torchlight.enabled = true;
        }
        //Off function
        private void off()
        {
            flash = false;
            Torchlight.enabled = false;

        } 
    }
}
