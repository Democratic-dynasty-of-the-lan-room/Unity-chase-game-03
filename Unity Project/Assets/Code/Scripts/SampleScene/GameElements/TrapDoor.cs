using UnityEngine;
using Code.Scripts.SampleScene.Player;

public class TrapDoor : MonoBehaviour
{
    [SerializeField] LeverScript2Point0 Lever;

    private Animation anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Lever.IsPressed)
        {
            if (anim.isPlaying)
            {
                Lever.LeverLocked = true;
            }
            else
            {
                Lever.LeverLocked = false;

                RunAnimation();
            }

            Lever.IsPressed = false;
        }
    }

    private void RunAnimation()
    {
        if (Lever.LeverIsUp)
        {
            Debug.Log("Open");

            anim.Play("TrapDoorOpen");
        }
        else
        {
            Debug.Log("Close");

            anim.Play("TrapDoorClose");
        }
    }
}
