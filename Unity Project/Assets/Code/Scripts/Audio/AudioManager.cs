using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        instance = this;
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);

        //FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D();

        //eventInstance.set3DAttributes(attributes);

        return eventInstance;
    }
}
