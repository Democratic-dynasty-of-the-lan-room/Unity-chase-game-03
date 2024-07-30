using UnityEngine;
using Code.Scripts.SampleScene.Player;

public class TrapDoor : MonoBehaviour
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && anim.isPlaying)
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
        if (Lever.IsPressed)
        {
            if (anim.isPlaying)
            {
                Lever.LeverLocked = true;
            }
            else
            {
                Lever.LeverLocked = false;

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

                    RunAnimation();
                }
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
