using UnityEngine;
using Mirror;

public class CameraScript : NetworkBehaviour
{
    Camera thisCamera;
    Plane[] planes;
    [Tooltip("a varible determain how close the agent needs to be for the camera to pick it up, also this area is rendered with a gizmo")]
    public float distanceCameraCanRegisterAgent;
    /*[HideInInspector]*/ public bool cameraActive;
    [HideInInspector] public float detectedTime = 0;

    [SerializeField] bool cameraIsMoving = true;
    [HideInInspector] public enum CameraAState
    {
        AgentCloseEnough,
        HackerUsesTheCamera,
        Disabled,
        Neither
    }
    /*[HideInInspector]*/ public CameraAState cameraState = CameraAState.Neither;
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
         planes = GeometryUtility.CalculateFrustumPlanes(thisCamera);
        
    }
    private void Update()
    {
        if (cameraState == CameraAState.Neither)
        {
            thisCamera.enabled = false;
        }
        else
        {
            if (cameraState == CameraAState.Disabled)
            {
                thisCamera.enabled = false;
            }
            else
            {
                thisCamera.enabled = true;
            }
        }
    }
    public bool isObjectVisible(GameObject objectToFind, Collider objectCollider, float timeBeforeDetected)
    {
            if (cameraState == CameraAState.AgentCloseEnough)
            {
                //checks if the camera is activ (not hacked)
                if (cameraActive)
                {
                if(Vector3.Distance(objectToFind.transform.position, transform.position) <= distanceCameraCanRegisterAgent)
                {
                    if(!Physics.Linecast(objectToFind.transform.position, transform.position))
                    {
                        if (cameraIsMoving)
                        {
                            planes = GeometryUtility.CalculateFrustumPlanes(thisCamera);
                        }
                        if (GeometryUtility.TestPlanesAABB(planes, objectCollider.bounds))
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
