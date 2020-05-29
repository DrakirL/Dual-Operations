using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditDetach : MonoBehaviour
{
    [SerializeField] GameObject enterObject; 
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == enterObject)
        {
            transform.parent = null;
        }
    }
}
