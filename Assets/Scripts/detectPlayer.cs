using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class detectPlayer : MonoBehaviour
{
    public Transform player;
    public float maxAngle = 50;
    public float maxRadius = 5;
    private bool fovtest123 = false;

    private void Update()
    {
        try
        {
            player = GetPlayer.Instance.getPlayer().transform;
        }
        catch
        {
            //we are in the first frame, nothing bad just ignore this
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRadius);

        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        if (!fovtest123)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, (player.position - transform.position).normalized * maxRadius);
        
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * maxRadius);
    }
    
    public static bool inFov(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {
        /*  Collider[] overlaps = new Collider[10];
          int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

          for (int i = 0; i < count + 1; i++)
          {

              if (overlaps[i] != null)
              {
                  if (overlaps[i].transform == target)
                  {
                      Vector3 direction = (target.position - checkingObject.position).normalized;
                      direction.y *= 0;

                      float angle = Vector3.Angle(checkingObject.forward, direction);
                      if(angle <= maxAngle)
                      {
                          Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                          RaycastHit hit;

                          if(Physics.Raycast(ray, out hit, maxRadius))
                          {
                              if(hit.transform == target)
                              {
                                  return true;
                              }
                          }
                      }
                  }
              }
          }


          return false;
          

        Vector3 direction = (target.position - checkingObject.position).normalized;
        direction.y *= 0;

        RaycastHit hit;
        if (Physics.Raycast(checkingObject.position, (target.position - checkingObject.position).normalized, out hit, maxRadius))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                float angle = Vector3.Angle(checkingObject.forward, direction);

                if (angle <= maxAngle)
                {
                    return true;
                }
            }
        }
        */
        return false;
        
    }
    /*
    private void Update()
    {
        fovtest123 = inFov(transform, player, maxAngle, maxRadius);
        if(fovtest123 == true)
        {
            Debug.Log("xdxdxd");
        }
    }
    */



}
