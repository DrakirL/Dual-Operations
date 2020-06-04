using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using FMODUnity;
using Mirror;
using System.Diagnostics;
using System.IO;
using System.Threading;

public class DualOperationsAudioPlayer : NetworkBehaviour
{
    // Singleton setup
    private static DualOperationsAudioPlayer instance;
    public static DualOperationsAudioPlayer Instance { get { return instance; } }

    void Awake()
    {
        instance = this;

        UnityEngine.Debug.Log("DOAP hasAuthority = " + hasAuthority);
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
    [SerializeField] private string footstepPath;
    [SerializeField] private string[] doorPaths;

    public void Detected()
    {
        UnityEngine.Debug.Log("Nådde ljudets Detected: is server = " + isServer);
        PlaySound(detectedPath, GetPlayer.Instance.getPlayer().transform.position, false);
    }

    public void Hack(int state)
    {
        UnityEngine.Debug.Log(state + " Hackety hack hack " + isServer);
        PlaySound(hackingPaths[state], GetPlayer.Instance.getPlayer().transform.position, false);
    }

    public void Step(GameObject source)
    {
        UnityEngine.Debug.Log("Step! " + isServer);
        PlaySound(footstepPath, source.transform.position, false);
    }

    public void Door(bool open, GameObject source)
    {
        UnityEngine.Debug.Log(open + " Access? " + isServer);
        PlaySound(doorPaths[open ? 1 : 0], source.transform.position, false);
    }

    void PlaySound(string path, Vector3 pos, bool agentOnly)
    {
        if (hasAuthority)
            CmdPlaySound(path, pos, agentOnly);
        
        else
            GetPlayer.Instance.CmdPlaySound(path, pos, agentOnly);
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

    [TargetRpc]
    void TargetRpcPlaySoundAgent(string path, Vector3 pos)
    {
        UnityEngine.Debug.Log("Nådde agent RPC");
        FMODUnity.RuntimeManager.PlayOneShot(path, pos);
    }

    [TargetRpc]
    void TargetRpcPlaySoundHacker(string path, Vector3 pos)
    {
        UnityEngine.Debug.Log("Nådde hacker RPC");
        FMODUnity.RuntimeManager.PlayOneShot(path, pos);
    }
}