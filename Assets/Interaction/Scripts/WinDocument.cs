using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinDocument : MonoBehaviour, IInteractable
{

    //private Image img;

    //void Start()
    //{
    //    img = GameObject.FindGameObjectWithTag("USB");
    //    img.enabled = false;
    //}

    public void GetInteracted(List<int> io)
    {
        GameManager._instance.winState = true;
       // img.enabled = true;
        Destroy(this.gameObject);
    }
}
