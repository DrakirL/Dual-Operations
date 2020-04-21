using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualOperationsAudioPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void PlaySound(string path)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path, transform.position);
    }
}
