using UnityEngine;
using System.Collections.Generic;

public class Generator : MonoBehaviour, IInteractable
{
    bool activated;

    private void Start()
    {
        activated = false;     
    }

    public void GetInteracted(List<int> io)
    {
        // Do generator stuff
        // Eventually use bool in another script to do generator stuff
        if (!activated)
        {
            GameManager._instance.WinState();
            activated = true;
        } 
    }

    public bool IsActivated() => (activated) ? true : false;
}