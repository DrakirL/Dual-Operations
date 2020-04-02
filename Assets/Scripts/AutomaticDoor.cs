using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    public bool activated;
    public Animator anim1;
    public Animator anim2;

    private void OnTriggerStay(Collider other)
    {
        if (activated && DoorIsClosed())
        {   
            if(anim1 != null)
            anim1.Play("Open");
            if(anim2 != null)
            anim2.Play("Open");
        }
    }
    bool DoorIsClosed() => (anim1.GetCurrentAnimatorStateInfo(0).IsName("CloseIdle")) ? true : false;
}
