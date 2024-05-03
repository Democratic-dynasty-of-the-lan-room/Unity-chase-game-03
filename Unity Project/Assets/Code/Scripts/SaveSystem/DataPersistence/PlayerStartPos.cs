using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStartPos : MonoBehaviour, IDataPersistence
{
    public int PositionToSpawn;

    public int SceneToLoad;

    public bool CanSetSpawn;  

    public void LoadData(GameData data)
    {
        PositionToSpawn = data.PosToSpawn;

        CanSetSpawn = data.CanSpawn;

        SceneToLoad = data.SceneNumber;
    }
    public void SaveData(ref GameData data)
    {
        data.PosToSpawn = PositionToSpawn;

        data.CanSpawn = CanSetSpawn;

        data.SceneNumber = SceneToLoad;
    }

    // Making sure that We have the right scene to load when you click Stop in unity.
    private void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            // What about the menu?
            SceneToLoad = SceneManager.GetActiveScene().buildIndex;

            Debug.Log("OnApplicationQuit in PlayerStartPos");
        }
    }

    public void RestartLoadData(CheckPointData CheckPointLoadData)
    {

    }

    public void RestartSaveData(ref CheckPointData CheckPointSaveData)
    {

    }
}
