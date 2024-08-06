using UnityEngine;

public class CentralBattery : MonoBehaviour, IDataPersistence
{
    public float CurrentCharge;
    public float MaxBatteryCharge;

    public static CentralBattery Instance { get; private set; }

    private void Awake()
    {
        // Make sure there is only one instance to keep the singleton pattern
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }   
    private void Update()
    {
        if (CurrentCharge < 0)
        {
            CurrentCharge = 0;
        }

        if (CurrentCharge > MaxBatteryCharge)
        {
            CurrentCharge = MaxBatteryCharge;
        }
    }
    public void LoadData(GameData data)
    {
        CurrentCharge = data.Battery;
    }

    public void SaveData(ref GameData data)
    {
        data.Battery = CurrentCharge;
    }

    public void RestartLoadData(CheckPointData CheckPointLoadData)
    {

    }

    public void RestartSaveData(ref CheckPointData CheckPointSaveData)
    {

    }
}
