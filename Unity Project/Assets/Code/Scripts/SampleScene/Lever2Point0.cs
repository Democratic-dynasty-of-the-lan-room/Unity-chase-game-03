using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace Code.Scripts.SampleScene.Player
{
    public class LeverScript2Point0 : MonoBehaviour
    {
        // Animation Set To Play From Script effected by the lever.
        public Animation anim;

        public bool LeverIsUp;

        public bool IsPressed;

        public bool LeverLocked;

        // Start is called before the first frame update
        private void Start()
        {
            // Setting Bools Lever up may go through the save system at some point.
            LeverIsUp = false;

            IsPressed = false;

            anim = GetComponentInChildren<Animation>();
        }

        //Switches Lever Direction
        public void SwitchLever()
        {
            IsPressed = true;

            if (!LeverLocked)
            {
                // Change bool of lever to be true or false               
                if (!LeverIsUp)
                {
                    LeverIsUp = true;

                    //Debug.Log("Lever = " + LeverIsUp);
                }
                else if (LeverIsUp)
                {
                    LeverIsUp = false;

                    //Debug.Log("Lever = " + LeverIsUp);
                }
            }
            else
            {
                Debug.Log("LeverLocked");
            }
        }
    }
}
