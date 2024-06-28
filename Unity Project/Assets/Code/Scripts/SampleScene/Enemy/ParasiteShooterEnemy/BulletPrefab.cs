using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPrefab : MonoBehaviour
{
    Rigidbody rb;

    private IEnumerator coroutine;

    public float Power;

    public float WallBounceTimes;

    public float TimesCollided;

    public int BulletDamageAmount;

    public float DespawnTime; // TODO - Make a coroutine to destroy this game object.

    void Start()
    {
        TimesCollided = 0;

        rb = GetComponent<Rigidbody>();

        rb.AddForce(this.transform.forward * Power, ForceMode.Impulse);

        coroutine = Despawn(DespawnTime);
        StartCoroutine(coroutine);
    }

    // if the bullet hits a wall destroy this object when it reaches max Bounce times.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().Health -= BulletDamageAmount;

            Debug.Log("Bullet Hit Health = " + collision.gameObject.GetComponent<PlayerHealth>().Health);
        }

        TimesCollided += 1;

        if (TimesCollided >= WallBounceTimes)
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator Despawn(float DespawnTime)
    {
        yield return new WaitForSeconds(DespawnTime);

        Destroy(this.gameObject);
    }
}
