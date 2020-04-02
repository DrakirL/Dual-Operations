using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseAimC : MonoBehaviour
{
	[Range(0,20)] public float mouseSensitivity = 4;
	public bool inverted = false;
	public Vector2 viewLimit = new Vector2(-25,25);
	
	Vector2 rot = Vector2.zero;
	
    void Start()
    {
		Cursor.lockState = CursorLockMode.Locked;
    }
	
	void MouseLook()
	{
		
		
		int inv = Convert.ToInt32(inverted == true ? 1:-1);
		
		rot.x += Input.GetAxis("Mouse X");
		rot.y += Input.GetAxis("Mouse Y");
		
		rot.y = Mathf.Clamp(rot.y,viewLimit.x,viewLimit.y);
		transform.eulerAngles = new Vector2(0,rot.x);
		
		float y_axis = (inv) * rot.y * mouseSensitivity;
		float x_axis = rot.x * mouseSensitivity;
		transform.localRotation = Quaternion.Euler(y_axis,x_axis,0);
	}
	


    // Update is called once per frame
    void Update()
    {
		MouseLook();
    }
}
