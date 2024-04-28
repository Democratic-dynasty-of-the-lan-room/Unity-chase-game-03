using System.Collections;
using UnityEngine;

public class AttackPlayerTrigger : MonoBehaviour
{

    private IEnumerator coroutine;

    [SerializeField] PlayerHealth playerHealth;

    public int AttackDamage;

    public float AttackWaitTime;

    private bool CanAttack;

    private bool InRange;

    private void Start()
    {
        CanAttack = false;

        InRange = false;
    }

    private void FixedUpdate()
    {
        
        if (CanAttack && InRange)
        {
            coroutine = AttackWait(AttackWaitTime);
            StartCoroutine(coroutine);

            //Deal Damage
            playerHealth.Health -= AttackDamage;

            CanAttack = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InRange = true;

            //Send a ray to see if the player is behind a wall or not.

            // Start the Attack Coroutine;

            //Deal First amount of damage
            playerHealth.Health -= AttackDamage;

            Debug.Log("OnTriggerEnter");

            CanAttack = true;           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InRange = false;

            CanAttack = false;
        }
    }

    private IEnumerator AttackWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        print("Coroutine ended: " + Time.time + " seconds");

        Debug.Log("Injured Health:" + playerHealth.Health);

        CanAttack = true;
    }

}
