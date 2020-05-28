using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    public void PlaySound(string path)
    {
        UnityEngine.Debug.Log("Borde spela ett ljud");
        FMODUnity.RuntimeManager.PlayOneShot(path, transform.position);
    }
}
