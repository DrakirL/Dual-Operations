using UnityEngine;
using System.Collections.Generic;
using Mirror;

public class Generator : NetworkBehaviour, IInteractable
{
    [SyncVar]
    [SerializeField] bool activated;
    [Tooltip("These object's colliders will be activated when generator is interacted with")]
    [SerializeField] GameObject[] doors;
    [Tooltip("Connected to the index in the GeneratorItems script in the hackers' map holder. Starts at 0")]
    [SerializeField] int generatorNum;

    public void GetInteracted(List<int> io)
    {
        if(!activated)
            Activate(true);     
    }

    [Command]
    void CmdActivate(bool boolToSet)
    {
        //foreach (GameObject go in gameObjects)
        foreach (GameObject go in GeneratorItems.Instance.generators[generatorNum].generatorObjects)
        {
            go.SetActive(boolToSet);
        }
    }

    // Activates the door's trigger collider
    void Activate(bool boolToSet)
    {
        activated = true;
        foreach (GameObject door in doors)
        {
            door.GetComponent<BoxCollider>().enabled = boolToSet;
            door.GetComponent<SlideDoor>().active = boolToSet;
        }
        CmdActivate(boolToSet);
    }
}