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
            if (Interactable.LeverIsUp)
            {

                anim.Play("PortCullusOpen");



            }
            else if (!Interactable.LeverIsUp)
            {

                anim.Play("PortCullusClose");

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
