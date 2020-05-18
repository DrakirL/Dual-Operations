using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ChangeAlertnessTexture : NetworkBehaviour
{
	Outline outline;
	private void Awake()
	{
		outline = target.GetComponent<Outline>();
        outline.enabled = false;
    }
	public enum SetTexture
	{
		Unsuspected,
        //Suspicious,
		Alerted
	};
	
	public SetTexture setTexture;

    public Material unsuspected;
	public Material alerted;
	
	[SyncVar]  Material last = null;
	[SyncVar]  Material tex = null;

    public Transform target;
	
    [ClientRpc]
	void RpcChangeTexture(ref tex)
	{
        Transform child;
        int n = target.transform.childCount;

        for (int i = 0; i < n; i++)
        {
            child = target.transform.GetChild(i);
            child.GetComponentInChildren<Renderer>().material = tex;
        }

        if (!isServer)
            last = tex;

		return;
	}

    void Update()
    {		
        if (isServer)
        {
            switch (setTexture)
            {
                case SetTexture.Unsuspected:
                tex = unsuspected;
                outline.enabled = false;                   
                break;

                case SetTexture.Alerted:
                tex = alerted;
                outline.enabled = true;
                break;

                default:
                tex = null;
                outline.enabled = false; 
                break;
            }

            if (tex != last)
            {
                RpcChangeTexture(ref tex);
            }
        }
    }
}
