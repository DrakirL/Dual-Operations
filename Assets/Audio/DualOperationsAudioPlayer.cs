using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualOperationsAudioPlayer : MonoBehaviour
{
    // MANAGING MUSIC
    private static FMOD.Studio.EventInstance Music;
    public string musicPath;

    private AlertMeter alert;

    [System.Serializable]
    public class Track
    {
        [SerializeField] public string parameterName;
        [SerializeField] public float fadeFloor;
        [SerializeField] public float fadeCeiling;
    }

    [SerializeField] Track[] tracks;

    void Start()
    {
        Music = FMODUnity.RuntimeManager.CreateInstance(musicPath);
        Music.start();
        Music.release();

        alert = (AlertMeter)FindObjectOfType(typeof(AlertMeter));
    }

    void Update()
    {
        if (alert != null)
        {
            for(int i = 0; i < tracks.Length; i++)
            {
                Music.setParameterByName(tracks[i].parameterName, (Mathf.SmoothStep(0.0f, 1.0f, (tracks[i].fadeFloor - alert.alertValue) / (tracks[i].fadeFloor - tracks[i].fadeCeiling))));
            }
        }
    }

    void OnDestroy()
    {
        Music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    // MANAGING OTHER SOUNDS
    void PlaySound(string path)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path, transform.position);
    }
}
