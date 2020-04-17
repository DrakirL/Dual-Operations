using UnityEngine;

public class CameraScript : MonoBehaviour
{
    Camera thisCamera;
    Plane[] planes;
    [Tooltip("a varible determain how close the agent needs to be for the camera to pick it up, also this area is rendered with a gizmo")]
    [SerializeField] float distanceCameraCanRegisterAgent;
    [HideInInspector] public bool cameraActive;
    [HideInInspector] public float detectedTime = 0;
    public GameObject lightSource;

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
    }
    public bool isObjectVisible(GameObject objectToFind, Collider objectCollider, float timeBeforeDetected)
    {
        //checks if the camera is activ (not hacked)
        if (cameraActive)
        {
            //see if the agent is within detection range
            if (Vector3.Distance(objectToFind.transform.position, this.transform.position) <= distanceCameraCanRegisterAgent)
            {
                //cast a raycast towards the agent
                RaycastHit objectHit;
                Vector3 direction = Vector3.Normalize(objectToFind.transform.position - transform.position);
                Physics.Raycast(transform.position, direction, out objectHit);
                
                //if the agent is the one getting hit then continue
                //if not, something is standing in the way towards the player 
                //meaning player is hidden => do not alert camera 
                if (objectHit.transform.gameObject == objectToFind)
                {
                    //creates frustum based on the cameras view, and look if agent is inside this frustum
                    planes = GeometryUtility.CalculateFrustumPlanes(thisCamera);
                    if (GeometryUtility.TestPlanesAABB(planes, objectCollider.bounds))
                    {
                        //timmer ensuring that the camera is not spaming out signals
                        detectedTime += Time.deltaTime;
                        if (detectedTime >= timeBeforeDetected)
                        {
                            detectedTime = 0;
                            return true;
                        }
                    }
                    //if any of the previous checks failed, reset the timer
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
            else
            {
                detectedTime = 0;
            }
        }
        else
        {
            detectedTime = 0;
        }
        return false;
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the cameras's position
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, distanceCameraCanRegisterAgent);
    }
}
