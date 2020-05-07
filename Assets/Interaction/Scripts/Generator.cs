using UnityEngine;
using System.Collections.Generic;

public class Generator : MonoBehaviour, IInteractable
{
    [SerializeField] bool activated;
    [Tooltip("These object's colliders will be activated when generator is interacted with")]
    [SerializeField] GameObject[] doors;
    [Tooltip("These object's will be activated when generator is interacted with")]
    [SerializeField] GameObject[] gameObjects;

    private void Start()
    {
        Activate(false);
    }

    public void GetInteracted(List<int> io)
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
        }
        foreach (GameObject go in gameObjects)
        {
            go.SetActive(boolToSet);           
        }
        activated = boolToSet;
    }
}