using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MouseAimC : MonoBehaviour
{
	public Transform cameraView;
	
	[Range(0,20)] public float mouseSensitivity = 4;
	public bool inverted = false;
	
	public float moveSpeed = 2;
		
	private Vector2 viewLimit = new Vector2(-90,90);
	private Vector2 XLimit = new Vector2(-90,90);
	private Vector2 rot = Vector2.zero;
	
	Vector3 movementDirection = Vector3.zero;
	Vector3 actorVelocity = Vector3.zero;
	
	public float gravity = 10;
	public float acceleration = 10;
	public float deacceleration = 3;
	public float friction = 4;
	
	public LayerMask colMask;
	
	//CharacterController controller; //Plan B
	
    void Start()
    {
		if(cameraView == null)
		{
			Camera mainCamera = Camera.main;
			if(mainCamera != null)
			{
				cameraView = mainCamera.gameObject.transform;
			}
		}
		
		Cursor.lockState = CursorLockMode.Locked;
		cameraView.transform.position = this.transform.position;
		
		//controller = GetComponent<CharacterController>();
    }
	
	void MoveWalk(float forward,float right)
	{		   
		//Get a pure direction vector

		//MoveFriction(friction);

		Vector3 movdir = new Vector3(right,0,forward);
		movdir.Normalize();
		
		float speed = Vector3.Magnitude(movdir);
		speed *= moveSpeed;

		//MoveAccelerate(movdir, speed, acceleration);
	
		movementDirection.x += moveSpeed * movdir.x;
		movementDirection.z += moveSpeed * movdir.z;
		
		//movementDirection.y += -gravity;
		
		//Add jump action here
	}
	
	/*void MoveAccelerate(Vector3 dir, float speed, float accel)
	{
		float addSpeed,accelSpeed,currentSpeed;

		
		accelSpeed = accel * speed * Time.deltaTime;

		Debug.Log("accelSpeed: "+accelSpeed);
		
		movementDirection.x += accelSpeed * dir.x;
		movementDirection.z += accelSpeed * dir.z;
		
	}*/
	
	/*void MoveFriction(float frict)
	{
		Vector3 vec = actorVelocity;
		float speed, newspeed, control;
		float drop;
		
		vec.y = 0; //?
		
		float s = Vector3.Magnitude(vec);
		
		speed = s;//vec.magnitude;

		drop = 0;
		
		control = speed < deacceleration ? deacceleration : speed;
		drop += control * frict * Time.deltaTime;
		
		newspeed = speed - drop;
		if(newspeed < 0)
		{
			newspeed = 0;
		}
		else
		{	
			newspeed /= speed;
		}
		
		//Debug.Log(newspeed);
	
		movementDirection.x *= newspeed;
		movementDirection.z *= newspeed;
		
	}*/
	
	void Move(Vector3 vec)
	{		
		transform.Translate(vec.x,0,0,cameraView.transform);
		transform.Translate(0,0,vec.z,Space.Self);
		transform.Translate(0,vec.y,0,Space.Self);
	}
	
	void MouseLook()
	{
		int inv = Convert.ToInt32(inverted == true ? 1:-1);
		
		rot.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
		rot.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

		//Clamp y-axis view
		rot.y = Mathf.Clamp(rot.y,viewLimit.x,viewLimit.y);
		
		cameraView.transform.eulerAngles = new Vector2(0,rot.x);
		
		float y_axis = (inv) * rot.y;
		float x_axis = rot.x;

		cameraView.transform.localRotation = Quaternion.Euler(y_axis,x_axis,0);
		transform.localRotation = Quaternion.Euler(0,x_axis,0);
	}

	void Collision(float forward,float right)
	{
		bool isGrounded = false;
		float dist = GetComponent<BoxCollider>().bounds.extents.y;
		
		Ray downRay =  new Ray (transform.position, -transform.up);
		RaycastHit hitInfo;
		
		Vector3 col = new Vector3(right,0,forward);
		
		//RaycastHit2D hitY = Physics2D.Raycast(rayVector, Vector2.up * directionY, rayLength, entityMask);
		
		//            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
		
		if(Physics.Raycast(transform.position,Vector3.down, out hitInfo, dist+2f, colMask, QueryTriggerInteraction.Ignore))
		{
			//int er = (int)dist - (int)hitInfo.distance;
			
			//if(hitInfo.distance <)
		//	{
			movementDirection.y = 0;
			transform.position = hitInfo.point;//+new Vector3(0f,2,0f);
			Debug.DrawLine(downRay.origin,hitInfo.point, Color.red);
		//	}
		}
		else
		{
			Debug.DrawLine(downRay.origin,downRay.direction*100,Color.green);
		}
	}	
	
    void Update()
    {
		movementDirection = Vector3.zero;
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));

		MouseLook();
		MoveWalk(input.y,input.x);

		//Collision(input.y,input.x);

		//up vector manipulation (debug-only)
		if(Input.GetKey(KeyCode.Space))
		{
			movementDirection.y = moveSpeed;
		}
		
		Move(movementDirection * Time.deltaTime);
		
		//Forward vector, only debbuging
		//------
		Ray front = new Ray (transform.position, transform.forward);
		Debug.DrawLine(front.origin,front.direction*60,Color.blue);
		//------
		
		cameraView.position = new Vector3(transform.position.x,transform.position.y,transform.position.z);
    }
}
