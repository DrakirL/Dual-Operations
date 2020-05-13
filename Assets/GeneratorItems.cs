using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GeneratorItems : NetworkBehaviour
{
    public static GeneratorItems Instance { get; private set; }
    [System.Serializable]
    public class GeneratorList
    {
        [Tooltip("The objects bound to this generator")]
        public GameObject[] generatorObjects;
    }
    [Tooltip("Number of generators that exists on the map")]
    public GeneratorList[] generators;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Deactivates all objects in the generators
        for (int i = 0; i < generators.Length; i++)
        {
            ActivateGeneratorItems(i, false);
        }    
    }

    // Activates all objects in a specific generator
    void ActivateGeneratorItems(int genNum, bool boolToSet)
    {
        for (int i = 0; i < generators[genNum].generatorObjects.Length; i++)
        {
            generators[genNum].generatorObjects[i].SetActive(boolToSet);
        }
    }
}
