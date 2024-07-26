using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Spike : MonoBehaviour
{
    [Header("Velocity Selected Damage amounts")]

    [Tooltip("if the players velocity magnitude is over these values apply damage and then more damage")]
    public float VelocityToDamage = 8.999998f;
    public float HigherVelocityToDamage = 16;

    [Tooltip("Different Velocity damage amounts to apply")]
    public int DamageAmount;
    public int HigherDamageAmount;



    [Header("Velocity based Damage")]
    public int DamageMultiplyAmount;

    [Tooltip("Switch to puerly velocity based Damage")]
    public bool IsVelocityBasedDamage;

    private float PlayerVelocity;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerVelocity = other.gameObject.GetComponentInParent<Rigidbody>().linearVelocity.magnitude;

            if (IsVelocityBasedDamage)
            {
                float roundedValue = Mathf.Round(PlayerVelocity);

                PlayerHealth.Instance.Health -= roundedValue.ConvertTo<int>() * DamageMultiplyAmount;

                Debug.Log("Velocity Damave: " + roundedValue * DamageMultiplyAmount);
            }
            else
            {
                Debug.Log("Touched Spike");            

                if (PlayerVelocity > VelocityToDamage && PlayerVelocity < HigherDamageAmount)
                {
                    Debug.Log("Spiked");

                    PlayerHealth.Instance.Health -= DamageAmount;

                }
                else if (PlayerVelocity < HigherVelocityToDamage)
                {
                    Debug.Log("Spiked Alot");

                    PlayerHealth.Instance.Health -= HigherDamageAmount;
                }
            }
        }
    }
}
