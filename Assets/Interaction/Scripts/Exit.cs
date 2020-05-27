using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour, IInteractable
{
    public void GetInteracted(List<int> io)
    {
        if (GameManager._instance.winState)
        {
            GetPlayer.Instance.LoadScene("Win", 0);
        }
    }
}
