using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class AgentControllerScript : NetworkBehaviour
{
    public Transform cameraView;
    public Transform objectModel;

    public enum MouseAimStyle
    {
        VerticalHorizontal,
        HorizontalOnly
    };

    enum playerState
    {
        eSneak,
        eWalk,
        eSprint
    };

    public MouseAimStyle mouseAimStyle;

    [Range(0, 20)] public float mouseSensitivity = 4;

    public bool mouseInverted = false;

    public float moveSpeed = 3;
    public float gravity = 10;
    //public float maxSlopeAngle = 25f;

    public LayerMask colMask;

    private Vector2 rot = Vector2.zero;
    private Vector3 movementDirection = Vector3.zero;
    private Vector3 actorVelocity = Vector3.zero;

    private float posRecover = 10f;

    [Range(0.05f, 0.25f)] public float accelMod = 0.1f;
    [Range(0.05f, 0.25f)] public float deaccelMod = 0.2f;
    protected float accelSpeed;
    protected float deaccelSpeed;

    bool wishMouse = true;

    public KeyCode moveForward = KeyCode.W;
    public KeyCode moveBackward = KeyCode.S;
    public KeyCode strafeLeft = KeyCode.A;
    public KeyCode strafeRight = KeyCode.D;
    public KeyCode interactKey = KeyCode.E;
    //public KeyCode fireKey = KeyCode.Space;
    //public float deacceleration = 3;
    //public float friction = 4;
    //private float jumpSpeed = 10f;

    void Start()
    {
        if (isLocalPlayer)
        {
            if (cameraView == null)
            {
                Camera mainCamera = Camera.main;
                if (mainCamera != null)
                {
                    cameraView = mainCamera.gameObject.transform;
                }
            }

            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    Vector3 lastdir = Vector3.zero;

    void MoveWalk(ref Vector3 velocity)
    {
        Vector3 movdir = new Vector3(velocity.x, 0, velocity.z);
        movdir.Normalize();

        if ((movdir.x != 0 || movdir.z != 0))
        {
            accelSpeed = MoveAccelerate(accelSpeed);
            deaccelSpeed = accelSpeed;
            lastdir = new Vector3(velocity.x, 0, velocity.z);
        }
        else
        {
            deaccelSpeed = MoveDeaccelerate(deaccelSpeed);
            accelSpeed = deaccelSpeed;
            movdir = new Vector3(lastdir.x, 0, lastdir.z);
        }

        velocity.x = movdir.x * accelSpeed;
        velocity.z = movdir.z * accelSpeed;
    }

    float MoveAccelerate(float speed)
    {
        if (speed >= moveSpeed)
        {
            speed = moveSpeed;
            return speed;
        }

        speed = speed + accelMod;
        return speed;
    }

    float MoveDeaccelerate(float speed)
    {
        if (speed <= 0)
        {
            speed = 0;
            return speed;
        }

        speed = speed - deaccelMod;
        return speed;
    }

    void MouseLook()
    {
        int inv = Convert.ToInt32(mouseInverted == true ? 1 : -1);
        Vector2 viewLimit = new Vector2(-90, 90);

        rot.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;

        if (mouseAimStyle == MouseAimStyle.HorizontalOnly)
        {
            rot.y = 0;
        }
        else
        {
            rot.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
            rot.y = Mathf.Clamp(rot.y, viewLimit.x, viewLimit.y);
        }

        float y_axis = (inv) * rot.y;
        float x_axis = rot.x;

        cameraView.transform.localRotation = Quaternion.Euler(y_axis, 0, 0);
        objectModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.localRotation = Quaternion.Euler(0, x_axis, 0);
    }

    void CollisionDetection(ref Vector3 velocity, bool ColCorrect)
    {
        RaycastHit hit;

        float distH = (GetComponent<CapsuleCollider>().radius);
        float distV = (GetComponent<CapsuleCollider>().height) * 0.5f;

        Ray downRay = new Ray(transform.position, Vector3.down);

        Ray frontRay = new Ray(transform.position, velocity.z * transform.forward);
        Ray rightRay = new Ray(transform.position, velocity.x * transform.right);
        Ray uRightRay = new Ray(transform.position, transform.right);
        Ray uLeftRay = new Ray(transform.position, -transform.right);

        // unused
        //float dist = (GetComponent<SphereCollider>().radius)*0.5f;
        //Ray upRay =  new Ray (transform.position, Vector3.up);
        //Ray downForwardRay =  new Ray (transform.position, Vector3.down+(forward*transform.forward));
        //Ray upForwardRay =  new Ray (transform.position, Vector3.up+(forward*transform.forward));


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

        if (Physics.Raycast(downRay, out hit, distV + 0.05f, colMask, QueryTriggerInteraction.Ignore))
        {
            Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
            Vector3[] v = mesh.vertices;

            for (int i = 0; i < v.Length; i++)
            {
                float n = Vector3.Dot(v[i] - velocity, hit.normal);

                if (n > 0.00f)
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

        //if(right !=0)
        //{
        if (Physics.Raycast(rightRay, out hit, distH + 0.05f, colMask, QueryTriggerInteraction.Ignore))
        {
            Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
            Vector3[] v = mesh.vertices;

            for (int i = 0; i < v.Length; i++)
            {
                float n = Vector3.Dot(v[i] - velocity, hit.normal);

                if (n > 0.00f || n < 0.00f)
                {
                    velocity.x = 0;
                    break;
                }
            }
        }
        //}

        //if(forward !=0)
        //{		
        if (Physics.Raycast(frontRay, out hit, distH + 0.05f, colMask, QueryTriggerInteraction.Ignore))
        {
            Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
            Vector3[] v = mesh.vertices;

            for (int i = 0; i < v.Length; i++)
            {
                float n = Vector3.Dot(v[i] - velocity, hit.normal);

                if (n > 0.00f || n < 0.00f)
                {
                    velocity.z = 0;
                    break;
                }
            }
        }
        //}
        //--------------------------
        // Collision correction with Linear interpolation
        //--------------------------
        if (ColCorrect)
        {
            if (Physics.Raycast(uRightRay, out hit, distH + 0.05f, colMask, QueryTriggerInteraction.Ignore))
            {
                Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
                Vector3[] v = mesh.vertices;

                for (int i = 0; i < v.Length; i++)
                {
                    float n = Vector3.Dot(v[i] - velocity, hit.normal);

                    if (n > 0.00f || n < 0.00f)
                    {
                        if (hit.distance < distH)
                        {
                            transform.position = Vector3.Lerp(transform.position, hit.point + (-transform.right) * distH, posRecover * Time.fixedDeltaTime);
                        }

                        break;
                    }
                }
            }

            if (Physics.Raycast(uLeftRay, out hit, distH + 0.05f, colMask, QueryTriggerInteraction.Ignore))
            {
                Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
                Vector3[] v = mesh.vertices;

                for (int i = 0; i < v.Length; i++)
                {
                    float n = Vector3.Dot(v[i] - velocity, hit.normal);

                    if (n > 0.00f || n < 0.00f)
                    {
                        if (hit.distance < distH)
                        {
                            transform.position = Vector3.Lerp(transform.position, hit.point + (transform.right) * distH, posRecover * Time.fixedDeltaTime);
                        }

                        break;
                    }
                }
            }

            /*if(Physics.Raycast(downForwardRay, out hit, dist + 0.05f, colMask,QueryTriggerInteraction.Ignore))
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
						if(hit.distance < dist)
						{
							transform.position = Vector3.Lerp(transform.position, hit.point + Vector3.up * dist, posRecover * Time.fixedDeltaTime);
						}
							
						break;
					}
		
				}
			}*/
        }

        //------------------------
    }

    void Move(Vector3 vec)
    {
        transform.Translate(vec);
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                wishMouse = !wishMouse;

            if (wishMouse)
            {
                Cursor.lockState = CursorLockMode.Locked;
                MouseLook();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
                    wishMouse = true;
            }
        }
    }

    Vector2 keyDirection = Vector2.zero;
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKey(moveForward))
                keyDirection.y = 1f;

            if (Input.GetKey(moveBackward))
                keyDirection.y = -1f;

            if (Input.GetKey(strafeRight))
                keyDirection.x = 1f;

            if (Input.GetKey(strafeLeft))
                keyDirection.x = -1f;

            Vector2 input = new Vector2(keyDirection.x, keyDirection.y);//Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            movementDirection = new Vector3(input.x, 0, input.y);

            MoveWalk(ref movementDirection);
            CollisionDetection(ref movementDirection, true);

            Move(movementDirection * Time.fixedDeltaTime);

            actorVelocity.y += -gravity * Time.fixedDeltaTime;
            CollisionDetection(ref actorVelocity, false);

            Move(actorVelocity * Time.fixedDeltaTime);
            movementDirection = Vector3.zero;
            keyDirection = Vector2.zero;
        }
    }
}
