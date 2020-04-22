using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnTransform : MonoBehaviour
{
    private static spawnTransform instance;
    public static spawnTransform Instance { get { return instance; } }
    // Use this for initialization
    void Start()
    {
        
        if (instance == null)
        {
            instance = this;
        }
    }
}
