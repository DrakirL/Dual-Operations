using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDoor : MonoBehaviour, IInteractable
{
    public Animator anim;

    public void GetInteracted()
    {       
            anim.Play("Open");
    }
}
