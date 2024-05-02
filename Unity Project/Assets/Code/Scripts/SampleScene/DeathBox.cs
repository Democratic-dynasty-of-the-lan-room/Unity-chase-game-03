using UnityEngine;

public class DeathBox : MonoBehaviour
{
    public PlayerHealth playerHealth;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerHealth.Health = 0;
            //Debug.Log("DeathBox");
        }
    }
}
