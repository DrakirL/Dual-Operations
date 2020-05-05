using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAlertnessTexture : MonoBehaviour
{
	Outline outline;
	private void Awake()
	{
		outline = GetComponent<Outline>();
	}
	public enum SetTexture
	{
		Unsuspected,
		Suspicious,
		Alerted
	};
	
	public SetTexture setTexture;

    public Material unsuspected;
	public Material alerted;
	
	Material last = null;
	Material tex = null;
	
    public Transform target;
	
	void changeTexture(Material texture)
	{
		Transform child;
		int n = target.transform.childCount;
		
		for(int i = 0; i < n; i++)
		{
			child = target.transform.GetChild(i);
			child.GetComponentInChildren<Renderer>().material = texture;
			//Debug.Log(i);
		}
		
		last = texture;
		
		return;
	}

    void Update()
    {
		switch(setTexture)
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
			break;
		}
		
		if(tex != last)
		  changeTexture(tex);
    }
}
