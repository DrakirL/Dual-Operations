using UnityEngine;

public class Generator : MonoBehaviour, IInteractable
{
    bool activated;

    private void Start()
    {
        activated = false;
    }

    public void GetInteracted()
    {
        // Do generator stuff
        // Eventually use bool in another script to do generator stuff

        activated = true;    
    }

    public bool IsActivated() => (activated) ? true : false;
}