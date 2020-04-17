using UnityEngine;

public class LightSwitch : MonoBehaviour, IInteractable
{
    public Animator switchAnim;
    public new GameObject light;

    public void GetInteracted()
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

