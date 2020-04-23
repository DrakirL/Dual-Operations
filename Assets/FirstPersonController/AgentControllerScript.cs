using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using Mirror;

public class AgentControllerScript : MonoBehaviour
{
	public Transform cameraView;
	public Transform objectModel;
	
	public enum MouseAimStyle
	{
		VerticalHorizontal,
		HorizontalOnly
	};
	
	public MouseAimStyle mouseAimStyle;
	
	[Range(0,20)] public float mouseSensitivity = 4;
	
	public bool mouseInverted = false;
	
	public float moveSpeed = 3;
	public float gravity = 10;
	
	public LayerMask colMask;

	private Vector2 rot = Vector2.zero;
	private Vector3 movementDirection = Vector3.zero;
	private Vector3 actorVelocity = Vector3.zero;

	private float posRecover = 10f;
	private float jumpSpeed = 10f;
	
	//public float acceleration = 10;
	//public float deacceleration = 3;
	//public float friction = 4;

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
    }
	
	void MoveWalk(float forward,float right)
	{		   
		//MoveFriction(friction);

		Vector3 movdir = new Vector3(right,0,forward);
		movdir.Normalize();
		
		//float speed = Vector3.Magnitude(movdir);
		//speed *= moveSpeed;

		//MoveAccelerate(movdir, speed, acceleration);
	
		movementDirection.x += moveSpeed * movdir.x;
		movementDirection.z += moveSpeed * movdir.z;
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

	void MouseLook()
	{
		int inv = Convert.ToInt32(mouseInverted == true ? 1:-1);
		Vector2 viewLimit = new Vector2(-90,90);
		
		rot.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
		
		if(mouseAimStyle == MouseAimStyle.HorizontalOnly)
		{
			rot.y = 0;
		}
		else 
		{
			rot.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
			rot.y = Mathf.Clamp(rot.y,viewLimit.x,viewLimit.y);
		}
		
		float y_axis = (inv) * rot.y;
		float x_axis = rot.x;
		
		cameraView.transform.localRotation = Quaternion.Euler(y_axis,0,0);
		objectModel.transform.localRotation = Quaternion.Euler(0,0,0);
		transform.localRotation = Quaternion.Euler(0,x_axis,0);
	}

	void CollisionDetection(ref Vector3 velocity, float forward,float right,bool ColCorrect)
	{		
		RaycastHit hit;
		
		//float dist = (GetComponent<SphereCollider>().radius)*0.5f;
		float distH = (GetComponent<CapsuleCollider>().radius);
		float distV = (GetComponent<CapsuleCollider>().height)*0.5f;
		
		Ray downRay =  new Ray (transform.position, Vector3.down);
		Ray upRay =  new Ray (transform.position, Vector3.up);
		Ray downForwardRay =  new Ray (transform.position, Vector3.down+(forward*transform.forward));
		Ray upForwardRay =  new Ray (transform.position, Vector3.up+(forward*transform.forward));

		Ray frontRay =  new Ray (transform.position, forward*transform.forward);
		Ray rightRay = new Ray (transform.position, right*transform.right);
		Ray uRightRay = new Ray (transform.position, transform.right);
		Ray uLeftRay = new Ray (transform.position, -transform.right);
		
		Ray forwardRightRay = new Ray(transform.position, (transform.forward)+(right*transform.right));
		Ray forwardRightRay2 = new Ray(transform.position, (-transform.forward)+(right*transform.right));
		
		// +=== HOW THE COLLISION WORKS ======+
		// Calculation is done by looking for vertices on the mesh, then looking for the normal,
		// then calculate the dot product of the vertices - player position and the normal of the mesh face.
		//
		// formula: n = (vertices - pos) · normal
		//
		// This returns a value to determine if the player is on the side where the object lies. 
		// If it is less than zero (0.01 in this case) then it is on the side of the object, 
		// otherwise it is not.
		// In some cases, it is checking for both since it has to accomodate for if the value is negative, 
		// depending upon where the players forward vector is facing. 
		
		// Currently, if the player ends up inside the mesh, a simple lerp function is performed
		// to put back the player into the right position. This is a hack solution, but at it will
		// suffice for this project. It also allows for simple slope collision.
		// +==================================+
		
		if(Physics.Raycast(downRay, out hit, distV + 0.05f, colMask,QueryTriggerInteraction.Ignore))
		{
			Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
			Vector3[] v = mesh.vertices;
			
			for(int i = 0; i < v.Length;i++)
			{
				float n = Vector3.Dot(v[i] - velocity, hit.normal);
				
				if(n > 0.00f) 
				{
					velocity.y = 0;
					break;
				}
			}
		}

		/*if(Physics.Raycast(upRay, out hit, dist + 0.05f, colMask,QueryTriggerInteraction.Ignore))
		{
			Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
			Vector3[] v = mesh.vertices;
			
			for(int i = 0; i < v.Length;i++)
			{
				float n = Vector3.Dot(v[i] - velocity, hit.normal);
			
				if(n > 0.00f) 
				{
					if(hit.distance < dist)
					{
						Vector3 returnPos = hit.point + Vector3.down * dist; 
						transform.position = Vector3.Lerp(transform.position, returnPos, posRecover * Time.fixedDeltaTime);
						
						velocity.y = 0;
					}		
					break;
				}
			}
		}

		if(Physics.Raycast(upRay, out hit, dist + 0.05f, colMask,QueryTriggerInteraction.Ignore)&&
		Physics.Raycast(upForwardRay, out hit, dist + 0.05f, colMask,QueryTriggerInteraction.Ignore))
		{
			Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
			Vector3[] v = mesh.vertices;
			
			for(int i = 0; i < v.Length;i++)
			{
				float n = Vector3.Dot(v[i] - velocity, hit.normal);
			
				if(n > 0.00f || n < 0.00f) 
				{	
					if(hit.distance < dist)
					{
						transform.position = Vector3.Lerp(transform.position, hit.point + Vector3.down * dist, posRecover * Time.fixedDeltaTime);
						velocity.y = 0;//-gravity;
					}	
					
					velocity.x = 0;
					velocity.z = 0;
					break;
				}
			}
		}*/

		if(right !=0)
		{
			if(Physics.Raycast(rightRay, out hit, distH+0.05f, colMask,QueryTriggerInteraction.Ignore))
			{
				Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
				Vector3[] v = mesh.vertices;

				for(int i = 0; i < v.Length;i++)
				{
					float n = Vector3.Dot(v[i] - velocity, hit.normal);

					if(n > 0.00f || n < 0.00f)
					{
						velocity.x = 0;
						break;
					}
				}		
			}
		}
		
		if(forward !=0)
		{		
			if(Physics.Raycast(frontRay, out hit, distH+0.05f, colMask,QueryTriggerInteraction.Ignore))
			{
				Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
				Vector3[] v = mesh.vertices;

				for(int i = 0; i < v.Length;i++)
				{
					float n = Vector3.Dot(v[i] - velocity, hit.normal);

					if(n > 0.00f || n < 0.00f)
					{
						velocity.z = 0;
						break;
					}
				}		
			}
		}
		//--------------------------
		// Collision correction with Linear interpolation
		//--------------------------
		if(ColCorrect)
		{
			if(Physics.Raycast(uRightRay, out hit, distH, colMask,QueryTriggerInteraction.Ignore))
			{
				Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
				Vector3[] v = mesh.vertices;

				for(int i = 0; i < v.Length;i++)
				{
					float n = Vector3.Dot(v[i] - velocity, hit.normal);

					if(n > 0.00f || n < 0.00f)
					{
						if(hit.distance < distH)
						{
							transform.position = Vector3.Lerp(transform.position, hit.point + (-transform.right) * distH, posRecover * Time.fixedDeltaTime);
						}
						
						break;
					}
				}		
			}
			
			if(Physics.Raycast(uLeftRay, out hit, distH, colMask,QueryTriggerInteraction.Ignore))
			{
				Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
				Vector3[] v = mesh.vertices;	

				for(int i = 0; i < v.Length;i++)
				{
					float n = Vector3.Dot(v[i] - velocity, hit.normal);

					if(n > 0.00f || n < 0.00f)
					{
						if(hit.distance < distH)
						{
							transform.position = Vector3.Lerp(transform.position, hit.point + (transform.right) * distH, posRecover * Time.fixedDeltaTime);
						}
						
						break;
					}
				}		
			}
			
			if(Physics.Raycast(forwardRightRay, out hit, distH, colMask,QueryTriggerInteraction.Ignore)||
			Physics.Raycast(forwardRightRay2, out hit, distH, colMask,QueryTriggerInteraction.Ignore))
			{
				Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
				Vector3[] v = mesh.vertices;	

				for(int i = 0; i < v.Length;i++)
				{
					float n = Vector3.Dot(v[i] - velocity, hit.normal);

					if(n > 0.00f || n < 0.00f)
					{
						velocity.x = 0;
						//velocity.z = 0;
						/*if(hit.distance < distH)
						{
							transform.position = Vector3.Lerp(transform.position, hit.point + (transform.right) * distH, posRecover * Time.fixedDeltaTime);
						}*/
						
						break;
					}
				}		
			}
			
			/*if(Physics.Raycast(downForwardRay, out hit, distV - 0.05f, colMask,QueryTriggerInteraction.Ignore))
			{
				Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
				Vector3[] v = mesh.vertices;
				
				//float n2 = (Vector3.Dot(hit.normal,Vector3.up));
				//float ncos = Mathf.Acos(n2);
				//float ang = (Mathf.Rad2Deg*ncos);	
				
				for(int i = 0; i < v.Length;i++)
				{
					float n = Vector3.Dot(v[i] - velocity, hit.normal);
				
					if(n > 0.00f) 
					{
						if(hit.distance < distV)
						{
							transform.position = Vector3.Lerp(transform.position, hit.point + Vector3.up * distV, posRecover * Time.fixedDeltaTime);
						}
							
						break;
					}
		
				}
			}*/
		}
	}
		
	//Doesn't need to be a function, but is a nice abstraction
	void Move(Vector3 vec)
	{		
		transform.Translate(vec);
	}
		
	void Update()
	{
		//if(isLocalPlayer)
		MouseLook();
	}
	
	void mainLoopFunction()
	{
		movementDirection = Vector3.zero;
		
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
		
		MoveWalk(input.y,input.x);

		
		//Only for testing, not used in final version
		//if(Input.GetKeyDown(KeyCode.Space))
		//{
			//cameraView.transform.position = Vector3.Lerp(cameraView.transform.position,
				//							cameraView.transform.position + new Vector3(0,-1f,0), Time.fixedDeltaTime);
		//	movementDirection.y = jumpSpeed;
		//}

		CollisionDetection(ref movementDirection, input.y,input.x,true);

		Move(movementDirection * Time.fixedDeltaTime);
		
		actorVelocity.y += -gravity * Time.fixedDeltaTime;
		
		CollisionDetection(ref actorVelocity, 0,0,false);
		Move(actorVelocity * Time.fixedDeltaTime);
	}
	
    void FixedUpdate()
    {
		//if(isLocalPlayer)
		 mainLoopFunction();
    }
}
