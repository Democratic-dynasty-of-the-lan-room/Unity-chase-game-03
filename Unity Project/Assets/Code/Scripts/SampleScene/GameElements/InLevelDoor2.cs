using UnityEngine;
using Code.Scripts.SampleScene.Player;

public class InLevelDoor2 : MonoBehaviour
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

        currentAnimation = null;
    }


    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("OnCollisionEnter");

            if (currentAnimation == null)
            {
                currentAnimation = GetCurrentAnimation();

                if (currentAnimation != null)
                {
                    pauseTime = anim[currentAnimation].time;
                }
                else
                {
                    Debug.Log("No current animation");
                }
            }         

            anim.Stop(currentAnimation);

            HitPlayer = true;

            Lever.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("On Trigger Exit");

            HitPlayer = false;

            Lever.enabled = true;
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
                if (currentAnimation != null)
                {                  
                    if (Lever.LeverIsUp)
                    {
                        Lever.LeverIsUp = false;

                        Lever.LeverLocked = false;

                        anim[currentAnimation].time = pauseTime;

                        anim.Play(currentAnimation);

                        currentAnimation = null;
                    }
                    else
                    {
                        Lever.LeverIsUp = true;

                        Lever.LeverLocked = false;

                        anim[currentAnimation].time = pauseTime;

                        anim.Play(currentAnimation);

                        currentAnimation = null;
                    }
                }
                else
                {
                    Lever.LeverLocked = false;

                    RunDoor();            
                }
            }

            Lever.IsPressed = false;
        }
        /*
        else if (Lever.IsPressed && HitPlayer)
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

            //HitPlayer = false;

            Lever.IsPressed = false;
        }
        */

        if (HitPlayer)
        {
            //Lever.enabled = false;
        }
        else
        {
            //Lever.enabled = true;
        }



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

    private void OldUpdateScript()
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
}
