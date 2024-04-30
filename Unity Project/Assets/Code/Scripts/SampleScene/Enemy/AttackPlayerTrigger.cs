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

    private bool CannotAttack;

    private void Start()
    {
        CanAttack = false;

        InRange = false;

        CannotAttack = false;
    }

    private void FixedUpdate()
    {    
        if (CanAttack && InRange && playerHealth.Health > 0 && CannotAttack == false)
        {
            Debug.Log("player Health here: " + playerHealth.Health);

            if (playerHealth.Health > 0)
            {

                Debug.Log("player Health In: " + playerHealth.Health);

                coroutine = AttackWait(AttackWaitTime);
                StartCoroutine(coroutine);

                //Deal Damage
                playerHealth.Health -= AttackDamage;

                CanAttack = true;
            }
            else if (playerHealth.Health < 0)
            {
                CannotAttack = true;

                playerHealth.Health = 100;

                Debug.Log("Cannot attack");
            }                     
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && CannotAttack == false)
        {
            InRange = true;

            //Send a ray to see if the player is behind a wall or not.

            // Start the Attack Coroutine;

            //Deal First amount of damage
            playerHealth.Health -= AttackDamage;

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
        if (playerHealth.Health > 0 && CannotAttack == false)
        {
            yield return new WaitForSeconds(waitTime);
            print("Coroutine ended: " + Time.time + " seconds");

            Debug.Log("Injured Health:" + playerHealth.Health);

            CanAttack = true;
        }  
    }
}
