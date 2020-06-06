using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyThis : MonoBehaviour, IInteractable
{
    public GameObject gen0;

    public void GetInteracted(List<int> io)
    {
        gen0.SetActive(false);
        Debug.Log("Working");
    }
}
