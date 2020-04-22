using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GetPlayer : NetworkBehaviour
{
    private static GetPlayer instance;
    public static GetPlayer Instance { get { return instance; } }
    // Use this for initialization
    void Start()
    {
        transform.position = spawnTransform.Instance.transform.position;
        transform.rotation = spawnTransform.Instance.transform.rotation;

        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        //this works for server but not for client
        Debug.LogWarning(isServer + " : " + AlertMeter._instance.alertValue);
        if(Input.GetKeyDown(KeyCode.G))
        {
            AlertMeter._instance.AddAlert(1);
        }
    }

    public GameObject getPlayer()
    {
        return this.gameObject;
    }
    
    public void addAlertServer(float value)
    {
        CmdAddAlertOnServer(value);
    }
    [Command]
    private void CmdAddAlertOnServer(float value)
    {
        AlertMeter._instance.alertValue = Mathf.Clamp(AlertMeter._instance.alertValue + value, 0, 100);
        AlertMeter._instance.timeStamp = Time.time;
        RpcAddAlertOnClient(AlertMeter._instance.alertValue, AlertMeter._instance.timeStamp);
    }
    [ClientRpc]
    private void RpcAddAlertOnClient(float newAlerValue, float newTimeStamp)
    {
        AlertMeter._instance.alertValue = newAlerValue;// Mathf.Clamp(AlertMeter._instance.alertValue + value, 0, 100);
        AlertMeter._instance.timeStamp = newTimeStamp;// Time.time;
    }

    

    //door open functions
    public void openDoorServer(string name)
    {
        CmdOpendorOnServer(name);
    }
    [Command]
    private void CmdOpendorOnServer(string name)
    {
        RpcPlayOpenAnimation(name);
    }
    [ClientRpc]
    private void RpcPlayOpenAnimation(string name)
    {
        GameObject.Find(name).GetComponent<SlideDoor>().RpcPlayOpenAnimation();
    }
}
