using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour, IInteractable
{
    public void GetInteracted(List<int> io)
    {
        if (GameManager._instance.winState)
        {
            GameManager._instance.LoadScene("Win", 0);
        }
    }
}
