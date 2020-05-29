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

    //public AlertMeter alert;

    void Start()
    {
        emitter = GetComponent<StudioEventEmitter>();
    }

    void Update()
    {
            emitter.SetParameter("LVL", Mathf.Clamp(AlertMeter._instance.alertValue, 0.0f, intenseStartValue) / intenseStartValue, false);
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
    [SerializeField] private string detectedPath;
    [SerializeField] private string[] hackingPaths;

    public void Detected()
    {
        UnityEngine.Debug.Log("Nådde ljudets Detected: is server = " + isServer);
        CmdPlaySound(detectedPath, GetPlayer.Instance.getPlayer().transform.position, false);
    }

    public void Hack(int state)
    {
        UnityEngine.Debug.Log(state + " Hackety hack hack " + isServer);
        CmdPlaySound(hackingPaths[state], GetPlayer.Instance.getPlayer().transform.position, false);
    }

    [Command]
    void CmdPlaySound(string path, Vector3 pos, bool agentOnly)
    {
        UnityEngine.Debug.Log("Nådde CMD");
        RpcPlaySound(path, pos, agentOnly);
    }

    [ClientRpc]
    void RpcPlaySound(string path, Vector3 pos, bool agentOnly)
    {
        UnityEngine.Debug.Log("Nådde RPC");
        FMODUnity.RuntimeManager.PlayOneShot(path, pos);
    }
}