using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;

public class Crusher : MonoBehaviour
{
    private Animation anim;

    [Tooltip("Amount of time crusher waits before starting")]
    public float StartTimeOffset;

    [Tooltip("amount of time crusher waits at top")]
    public float WaitTimeUp;
    [Tooltip("amount of time crusher waits at bottom")]
    public float WaitTimeDown;

    private IEnumerator WaitBeforeUp;
    private IEnumerator WaitBeforeDown;
    private IEnumerator TimeOffset;

    private bool IsDown;
    private bool StopAnim;
    private bool CanPlay;

    private void OnCollisionEnter(Collision collision)
    {
        if (anim.IsPlaying("CrusherDown"))
        {
            if (collision.collider.CompareTag("Player"))
            {
                Debug.Log("Stop anim Before");
                PlayerHealth.Instance.Health = 0;

                anim.Stop();
                StopAnim = true;
                Debug.Log("StopAnimation");

                // TODO - add camera shake
            }
        } 
    }

    private void OnEnable()
    {
        StopAnim = false;

        CanPlay = false;

        IsDown = true;

        TimeOffset = WaitTimeOffset(StartTimeOffset);
        StartCoroutine(TimeOffset);
    }

    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
    }

    void Update()
    {
        if (!anim.isPlaying && !StopAnim && CanPlay)
        {
            if (IsDown)
            {
                WaitBeforeUp = WaitAfterUp(WaitTimeUp);
                StartCoroutine(WaitBeforeUp);

                CanPlay = false;
            }
            else
            {
                WaitBeforeDown = WaitAfterDown(WaitTimeDown);
                StartCoroutine(WaitBeforeDown);

                CanPlay = false;
            }
        }
    }

    private IEnumerator WaitAfterUp(float WaitTimeUp)
    {
        yield return new WaitForSeconds(WaitTimeUp);

        IsDown = false;

        CanPlay = true;

        anim.Play("CrusherUp");
    }

    private IEnumerator WaitAfterDown(float WaitTimeDown)
    {
        yield return new WaitForSeconds(WaitTimeDown);

        IsDown = true;

        CanPlay = true;

        anim.Play("CrusherDown");
    }

    private IEnumerator WaitTimeOffset(float TimeOffset)
    {
        yield return new WaitForSeconds(TimeOffset);

        CanPlay = true;
    }
}