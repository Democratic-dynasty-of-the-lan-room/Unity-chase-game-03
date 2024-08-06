using UnityEngine;

public class PickUpBattery : MonoBehaviour
{
    public float BatteryUpAmount;

    public bool CanRun;


    private void OnEnable()
    {
        Debug.Log("OnEnable");

        if (CentralBattery.Instance.CurrentCharge < CentralBattery.Instance.MaxBatteryCharge)
        {
            if (CentralBattery.Instance.CurrentCharge < CentralBattery.Instance.MaxBatteryCharge && CentralBattery.Instance.CurrentCharge + BatteryUpAmount <= CentralBattery.Instance.MaxBatteryCharge)
            {
                CentralBattery.Instance.CurrentCharge += BatteryUpAmount;

                Debug.Log("Add Health");

                Destroy(transform.parent.gameObject);
                Debug.Log("Destroy battery");
            }
            else
            {
                CentralBattery.Instance.CurrentCharge = CentralBattery.Instance.MaxBatteryCharge;

                Destroy(transform.parent.gameObject);
            }
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}