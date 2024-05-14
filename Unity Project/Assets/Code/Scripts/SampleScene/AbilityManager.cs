using UnityEngine;

public class AbilityManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] Dash dash;

    public bool SaveDash;

    public void Dash()
    {
        if (dash.enabled == true)
        {
            SaveDash = true;

            //TODO - also save the item button somehow.
        }
    }









    public void LoadData(GameData data)
    {
        //SaveDash = data.ADash;
    }

    public void SaveData(ref GameData data)
    {
        //data.ADash = SaveDash;
    }

    public void RestartLoadData(CheckPointData CheckPointLoadData)
    {
        
    }

    public void RestartSaveData(ref CheckPointData CheckPointSaveData)
    {
       
    }
}
