using UnityEngine;
using System.Collections.Generic;

public class Radio : MonoBehaviour, IInteractable
{
    AudioSource sound;

    private void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    public void GetInteracted(List<int> io)
    {
        if (!SoundIsPlaying())
        {        
            sound.Play();
        }
        else
        {
            sound.Pause();
        }
    }

    bool SoundIsPlaying() => (sound.isPlaying) ? true : false;
}
