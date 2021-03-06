﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mirror
{
    class MouseAimC : NetworkBehaviour
    {
        public Transform cameraView;
        public Transform objectModel;

        public enum MouseAimStyle
        {
            VerticalHorizontal,
            HorizontalOnly
        };

        public MouseAimStyle mouseAimStyle;

        [Range(0, 20)] public float mouseSensitivity = 4;
        public bool mouseInverted = false;

        public float moveSpeed = 2;

        //private Vector2 viewLimit = new Vector2(-90,90);
        //private Vector2 XLimit = new Vector2(-90,90);
        private Vector2 rot = Vector2.zero;

        Vector3 movementDirection = Vector3.zero;
        Vector3 actorVelocity = Vector3.zero;

        public float gravity = 10;
        //public float acceleration = 10;
        //public float deacceleration = 3;
        //public float friction = 4;

        public float cameraOffsetY = 9;

        public LayerMask colMask;// = LayerMask.GetMask("StaticWorldObject");

        //private bool isGrounded = false;
        //public float slopeRise = 1f;
        //public float slopeFall = 1f;
        public float height = 0.5f;
        public float jumpSpeed = 10f;

        float posRecover = 10f;



        void Start()
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
            cameraView.transform.position = new Vector3(transform.position.x, transform.position.y + cameraOffsetY, transform.position.z);
        }

        void MoveWalk(float forward, float right)
        {
            //Get a pure direction vector

            //MoveFriction(friction);

            Vector3 movdir = new Vector3(right, 0, forward);
            movdir.Normalize();

            //float speed = Vector3.Magnitude(movdir);
            //speed *= moveSpeed;

            //MoveAccelerate(movdir, speed, acceleration);

            movementDirection.x += moveSpeed * movdir.x;
            movementDirection.z += moveSpeed * movdir.z;



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
            transform.Translate(vec.x, 0, 0, cameraView.transform);
            transform.Translate(0, 0, vec.z, Space.Self);
            transform.Translate(0, vec.y, 0, Space.Self);
        }

        void MouseLook()
        {
            int inv = Convert.ToInt32(mouseInverted == true ? 1 : -1);
            Vector2 viewLimit = new Vector2(-90, 90);
            Vector2 XLimit = new Vector2(-90, 90);

            rot.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;

            if (mouseAimStyle == MouseAimStyle.HorizontalOnly)
            {
                rot.y = 0;
            }
            else
            {
                rot.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

                //Clamp y-axis view
                rot.y = Mathf.Clamp(rot.y, viewLimit.x, viewLimit.y);
            }

            cameraView.transform.eulerAngles = new Vector2(0, rot.x);

            float y_axis = (inv) * rot.y;
            float x_axis = rot.x;

            cameraView.transform.localRotation = Quaternion.Euler(y_axis, x_axis, 0);
            transform.localRotation = Quaternion.Euler(0, x_axis, 0);
            objectModel.transform.localRotation = Quaternion.Euler(0, 0, 0);//Quaternion.Euler(y_axis,0,0);
        }

        void Collision(ref Vector3 movement, float forward, float right)
        {
            RaycastHit hit;

            float dist = (GetComponent<SphereCollider>().radius) * 0.5f;

            Ray downRay = new Ray(transform.position, Vector3.down);
            Ray upRay = new Ray(transform.position, Vector3.up);
            Ray upForwardRay = new Ray(transform.position, Vector3.up + (forward * transform.forward));
            Ray downForwardRay = new Ray(transform.position, Vector3.down + (forward * transform.forward));

            Ray frontRay = new Ray(transform.position, forward * transform.forward);
            Ray rightRay = new Ray(transform.position, right * transform.right);
            Ray uRightRay = new Ray(transform.position, transform.right);
            Ray uLeftRay = new Ray(transform.position, -transform.right);


            /*Debug.DrawLine(downRay.origin,downRay.origin + downRay.direction * dist, Color.green);
            Debug.DrawLine(rightRay.origin,rightRay.origin + rightRay.direction * dist, Color.red);	
            Debug.DrawLine(downForwardRay.origin,downForwardRay.origin + downForwardRay.direction * dist, Color.red);	
            Debug.DrawLine(upForwardRay.origin,upForwardRay.origin + upForwardRay.direction * dist, Color.red);	*/


            //Checking below player for surface
            if (Physics.Raycast(downRay, out hit, dist + 0.05f, colMask, QueryTriggerInteraction.Ignore))
            {
                //MeshCollider meshCollider = hit.collider as MeshCollider;
                //Mesh mesh = meshCollider.sharedMesh;
                Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
                Vector3[] v = mesh.vertices;

                float n2 = (Vector3.Dot(hit.normal, Vector3.up));
                float ncos = Mathf.Acos(n2);
                float ang = (Mathf.Rad2Deg * ncos);

                for (int i = 0; i < v.Length; i++)
                {
                    float n = Vector3.Dot(v[i] - movementDirection, hit.normal);

                    if (n >= 0.01f)
                    {
                        //Height correction with linear interpolation if clipping with mesh's face
                        if (hit.distance < dist)
                        {
                            transform.position = Vector3.Lerp(transform.position, hit.point + Vector3.up * dist, posRecover * Time.fixedDeltaTime);
                        }

                        //isGrounded = true;
                        movementDirection.y = 0;

                        break;
                    }
                }
            }

            if (Physics.Raycast(upRay, out hit, dist + 0.05f, colMask, QueryTriggerInteraction.Ignore))
            {
                //MeshCollider meshCollider = hit.collider as MeshCollider;
                //Mesh mesh = meshCollider.sharedMesh;
                Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
                Vector3[] v = mesh.vertices;

                for (int i = 0; i < v.Length; i++)
                {
                    float n = Vector3.Dot(v[i] - movementDirection, hit.normal);

                    if (n > 0.01f)
                    {

                        //Height correction with linear interpolation if clipping with mesh's face
                        if (hit.distance < dist)
                        {
                            transform.position = Vector3.Lerp(transform.position, hit.point + Vector3.down * dist, posRecover * Time.fixedDeltaTime);
                            movementDirection.y = 0;
                        }

                        //isGrounded = false;		
                        break;
                    }
                }
            }

            if (Physics.Raycast(upForwardRay, out hit, dist + 0.05f, colMask, QueryTriggerInteraction.Ignore))
            {
                //MeshCollider meshCollider = hit.collider as MeshCollider;
                //Mesh mesh = meshCollider.sharedMesh;
                Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
                Vector3[] v = mesh.vertices;

                for (int i = 0; i < v.Length; i++)
                {
                    float n = Vector3.Dot(v[i] - movementDirection, hit.normal);

                    if (n > 0.01f)
                    {
                        movementDirection.y = 0;
                        break;
                    }
                }
            }

            if (right != 0)
            {
                if (Physics.Raycast(rightRay, out hit, dist, colMask, QueryTriggerInteraction.Ignore))
                {
                    //MeshCollider meshCollider = hit.collider as MeshCollider;
                    //Mesh mesh = meshCollider.sharedMesh;
                    Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
                    Vector3[] v = mesh.vertices;

                    for (int i = 0; i < v.Length; i++)
                    {
                        float n = Vector3.Dot(v[i] - movementDirection, hit.normal);

                        if (n > 0.01f || n < 0.01f)
                        {
                            movementDirection.x = 0;
                            break;
                        }
                    }
                }
            }

            if (forward != 0)
            {
                if (Physics.Raycast(frontRay, out hit, dist + 0.05f, colMask, QueryTriggerInteraction.Ignore))
                {
                    //MeshCollider meshCollider = hit.collider as MeshCollider;
                    //Mesh mesh = meshCollider.sharedMesh;
                    Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
                    Vector3[] v = mesh.vertices;

                    for (int i = 0; i < v.Length; i++)
                    {
                        float n = Vector3.Dot(v[i] - movementDirection, hit.normal);

                        if (n > 0.01f || n < 0.01f)
                        {
                            movementDirection.z = 0;
                            break;
                        }
                    }
                }
            }
            //--------------------------
            // Collision correction for "wall-hugging" etc.
            //--------------------------

            if (Physics.Raycast(uRightRay, out hit, dist, colMask, QueryTriggerInteraction.Ignore))
            {
                //MeshCollider meshCollider = hit.collider as MeshCollider;
                //Mesh mesh = meshCollider.sharedMesh;
                Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
                Vector3[] v = mesh.vertices;

                for (int i = 0; i < v.Length; i++)
                {
                    float n = Vector3.Dot(v[i] - movementDirection, hit.normal);

                    if (n > 0.01f || n < 0.01f)
                    {
                        if (hit.distance < dist)
                        {
                            transform.position = Vector3.Lerp(transform.position, hit.point + (-transform.right) * dist, posRecover * Time.fixedDeltaTime);
                        }

                        break;
                    }
                }
            }

            if (Physics.Raycast(uLeftRay, out hit, dist, colMask, QueryTriggerInteraction.Ignore))
            {
                //MeshCollider meshCollider = hit.collider as MeshCollider;
                //Mesh mesh = meshCollider.sharedMesh;
                Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
                Vector3[] v = mesh.vertices;

                for (int i = 0; i < v.Length; i++)
                {
                    float n = Vector3.Dot(v[i] - movementDirection, hit.normal);

                    if (n > 0.01f || n < 0.01f)
                    {
                        if (hit.distance < dist)
                        {
                            transform.position = Vector3.Lerp(transform.position, hit.point + (transform.right) * dist, posRecover * Time.fixedDeltaTime);
                        }

                        break;
                    }
                }
            }

            if (Physics.Raycast(downRay, out hit, dist + 0.05f, colMask, QueryTriggerInteraction.Ignore) &&
            Physics.Raycast(downForwardRay, out hit, dist, colMask, QueryTriggerInteraction.Ignore))
            {
                //MeshCollider meshCollider = hit.collider as MeshCollider;
                //Mesh mesh = meshCollider.sharedMesh;
                Mesh mesh = hit.transform.GetComponent<MeshFilter>().mesh;
                Vector3[] v = mesh.vertices;

                for (int i = 0; i < v.Length; i++)
                {
                    float n = Vector3.Dot(v[i] - movementDirection, hit.normal);

                    if (n > 0.01f || n < 0.01f)
                    {
                        //Height correction with linear interpolation if clipping with mesh's face
                        if (hit.distance < dist)
                        {
                            transform.position = Vector3.Lerp(transform.position, hit.point + Vector3.up * dist, posRecover * Time.fixedDeltaTime);
                        }
                        movementDirection.y = 0;

                        break;
                    }
                }
            }

            //------------------------
        }

        void Update()
        {
            MouseLook();

            //Forward vector, only debbuging
            //------
            Ray front = new Ray(transform.position, transform.forward);
            Debug.DrawLine(front.origin, front.direction * 60, Color.blue);
            //------

            cameraView.position = new Vector3(transform.position.x, transform.position.y + cameraOffsetY, transform.position.z);
        }


        void FixedUpdate()
        {
            movementDirection = Vector3.zero;

            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


            MoveWalk(input.y, input.x);

            movementDirection.y = -gravity;

            if (Input.GetKey(KeyCode.Space))
            {

                movementDirection.y = jumpSpeed;

                //isGrounded = false;
                //Move(movementDirection * Time.fixedDeltaTime);
            }

            Collision(ref movementDirection, input.y, input.x);

            //final transform calculation
            Move(movementDirection * Time.fixedDeltaTime);
        }
    }
}
