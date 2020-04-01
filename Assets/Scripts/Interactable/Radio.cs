using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour, IInteractable
{
    bool on = false;
    AudioSource sound;

    private void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    public void GetInteracted()
    {
        if (!on)
        {        
            sound.Play();
            on = true;
        }
        else
        {
            sound.Pause();
            on = false;
        }
    }
}
