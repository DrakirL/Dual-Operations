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
    [ClientRpc]
    public void RpcUpdateRadio(float n, int index)
    {
        //Teleport this to the radio
        transform.position = CameraManager.Instance.radioStruct[index].radio.transform.position;
               
        //Start radio music
        emitter.SetParameter("RadioState", n, false);
    }

    //[Header("SFX Settings")]
    // MANAGING OTHER SOUNDS
    [SerializeField]
    private string playerDetectedPath;

    public void Detected()
    {
        UnityEngine.Debug.Log("Nådde ljudets Detected: is server = " + isServer);
        
        if (isServer)
            RpcPlaySound(playerDetectedPath, GetPlayer.Instance.getPlayer());
    }

    [ClientRpc]
    void RpcPlaySound(string path, GameObject source)
    {
            UnityEngine.Debug.Log("Borde spela ett ljud");
            FMODUnity.RuntimeManager.PlayOneShot(path, source.transform.position);
    }
}
