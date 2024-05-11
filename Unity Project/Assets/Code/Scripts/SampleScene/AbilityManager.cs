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
      
    }

    public void SaveData(ref GameData data)
    {
      
    }

    public void RestartLoadData(CheckPointData CheckPointLoadData)
    {
        
    }

    public void RestartSaveData(ref CheckPointData CheckPointSaveData)
    {
       
    }
}
