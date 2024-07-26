using UnityEngine;
using Code.Scripts.SampleScene.Player;

public class InLevelDoor : MonoBehaviour
{
    [SerializeField] LeverScript2Point0 Lever;

    private Animation anim;

    public bool HitPlayer;

    public float pauseTime;

    private string currentAnimation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();

        HitPlayer = false;
    }


    private void OnCollisionEnter(Collision collider)
    {
        if (collider.collider.CompareTag("Player"))
        {
            if (currentAnimation == null)
            {
                currentAnimation = GetCurrentAnimation();

                pauseTime = anim[currentAnimation].time;
            }          

            anim.Stop(currentAnimation);

            HitPlayer = true;

            Lever.LeverLocked = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("On Trigger Exit");

            //Lever.LeverLocked = false;

            //HitPlayer = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Lever.IsPressed && !HitPlayer)
        {
            if (anim.isPlaying)
            {
                Lever.LeverLocked = true;
            }
            else
            {
                Lever.LeverLocked = false;

                RunDoor();
            }

            Lever.IsPressed = false;
        }
        else if (Lever.IsPressed && HitPlayer)
        {
            if (anim.isPlaying)
            {
                Lever.LeverLocked = true;
            }
            else
            {
                Lever.LeverLocked = false;

                anim[currentAnimation].time = pauseTime;

                anim.Play(currentAnimation);

                /*
                if (Lever.LeverIsUp)
                {
                    Lever.LeverIsUp = false;
                }
                else
                {
                    Lever.LeverIsUp = true;
                }    
                */

                //Lever.LeverLocked = true;// Testing here

                HitPlayer = false;

                currentAnimation = null;
            }
        }

        Lever.IsPressed = false;
    }

    private void RunDoor()
    {
        if (Lever.LeverIsUp)
        {
            //Debug.Log("Open");

            anim.Play("DoorOpen");
        }
        else
        {
            //Debug.Log("Close");

            anim.Play("DoorClose");
        }
    }

    private string GetCurrentAnimation()
    {
        foreach (AnimationState state in anim)
        {
            if (anim.IsPlaying(state.name))
            {
                return state.name;
            }
        }
        return null;
    }
}
