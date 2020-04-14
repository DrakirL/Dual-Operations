using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectPlayer : MonoBehaviour
{

    public Transform player;
    public float maxAngle;
    public float maxRadius;
   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
    }
}
