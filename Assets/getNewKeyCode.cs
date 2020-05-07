using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getNewKeyCode : MonoBehaviour, IInteractable
{
    [SerializeField] int newKeyCode;
    BoxCollider col;
    private void Start()
    {
        col = GetComponent<BoxCollider>();
    }
    public void GetInteracted(List<int> io)
    {
        if (!io.Contains(newKeyCode))
        {
            GetPlayer.Instance.getPlayer().GetComponent<InputManager>().objectPlayerCanInterRactWith.Add(newKeyCode);
        }
    }
}
