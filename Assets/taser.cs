using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class taser : NetworkBehaviour
{

    public Camera aimCam;
    public float range;
    public bool bengt = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("fire");
                Shoot();
            }
        }
        


        if (bengt)
        {
            Shoot();
        }
    }

    
    public void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(aimCam.transform.position, aimCam.transform.forward, out hit, range))
        {
            AI target = hit.transform.GetComponent<AI>();
            Debug.Log(target);
            if (target != null)
            {
                target.dead = true;
            }
        }
    }
}
