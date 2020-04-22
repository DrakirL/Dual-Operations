using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public class AnnoyingTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(NoiseLoop());
    }

    IEnumerator NoiseLoop()
    {
        while (true)
        {
            //Trigger event
            DualOperationsAudioPlayer.audioPlayer.TempFunc(transform.gameObject);
            yield return new WaitForSeconds(1.0f);
        }
    }
}