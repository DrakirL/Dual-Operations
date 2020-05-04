using UnityEngine;

public class Generator : MonoBehaviour, IInteractable
{
    [SerializeField] bool activated;
    [Tooltip("These doors will be activated when generator is interacted with6")]
    [SerializeField] GameObject[] doors;

    private void Start()
    {
        Activate(false);
    }

    public void GetInteracted()
    {
        if (!activated)
        {
            Activate(true);          
        }
    }

    // Activates the door's trigger collider
    void Activate(bool boolToSet)
    {
        foreach (GameObject door in doors)
        {
            door.GetComponent<BoxCollider>().enabled = boolToSet;
            door.GetComponent<SlideDoor>().active = boolToSet;
            activated = boolToSet;
        }       
    }
}