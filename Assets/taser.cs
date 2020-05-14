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
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                Debug.Log("skjut");
                Shoot();
            }
        }
        if (bengt)
        {
            Shoot();
        }
    }
    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(aimCam.transform.position, aimCam.transform.forward, out hit, range))
        {
            AI target = hit.transform.GetComponent<AI>();
            Debug.Log(target);
            if (target != null)
            {
                target.dead = true;
                Debug.Log("xd");
            }
        }
    }
}
