﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public GameObject hinge;

    Animator anim;
    BoxCollider col;
    
    private void Start()
    {
        // Fetch animator from hinge component
        anim = hinge.GetComponent<Animator>();
        col = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        EnableDoor();
    }
 
    public void GetInteracted()
    {
        if (DoorIsClosed())
        {       
            anim.Play("Open");          
        }
        else
        {          
            anim.Play("Close");
        }
    }
    bool DoorIsClosed() => (anim.GetCurrentAnimatorStateInfo(0).IsName("ClosedIdle")) ? true : false;
    bool DoorIsIdle() => (anim.GetCurrentAnimatorStateInfo(0).IsName("OpenIdle") || anim.GetCurrentAnimatorStateInfo(0).IsName("ClosedIdle")) ? true : false;

    // Enables trigger collider for interaction
    void EnableDoor()
    {
        if (DoorIsIdle())
            col.enabled = true;
        else
            col.enabled = false;
        
    }
}
