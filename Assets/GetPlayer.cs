using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayer : MonoBehaviour
{
    private static GetPlayer instance;
    public static GetPlayer Instance { get { return instance; } }
    // Use this for initialization
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public GameObject getPlayer()
    {
        return this.gameObject;
    }
}
