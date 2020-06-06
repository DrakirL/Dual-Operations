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
    NetworkConnection AgentConn { get { return GetPlayer.Instance.getPlayer().GetComponent<NetworkIdentity>().connectionToClient; } }

    public enum Listerners
    { Agent, Hacker, Both };

    [SerializeField] private string detectedPath;
    [SerializeField] private string[] hackingPaths;
    [SerializeField] private string tasorPath;
    [SerializeField] private string[] footstepPaths;
    [SerializeField] private string[] doorPaths;

    public void Detected()
    {
        PlaySound(detectedPath, GetPlayer.Instance.getPlayer().transform.position, Listerners.Both);
    }

    public void Hack(int state)
    {
        PlaySound(hackingPaths[state], GetPlayer.Instance.getPlayer().transform.position, Listerners.Hacker);
    }

    public void Tasor()
    {
        UnityEngine.Debug.Log("Taser! isServer = " + isServer);
        PlaySound(tasorPath, GetPlayer.Instance.getPlayer().transform.position, Listerners.Agent);
    }

    public void Step(GameObject source)
    {

        int x = 0;

        if (source.transform.root == GetPlayer.Instance.getPlayer())
            x = 1;

        UnityEngine.Debug.Log(x + "step " + isServer);
        PlaySound(footstepPaths[x], source.transform.position, Listerners.Agent);
    }

    public void Door(bool open, GameObject source)
    {
        UnityEngine.Debug.Log("open = " + open + " isServer = " + isServer);

        int x = 0;

        if (open = true)
            x = 1;

        PlaySound(doorPaths[x], source.transform.position, Listerners.Agent);
    }

    void PlaySound(string path, Vector3 pos, Listerners listeners)
    {
        if (hasAuthority)
            CmdPlaySound(path, pos, listeners);
        
        else
            GetPlayer.Instance.CmdPlaySound(path, pos, listeners);
    }

    [Command]
    void CmdPlaySound(string path, Vector3 pos, Listerners listeners)
    {
        UnityEngine.Debug.Log("Nådde CMD");

        switch (listeners)
        {
            case Listerners.Agent:
                TargetRpcPlaySoundAgent(AgentConn, path, pos);
                break;
            case Listerners.Hacker:
                TargetRpcPlaySoundHacker(path, pos);
                break;
            case Listerners.Both:
                TargetRpcPlaySoundAgent(AgentConn, path, pos);
                TargetRpcPlaySoundHacker(path, pos);
                break;
            default:
                UnityEngine.Debug.LogError("Somehow tried to use a null enum I guess?");
                break;
        }
    }

    [TargetRpc]
    void TargetRpcPlaySoundAgent(NetworkConnection target, string path, Vector3 pos)
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