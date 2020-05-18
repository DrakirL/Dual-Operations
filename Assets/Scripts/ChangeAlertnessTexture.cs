using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAlertnessTexture : MonoBehaviour
{
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
			break;
			
			case SetTexture.Alerted:
			tex = alerted;
			break;
			
			default:
			tex = null;
			break;
		}
		
		if(tex != last)
		  changeTexture(tex);
    }
}
