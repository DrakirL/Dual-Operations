using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ChangeAlertnessTexture : NetworkBehaviour
{
	Outline outline;
	private void Awake()
	{
		outline = GetComponent<Outline>();
        outline.enabled = false;
	}
	public enum SetTexture
	{
		Unsuspected,
		Alerted
	};
	
	public SetTexture setTexture;

    public Material unsuspected;
	public Material alerted;
	
	  Material last = null;
	  Material tex = null;

    [SyncVar] int lastInt = 0;
    [SyncVar] int texInt = 0;

    public Transform target;
	
    [ClientRpc]
	void RpcChangeTexture()
	{
        if (texInt == 1)
        {
            tex = unsuspected;
        }
        else if (texInt == 2)
        {
            tex = alerted;
        }
        else
        {
            Debug.LogError("this should not happen!" + texInt);
        }
        Transform child;
        int n = target.transform.childCount;

        for (int i = 0; i < n; i++)
        {
            child = target.transform.GetChild(i);
            child.GetComponentInChildren<Renderer>().material = tex;
        }
        if (!isServer)
        {
            lastInt = texInt;
        }
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
                        texInt = 1;                       
                    break;

                case SetTexture.Alerted:
                        tex = alerted;
                        outline.enabled = true;
                        texInt = 2;
                    break;

                default:
                        tex = null;
                        texInt = 0;
                    
                    break;
            }
            if (texInt != lastInt)
            {
                RpcChangeTexture();
            }
        }
    }
}
