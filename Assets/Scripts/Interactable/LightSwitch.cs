using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    public Animator anim;
    public new GameObject light;

    public void GetInteracted()
    {
        if (!LightIsOn())
        {
            light.SetActive(true);          
            anim.Play("OnIdle");
        }
        else
        {
            light.SetActive(false);         
            anim.Play("OffIdle");
        }
    }

    bool LightIsOn() => (light.activeSelf) ? true : false;
}

