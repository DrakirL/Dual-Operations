using UnityEngine;

public class CameraScript : MonoBehaviour
{

    [SerializeField] CameraManager removeMe;
    Camera thisCamera;
    Plane[] planes;
    [HideInInspector] public bool cameraActive;
    [HideInInspector] public float detectedTime = 0;
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
    public bool isObjectVisible(GameObject objectToFind, Collider objectCollider, float distance, float timeBeforeDetected)
    {
        if (cameraActive)
        {
            if (Vector3.Distance(objectToFind.transform.position, this.transform.position) <= distance)
            {
                RaycastHit objectHit;
                Vector3 direction = Vector3.Normalize(objectToFind.transform.position - transform.position);
                Physics.Raycast(transform.position, direction, out objectHit);

                if (GeometryUtility.TestPlanesAABB(planes, objectCollider.bounds) && objectHit.transform.gameObject == objectToFind)
                {
                    detectedTime += Time.deltaTime;
                    if (detectedTime >= timeBeforeDetected)
                    {
                        detectedTime = 0;
                        return true;
                    }
                }
                else
                {
                    detectedTime = 0;
                }
            }
            else
            {
                detectedTime = 0;
            }
        }
        return false;
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, removeMe.cameraFOVDistance);
    }
}
