using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    bool open = false;
    public GameObject hinge;
    Animator anim;

    private void Start()
    {
        // Fetch animator from hinge component
        anim = hinge.GetComponent<Animator>();
    }

    public void GetInteracted()
    {
        if (!open)
        {
            //hinge.transform.Rotate(transform.rotation.x, transform.rotation.y + 90f, transform.rotation.z, Space.World);          
            anim.Play("Open");
            open = true;            
        }
        else
        {
            //hinge.transform.Rotate(transform.rotation.x, transform.rotation.y - 90f, transform.rotation.z, Space.World);           
            anim.Play("Close");
            open = false;
        }
    }
}
