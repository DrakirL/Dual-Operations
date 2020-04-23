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
        GameManager._instance.WinState();
        activated = true;    
    }

    public bool IsActivated() => (activated) ? true : false;
}