using UnityEngine;
using System.Collections.Generic;

public class Generator : MonoBehaviour, IInteractable
{  
    [SerializeField] bool activated;
    [Tooltip("These object's colliders will be activated when generator is interacted with")]
    [SerializeField] GameObject[] doors;
    [Tooltip("Connected to the index in the GeneratorItems script in the hackers' map holder. Starts at 0")]
    [SerializeField] int generatorNum;

    public GameObject objj;

    public void GetInteracted(List<int> io)
    {
        if(!activated)
            GetPlayer.Instance.ActivateGeneratorItemsServer(generatorNum, true);
        activated = true;
        Destroy(objj);
    }
}