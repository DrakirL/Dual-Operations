using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class taser : MonoBehaviour
{

    public Camera aimCam;
    public float range;
    public bool bengt = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {

		if(Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log("fire");
			Shoot();
		}
    }


    public void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(aimCam.transform.position, aimCam.transform.forward, out hit, range))
        {
            AI target = hit.transform.GetComponent<AI>();
            //Debug.Log(target);
            if (target != null)
            {
                target.dead = true;
            }
        }
    }
}
