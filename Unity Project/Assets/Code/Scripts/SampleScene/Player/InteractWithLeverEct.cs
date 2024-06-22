using Code.Scripts.SampleScene.Player;
using UnityEngine;

public class InteractWithLeverEct : MonoBehaviour
{
    [SerializeField] GameObject EToPressLever;

    //[SerializeField] private string InteractableTag = "Interactable";

    PlayerInput playerInput;

    public float rayLength = 3;

    private void Awake()
    {
        playerInput = new PlayerInput();

        EToPressLever.SetActive(false);
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            //Debug.Log("RaycastWorking");
          
            var HitTransform = hit.transform;
            if (HitTransform.CompareTag("Interactable"))
            {
                EToPressLever.SetActive(true);

                //Debug.Log("RaycastWorking");
                var Collider = hit.collider.gameObject.GetComponent<LeverScript2Point0>();


                if (playerInput.GamePlay.Interact.triggered)
                {
                    // Why does this run 3 times???
                    //hit.collider.gameObject.GetComponent<LeverScript2Point0>().SwitchLever();
                   

                    //Collider.IsPressed = true;

                    // Switches the lever
                    if (hit.collider.gameObject.GetComponent<LeverScript2Point0>() == true)
                    {
                        //Debug.Log("Switch Lever");
                        
                        Collider.SwitchLever();
                    }
                    else
                    {
                        //Collider.IsPressed = false;

                        //Debug.Log("LeverScript2Point0 Not found");
                    }                   
                }
                else
                {
                    Collider.IsPressed = false;
                }
            }
            else
            {
            

                EToPressLever.SetActive(false);
            }
        }
    }
    // In this script I will Check if the player is looking at an object with the Lever/ObjectInteractable Tag. Then this script will set the LeveScript's flip lever function.
    // If I make a general interact Function then this script should be able to call any game object with the interact functiono in it. That way it will also work for light switches ect.
    // Seems it will only work for levers I'm afraid. Or objects with a lever script attached to them. Just a rebranding will work lol?
}
