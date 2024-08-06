using UnityEngine;
using TMPro;

public class BatteryUI : MonoBehaviour
{
    public TextMeshProUGUI field;

    void Update()
    {
        field.text = CentralBattery.Instance.CurrentCharge.ToString();
    }
}
