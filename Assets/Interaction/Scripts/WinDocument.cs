using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDocument : MonoBehaviour, IInteractable
{
    bool activated = false;
    public void GetInteracted(List<int> io)
    {
        if(!activated)
            GetPlayer.Instance.LoadScene("Win", 0.5f);
        activated = true;
    }
}
