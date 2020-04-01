using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    Camera thisCamera;
    Plane[] planes;
    [HideInInspector] public bool cameraActive;
    // Start is called before the first frame update
    void Start()
    {
        cameraActive = true;
        try
        {
            thisCamera = gameObject.GetComponent<Camera>();
        }
        catch
        {
            Debug.LogError("this game object do not have a camera!");
        }
        planes = GeometryUtility.CalculateFrustumPlanes(thisCamera);
    }
    public bool isObjectVisible(GameObject objectToFind, Collider objectCollider)
    {
        if (cameraActive)
        {
            RaycastHit objectHit;
            Vector3 direction = Vector3.Normalize(objectToFind.transform.position - transform.position);
            Physics.Raycast(transform.position, direction, out objectHit);

            return GeometryUtility.TestPlanesAABB(planes, objectCollider.bounds) && objectHit.transform.gameObject == objectToFind;
        }
        return false;
    }
}
