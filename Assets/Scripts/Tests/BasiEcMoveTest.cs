using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasiEcMoveTest : MonoBehaviour
{
    //this is just a script i copied over to controll a frog to see if the camera can notice toe player
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpStrength;
    [SerializeField] int amountOfJumps = 2;
    int amountOfJumpsLeft;

    [SerializeField] float cameraMoveSpeed;
    [SerializeField] Transform cameraTransform;
    Rigidbody playerRB;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            playerRB = gameObject.GetComponent<Rigidbody>();
        }
        catch
        {
            Debug.LogError("this object misses one or more necisary components!");
        }

    }
    void Update()
    {
        //update rotation for the camera
        //cameraTransform.Translate(new Vector3(-Input.mousePosition.y, Input.mousePosition.x, 0) * cameraMoveSpeed);
        //rotate the player
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, cameraTransform.eulerAngles.y, transform.eulerAngles.z);

    }
    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && amountOfJumpsLeft > 0)
        {
            amountOfJumpsLeft--;
            //the five is the max abs value of the velocity (in y) during a jump
            playerRB.AddForce(Vector3.up * (jumpStrength - (playerRB.velocity.y / 5) * jumpStrength));

        }
        RaycastHit hit;
        Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity);
        if (hit.distance <= 1)
        {
            //minus one cause as soon as you yoump you regain a jump
            amountOfJumpsLeft = (amountOfJumps - 1);
        }

        if (Input.GetKey(KeyCode.W))
        {
            playerRB.MovePosition(transform.position + (transform.forward * movementSpeed));
        }
        if (Input.GetKey(KeyCode.D))
        {
            //  playerRB.MovePosition(transform.position + (transform.right * movementSpeed));
            transform.Rotate(Vector3.up * cameraMoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            //playerRB.MovePosition(transform.position + (-transform.right * movementSpeed));
            transform.Rotate(Vector3.up * -cameraMoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerRB.MovePosition(transform.position + (-transform.forward * movementSpeed));
        }
    }

}