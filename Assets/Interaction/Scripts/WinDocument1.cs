﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDocument1 : MonoBehaviour, IInteractable
{
    public void GetInteracted(List<int> io)
    {
        //GameManager._instance.winState = true;
        Destroy(this.gameObject);
    }
}
