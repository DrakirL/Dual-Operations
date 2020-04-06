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
    
    [SerializeField] float cameraAlertTime;
    [SerializeField] GameObject Spy;
    Collider spyCol;

    [SerializeField] float shutDownTimer = 10;
    [SerializeField] RawImage cameraRenderer;


    [SerializeField] int temp;

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
        alertTimes = camerasThatSeeTheSpy();
        if (alertTimes.Count == 0)
        {
         //   Debug.Log("no camera has spotted the agent");
        }
        else
        {
            for (int i = 0; i < alertTimes.Count; i++)
            {
                //TYPE HERE WHAT SHOULD HAPPEN WHEN CAMERA DETECT AGENT
                Debug.Log(alertTimes[i].gameObject.name + " has spoted the agent!");
            }
        }
    }

    //function to detect all cameras that can see the player
    List<CameraScript> camerasThatSeeTheSpy()
    {
        List<CameraScript> tempList = new List<CameraScript>();
        //whith the function from the CameraScript, collects all cameras that can see the agent
        for (int i = 0; i < cameraStruct.Length; i++)
        {
            if (cameraStruct[i].camera.isObjectVisible(Spy, spyCol, cameraAlertTime))
                tempList.Add(cameraStruct[i].camera);
        }
        return tempList;
    }

    //hacker funktioanlaties
    RenderTexture updateHackerCameraView(int index)
    {
        //use this function by typing something like this
        //cameraRenderer.texture = updateHackerCameraView(temp);
        return cameraStruct[index].cameraView;
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
