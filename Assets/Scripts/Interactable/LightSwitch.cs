using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    bool on = false;
    public Animator anim;
    public GameObject light;

    public void GetInteracted()
    {
        if (!on)
        {
            light.SetActive(true);          
            anim.Play("OnIdle");
            on = true;
        }
        else
        {
            light.SetActive(false);         
            anim.Play("OffIdle");
            on = false;
        }
    }
}

