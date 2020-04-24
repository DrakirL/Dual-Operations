using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DualOperationsAudioPlayer : MonoBehaviour
{
    // Singleton setup
    public static DualOperationsAudioPlayer audioPlayer;

    void Awake()
    {
        audioPlayer = this;
    }

    //[Header("Music Settings")]

    // MANAGING MUSIC
    private FMOD.Studio.EventInstance Music;
    public string musicPath;

    public AlertMeter alert;

    /* Track system
    [System.Serializable]
    public class Track
    {
        [SerializeField] public string parameterName;
        [SerializeField] public float fadeFloor;
        [SerializeField] public float fadeCeiling;  
    }

    [SerializeField] Track[] tracks;
    */

    void Start()
    {
        Music = FMODUnity.RuntimeManager.CreateInstance(musicPath);
        Music.start();
        Music.release();

        if (alert != null)
        {
            Music.setParameterByName("RadioState", 0.0f);
        }
    }

    void Update()
    {
        if(alert != null)
        {
            Music.setParameterByName("LVL", alert.alertValue / 100.0f);
        }

        //if tracks are used
        //Music.setParameterByName(tracks[i].parameterName, (Mathf.SmoothStep(0.0f, 1.0f, (tracks[i].fadeFloor - alert.alertValue) / (tracks[i].fadeFloor - tracks[i].fadeCeiling))));
    }

    void OnDestroy()
    {
        Music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }


    //[Header("SFX Settings")]
    // MANAGING OTHER SOUNDS
    public void HackCameraEvent()
    { }

    void PlaySound(string path, GameObject source)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path, source.transform.position);
    }
}
