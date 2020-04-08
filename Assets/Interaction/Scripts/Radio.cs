using UnityEngine;

public class Radio : MonoBehaviour, IInteractable
{
    AudioSource sound;

    private void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    public void GetInteracted()
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
