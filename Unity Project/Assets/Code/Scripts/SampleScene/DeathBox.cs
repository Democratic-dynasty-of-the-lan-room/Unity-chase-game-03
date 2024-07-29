using UnityEngine;

public class DeathBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealth.Instance.Health = 0;
            //Debug.Log("DeathBox");
        }
    }
}
