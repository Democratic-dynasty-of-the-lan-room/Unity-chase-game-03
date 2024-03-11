using UnityEngine;

public class InSpawnPoint : MonoBehaviour
{
    public DataPersistenceManager DataPersistenceManager;

    // Saves Restart Data
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Triggered spawn point");

            DataPersistenceManager.instance.RestartSaveGame();
        }
    }
}
