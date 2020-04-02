using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct CameraStruct
{
    public CameraScript camera;
    public RenderTexture cameraView;
    public bool AgentIsInCamera;
}

public class CameraManager : MonoBehaviour
{
    [SerializeField] CameraStruct[] cameraStruct;

    //OBS this is not ment to be public, remove when game is up
    [SerializeField] public float cameraFOVDistance;
    [SerializeField] float cameraAlertTime;
    [SerializeField] GameObject Spy;
    Collider spyCol;

    [SerializeField] float shutDownTimer = 10;
    [SerializeField] RawImage cameraRenderer;
    [SerializeField] int tempImageChanger = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        spyCol = Spy.GetComponent<Collider>();
        shutDownCamera(cameraStruct[0]);

    }

    // Update is called once per frame
    void Update()
    {
        List<CameraScript> alertTimes = new List<CameraScript>();
        alertTimes = camerasThatSeeTheSpy(cameraStruct);
        for (int i = 0; i < alertTimes.Count; i++)
        {
            //TYPE HERE WHAT SHOULD HAPPEN WHEN CAMERA DETECT AGENT
            //Debug.Log(alertTimes[i].gameObject.name);
        }
    }

    //functions that is currently not being used
    /* bool isAgentSeenByThisCamera(CameraStruct[] TracingCameras, int index)
     {
         return TracingCameras[index].camera.isObjectVisible(Spy, spyCol, cameraFOVDistance, cameraAlertTime);
     }
     bool isAgentSeenByThisCamera(CameraStruct TracingCamera)
     {
         return TracingCamera.camera.isObjectVisible(Spy, spyCol, cameraFOVDistance, cameraAlertTime);
     }

     bool isAgentSeenByAnyCamera(CameraStruct[] TracingCameras)
     {
         for (int i = 0; i < TracingCameras.Length; i++)
         {
             if (TracingCameras[i].camera.isObjectVisible(Spy, spyCol, cameraFOVDistance, cameraAlertTime))
                 return true;
         }
         return false;
     }*/
    //function to detect all cameras that can see the player
    List<CameraScript> camerasThatSeeTheSpy(CameraStruct[] TracingCameras)
    {
        List<CameraScript> tempList = new List<CameraScript>();
        for (int i = 0; i < TracingCameras.Length; i++)
        {
            if (TracingCameras[i].camera.isObjectVisible(Spy, spyCol, cameraFOVDistance, cameraAlertTime))
                tempList.Add(TracingCameras[i].camera);
        }
        return tempList;
    }





    //hacker funktioanlaties
    RenderTexture updateHackerCameraView(int index)
    {
        return cameraStruct[index].cameraView;
        // cameraRenderer.texture = cameraStruct[index].cameraView;
    }
    RenderTexture getCameraView(CameraStruct[] TracingCameras, int index)
    {
        return TracingCameras[index].camera.gameObject.GetComponent<Camera>().targetTexture;
    }

    public void shutDownCamera(CameraStruct cameraStruct)
    {
        if (cameraStruct.camera.cameraActive)
        {
            cameraStruct.camera.cameraActive = false;
            StartCoroutine(acivateCamera(cameraStruct, shutDownTimer));
        }
    }
    private IEnumerator acivateCamera(CameraStruct cameraStruct, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        cameraStruct.camera.cameraActive = true;
    }
}
