using UnityEngine;
using FMOD.Studio;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

public class Music : MonoBehaviour
{
    private EventInstance EerieAmbientSound;
    //private EventInstance Decayed;

    private bool CanStartDecayed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EerieAmbientSound = AudioManager.instance.CreateEventInstance(FMODEvents.instance.EerieAmbient);
        //Decayed = AudioManager.instance.CreateEventInstance(FMODEvents.instance.EerieAmbient);

        Debug.Log("StartEerie Ambient sound");

        CanStartDecayed = true;
    }

    private void Update()
    {
        PLAYBACK_STATE playbackState;
        EerieAmbientSound.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(EerieAmbientSound, GetComponent<Transform>(), GetComponent<Rigidbody>());
            EerieAmbientSound.start();
        }

        /*
        if (CanStartDecayed)
        {
            DecayedMenuMusic();

            CanStartDecayed = false;

            EerieAmbientSound.stop(STOP_MODE.ALLOWFADEOUT);
        }
        */

    }

    private void OnDisable()
    {
        EerieAmbientSound.stop(STOP_MODE.ALLOWFADEOUT);
        //Decayed.stop(STOP_MODE.ALLOWFADEOUT);
    }

    private void DecayedMenuMusic()
    {
        /*
        //AudioManager.instance.PlayOneShot(FMODEvents.instance.Decayed, Camera.main.transform.position);
        PLAYBACK_STATE playbackState;
        Decayed.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(Decayed, GetComponent<Transform>(), GetComponent<Rigidbody>());
            Decayed.start();

            EerieAmbientSound.stop(STOP_MODE.ALLOWFADEOUT);

            Debug.Log("Start Decayed");
        }
        */
    }
}
