using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Level Music")]

    [field: SerializeField] public EventReference Decayed { get; private set; }

    [field: Header("Level Ambient")]

    [field: SerializeField] public EventReference EerieAmbient { get; private set; }

    [field: Header("Level SFX")]

    [field: SerializeField] public EventReference LeverSound { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference PlayerFootsteps { get; private set; }

    [field: SerializeField] public EventReference PlayerRunning { get; private set; }

    [field: SerializeField] public EventReference DashSound { get; private set; }

    [field: Header("PickedUpSound SFX")]
    [field: SerializeField] public EventReference PickedUpSound { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}
