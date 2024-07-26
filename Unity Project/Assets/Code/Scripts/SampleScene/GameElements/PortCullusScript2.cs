using Code.Scripts.SampleScene.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PortCullusScript2 : MonoBehaviour
{
    private Animation anim;

    [SerializeField] LeverScript2Point0 Lever;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {      
        /*
        if(anim.isPlaying == false && Lever.IsPressed == true)
        {
            Lever.IsPressed = false;
            //Debug.Log("PortCulluspresed");

            Debug.Log("LeverPressed");

            if (Lever.LeverIsUp && !Lever.anim.isPlaying)
            {
                anim.Play("PortCullusOpen");

                // Animate The Lever
                Lever.anim.Play("BoneLeverAnimation");

                AudioManager.instance.PlayOneShot(FMODEvents.instance.LeverSound, Lever.transform.position);
            }
            else if (!Lever.LeverIsUp && !Lever.anim.isPlaying)
            {
                anim.Play("PortCullusClose");

                // Animate The lever
                Lever.anim.Play("BoneLeverBackAnim");

                AudioManager.instance.PlayOneShot(FMODEvents.instance.LeverSound, Lever.transform.position);
            }           
        }
        else if (anim.isPlaying == true)
        {
            Lever.IsPressed = false;

            if (Lever.LeverIsUp && Lever.IsPressed == true)
            {
                Lever.LeverIsUp = false;
            }
            else if (!Lever.LeverIsUp && Lever.IsPressed == true)
            {
                Lever.LeverIsUp = true;
            }
        }
        */


        if (Lever.IsPressed)
        {
            if (anim.isPlaying || Lever.anim.isPlaying)
            {
                Lever.LeverLocked = true;
            }
            else
            {
                Lever.LeverLocked = false;

                RunLever();
            }       

            Lever.IsPressed = false;
        }
    }


    private void RunLever()
    {
        if (Lever.LeverIsUp)
        {
            Debug.Log("Open");

            anim.Play("PortCullusOpen");
            Lever.anim.Play("BoneLeverAnimation");
        }
        else
        {
            Debug.Log("Close");

            anim.Play("PortCullusClose");
            Lever.anim.Play("BoneLeverBackAnim");
        }
    }
}
