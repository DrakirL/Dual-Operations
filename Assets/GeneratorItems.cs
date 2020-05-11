using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorItems : MonoBehaviour
{
    public static GeneratorItems Instance { get; private set; }
    [System.Serializable]
    public class GeneratorList
    {
        public GameObject[] generatorObjects;
    }

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
        ActivateGeneratorItems(false);
    }

    void ActivateGeneratorItems(bool boolToSet)
    {
        for (int i = 0; i < generators.Length; i++)
        {
            for (int j = 0; j < generators[i].generatorObjects.Length; j++)
            {
                generators[i].generatorObjects[j].SetActive(boolToSet);
            }
        }
    }
}
