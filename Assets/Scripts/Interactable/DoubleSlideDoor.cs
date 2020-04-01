using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSlideDoor : MonoBehaviour, IInteractable
{
    public Animator anim1;
    public Animator anim2;
    public BoxCollider col;
    bool open = false;

    private void Update()
    {
        if (anim1.GetCurrentAnimatorStateInfo(0).IsName("CloseIdle"))
        {
            open = true;
            col.enabled = true;
        }            
    }
    public void GetInteracted()
    {
        anim1.Play("Open");
        anim2.Play("Open");
        open = false;
        col.enabled = false;
    }
    
}
