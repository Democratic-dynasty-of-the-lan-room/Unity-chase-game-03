using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;

public class SwingingObstacle : MonoBehaviour
{
    private Animation anim;

    [Tooltip("Amount of time SwingingObstacle waits before starting")]
    public float StartTimeOffset;

    [Tooltip("amount of time waits at front")]
    public float WaitTimeUp;
    [Tooltip("amount of time waits at back")]
    public float WaitTimeDown;

    [Tooltip("Not Implemented yet. Switch To Velocity based Damage. From the player and obstacle")]// Ideally get velocityi of the singing object as well as the players velocity to deal damage
    public bool VelocityBasedDamage;

    public int DamageAmount;

    Rigidbody Collisionrb;
    public float KnockBackForce;
    public float UpwardsModifier;
    public float KnockBackRadius;



    private IEnumerator WaitBeforeUp;
    private IEnumerator WaitBeforeDown;
    private IEnumerator TimeOffset;

    private bool IsForward;
    private bool StopAnim;
    private bool CanPlay;

    private void OnCollisionEnter(Collision collision)
    {
        if (anim.isPlaying)
        {
            if (collision.collider.CompareTag("Player"))
            {
             
                if (VelocityBasedDamage)
                {
                    Debug.Log("Damage Velocity");
                }
                else
                {
                    PlayerHealth.Instance.Health -= DamageAmount;

                    // apply knockback force
                    Collisionrb = collision.rigidbody.GetComponent<Rigidbody>();
                    var CollisionPlayer = collision.gameObject.GetComponent<PlayerMovment>();
                  
                    if (CollisionPlayer.grounded)
                    {
                        Collisionrb.AddExplosionForce(KnockBackForce * 2.5f, this.transform.position, KnockBackRadius * 2, UpwardsModifier * 1.5f, ForceMode.Impulse);
                    }
                    else
                    {
                        Collisionrb.AddExplosionForce(KnockBackForce, this.transform.position, KnockBackRadius, UpwardsModifier, ForceMode.Impulse);
                    }

                    Debug.Log("Damage" + PlayerHealth.Instance.Health);
                }                   

                // TODO - add camera shake
            }
        }
    }

    private void OnEnable()
    {
        StopAnim = false;

        CanPlay = false;

        IsForward = true;

        TimeOffset = WaitTimeOffset(StartTimeOffset);
        StartCoroutine(TimeOffset);
    }

    void Start()
    {
        anim = GetComponentInParent<Animation>();
    }

    void Update()
    {
        if (!anim.isPlaying && !StopAnim && CanPlay)
        {
            if (IsForward)
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

        IsForward = false;

        CanPlay = true;

        anim.Play("Swingforward");
    }

    private IEnumerator WaitAfterDown(float WaitTimeDown)
    {
        yield return new WaitForSeconds(WaitTimeDown);

        IsForward = true;

        CanPlay = true;

        anim.Play("SwingBack");
    }

    private IEnumerator WaitTimeOffset(float TimeOffset)
    {
        yield return new WaitForSeconds(TimeOffset);

        CanPlay = true;
    }
}