using UnityEngine;
using System.Collections.Generic;

public class LightSwitch : MonoBehaviour, IInteractable
{
    public Animator switchAnim;
    public new GameObject light;

    public void GetInteracted(List<int> io)
    {
        if (!LightIsOn())
        {         
            switchAnim.Play("OnIdle");
        }
        else
        {        
            switchAnim.Play("OffIdle");
        }

        light.SetActive(!LightIsOn());
    }

    bool LightIsOn() => (light.activeSelf) ? true : false;
}

