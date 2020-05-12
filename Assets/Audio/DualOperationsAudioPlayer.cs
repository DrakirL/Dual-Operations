using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using FMODUnity;
using Mirror;
using System.Diagnostics;

public class DualOperationsAudioPlayer : NetworkBehaviour
{
    // Singleton setup
    private static DualOperationsAudioPlayer instance;
    public static DualOperationsAudioPlayer Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
    }

    // MANAGING MUSIC
    private StudioEventEmitter emitter;
    public float intenseStartValue;

    public AlertMeter alert;

    void Start()
    {
        emitter = GetComponent<StudioEventEmitter>();
    }

    void Update()
    {
        if(alert != null)
        {
            emitter.SetParameter("LVL", Mathf.Clamp(alert.alertValue, 0.0f, intenseStartValue) / intenseStartValue, false);
        }
    }

    //public event Action onUpdateRadio;
    public void UpdateRadio(float n, GameObject source)
    {
        //Teleport this to the radio
        transform.position = source.transform.position;

        //Start radio music
        emitter.SetParameter("RadioState", n, false);
    }

    //[Header("SFX Settings")]
    // MANAGING OTHER SOUNDS
    [SerializeField]
    private string playerDetectedPath;

    public void Detected()
    {
        UnityEngine.Debug.Log("Nådde ljudets Detected");
        PlaySound(playerDetectedPath, GetPlayer.Instance.getPlayer());
    }

    void PlaySound(string path, GameObject source)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path, source.transform.position);
    }
}
