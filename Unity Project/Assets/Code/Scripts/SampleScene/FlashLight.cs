using System;
using System.Collections;
using UnityEngine;

namespace Code.Scripts.SampleScene
{
    public class FlashLight : MonoBehaviour
    {
        private IEnumerator Coroutine;

        bool flash = false;

        private Light Torchlight;

        public float batteryTime;
        public float BatteryDownAmount;

        private bool CanCoroutine;

        //private float testLightIntensity = 1;

        [SerializeField] double MaxLightRange = 0.001;
        [SerializeField] double MinLightRange = -0.001;
        [SerializeField] float DefaultLightIntensity = 2;

        // Start is called before the first frame update
        void Start()
        {
            Torchlight = this.GetComponent<Light>();
            Torchlight.enabled = false;

            CanCoroutine = true;

            //CentralBattery.Instance.CurrentCharge = 10;
            //Debug.Log("Test In start");
        }

        private void FixedUpdate()
        {
            if (flash && CanCoroutine)
            {
                Coroutine = CoroutineBattery(batteryTime);
                StartCoroutine(Coroutine);

                CanCoroutine = false;

                Debug.Log("Start Coroutine");
            }
        }

        // Update is called once per frame
        //Turning torch on and off with two functions
        void Update()
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (Input.GetKeyDown(KeyCode.F)) 
            {
                
                if(flash == false)
                {
                    if (CentralBattery.Instance.CurrentCharge != 0)
                    {
                        On();

                        Debug.Log("Hello from Flashlight On");
                    }      
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

                //Debug.Log("MouseWheel");             
            }
            if (scrollInput < 0 && Torchlight.range > MinLightRange)
            {
                Torchlight.spotAngle++;
                Torchlight.range--;               
            }          
            if (Torchlight.range > 80 && scrollInput > 0 || Torchlight.range < 30 && scrollInput < 0 && Torchlight.range < MaxLightRange && Torchlight.range > MinLightRange)
            {
                
                Torchlight.intensity -= Mathf.Lerp(0.04f, 0f, 0f);
            }
            else if (Torchlight.range > 80 && scrollInput < 0 || Torchlight.range < 30 && scrollInput > 0)
            {
                Torchlight.intensity += Mathf.Lerp(0.04f, 0f, 0f);
            }
            else if (Torchlight.range < 80 && Torchlight.range > 30)
            {
                Torchlight.intensity = DefaultLightIntensity;
            }

            if (CentralBattery.Instance.CurrentCharge <= 0f)
            {
                off();
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

        private IEnumerator CoroutineBattery(float batteryTime)
        {
            Debug.Log("Start Time Coroutine");

            yield return new WaitForSeconds(batteryTime);

            CentralBattery.Instance.CurrentCharge -= BatteryDownAmount;

            CanCoroutine = true;

            Debug.Log("Remove Battery Amount: " + CentralBattery.Instance.CurrentCharge);
        }
    }
}
