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
	
	public float cameraOffsetY = 9;
	
	public LayerMask colMask;
	
	private CharacterController controller; //Plan B
	private bool isGrounded = false;
	
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
		cameraView.transform.position = new Vector3(transform.position.x,transform.position.y+cameraOffsetY,transform.position.z);		
		
		
		controller = GetComponent<CharacterController>();
		
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
		
		movementDirection.y += -gravity;
		
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

		float dist = GetComponent<BoxCollider>().bounds.extents.y;
		
		Ray downRay =  new Ray (transform.position, -transform.up);
		RaycastHit hit;
		//RaycastHit hit2;
		
		//Vector3 col = new Vector3(right,0,forward);
		
		//Checking below player for surface
		if(Physics.Raycast(downRay, out hit, dist, colMask))
		{
			MeshCollider meshCollider = hit.collider as MeshCollider;
			if (meshCollider == null || meshCollider.sharedMesh == null)
             return;
		
			Mesh mesh = meshCollider.sharedMesh;
			Vector3[] v = mesh.vertices;
			
		 	Debug.DrawLine(downRay.origin,transform.up, Color.red);
			
			//Find the surface 
			for(int i = 0; i < v.Length;i++)
			{
				//Debug.Log("No." + i + " = " + v[i]);
				double n = (-1*Vector3.Dot(v[i] - movementDirection,hit.normal));
				//float n2 = Mathf.Clamp(n,0.01f,0.1f);
				//Debug.Log(n);
				
				if(n < -0.01)
				{
					movementDirection.y = 0;
					//transform.position = new Vector3(transform.position.x,movementDirection.y + 2f,transform.position.z);
					break;
					//isGrounded = true;
				}
			}
			
			return;

		}
		else
		{
			Debug.DrawLine(downRay.origin,downRay.direction*100,Color.green);
		}
	}	
	
		
	void Update()
	{
		MouseLook();
		
		//Forward vector, only debbuging
		//------
		Ray front = new Ray (transform.position, transform.forward);
		Debug.DrawLine(front.origin,front.direction*60,Color.blue);
		//------
	
		cameraView.position = new Vector3(transform.position.x,transform.position.y+cameraOffsetY,transform.position.z);
	}
	
	
    void FixedUpdate()
    {
		movementDirection = new Vector3(0f,0f,0f);
		
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
		
		MoveWalk(input.y,input.x);
		Collision(0,0);

		//up vector manipulation (debug-only)
		if(Input.GetKey(KeyCode.Space))
		{
			movementDirection.y = moveSpeed;
		}

		//final transform calculation
		Move(movementDirection * Time.fixedDeltaTime);	
    }
}
