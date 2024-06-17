using Code.Scripts.SampleScene.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PortCullusScript : MonoBehaviour
{
    private Animation anim;

    [SerializeField] LeverScript Interactable;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {           
        if(anim.isPlaying == false && Interactable.IsPressed == true)
        {
            if (Interactable.LeverIsUp && !Interactable.anim.isPlaying)
            {
                anim.Play("PortCullusOpen");

                // Animate The Lever
                Interactable.anim.Play("BoneLeverAnimation");

                AudioManager.instance.PlayOneShot(FMODEvents.instance.LeverSound, Interactable.transform.position);
            }
            else if (!Interactable.LeverIsUp && !Interactable.anim.isPlaying)
            {
                anim.Play("PortCullusClose");

                // Animate The lever
                Interactable.anim.Play("BoneLeverBackAnim");

                AudioManager.instance.PlayOneShot(FMODEvents.instance.LeverSound, Interactable.transform.position);
            }           
        }
        else if (anim.isPlaying == true)
        {
            if (Interactable.LeverIsUp && Interactable.IsPressed == true)
            {
                Interactable.LeverIsUp = false;
            }
            else if (!Interactable.LeverIsUp && Interactable.IsPressed == true)
            {
                Interactable.LeverIsUp = true;
            }                
        }         
    }
}
