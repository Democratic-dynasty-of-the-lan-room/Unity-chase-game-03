using UnityEngine;

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

    public void RestartLoadData(CheckPointData CheckPointLoadData)
    {

    }

    public void RestartSaveData(ref CheckPointData CheckPointSaveData)
    {

    }
}
