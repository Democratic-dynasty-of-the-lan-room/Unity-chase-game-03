using UnityEngine;
using UnityEngine.UI;

public class SliderBa : MonoBehaviour
{
    public Slider BatterCharge;

    void Update()
    {
        BatterCharge.value = CentralBattery.Instance.CurrentCharge / 100;
    }
}
