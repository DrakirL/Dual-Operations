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
        if (instance == null)
        {
            instance = this;
        }
    }

    public GameObject getPlayer()
    {
        return this.gameObject;
    }
    public void openDoorServer(string name)
    {
        CmdOpendorOnServer(name);
    }

    [Command]
    private void CmdOpendorOnServer(string name)
    {
        //door.RpcPlayOpenAnimation();
        RpcPlayOpenAnimation(name);
        Debug.Log("2");
    }
    [ClientRpc]
    private void RpcPlayOpenAnimation(string name)
    {
        GameObject.Find(name).GetComponent<SlideDoor>().RpcPlayOpenAnimation();
        Debug.Log("2 " + name);
    }
        /*public void openDoorServer(SlideDoor door)
        {
            CmdOpendorOnServer(door);
        }

        [Command]
        private void CmdOpendorOnServer(SlideDoor door)
        {
            door.RpcPlayOpenAnimation();
            Debug.Log("2");
        }*/
    }
