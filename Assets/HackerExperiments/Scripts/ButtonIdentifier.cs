using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

//really bad name of the class since the function of this script changed during it's creation
public class ButtonIdentifier : MonoBehaviour
{
    [HideInInspector] public UnityEvent UE;
    [HideInInspector] public HackerButtonHandler HBH;

    public void function()
    {
        UE.Invoke();
        HBH.takeDownOptions();
    }
}