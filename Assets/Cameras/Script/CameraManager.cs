using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

[System.Serializable]
public struct CameraStruct
{
    public CameraScript camera;
    public RenderTexture cameraView;
    public bool AgentIsInCamera;
}

[System.Serializable]
public struct RadioStruct
{
    public radioInterract radio;
}

public class CameraManager : NetworkBehaviour
{
    private static CameraManager instance;
    public static CameraManager Instance { get { return instance; } }
    // Use this for initialization
    [SerializeField] CameraStruct[] cameraStruct;
    [SerializeField] RadioStruct[] radioStruct;
    [SerializeField] float flashTimer = 1;

    [SerializeField] float cameraAlertTime;
    GameObject Spy;
    Collider spyCol;

    [SerializeField] float shutDownTimer = 10;

    [Tooltip("this is the variable that defines how much the alert state increase every cameraAlertTime-seconds")]
    [SerializeField] float alertStateInc;

    float maxDistance = 0;

    

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        for(int i = 0; i < cameraStruct.Length; i++)
        {
            if (cameraStruct[i].camera.distanceCameraCanRegisterAgent > maxDistance)
                maxDistance = cameraStruct[i].camera.distanceCameraCanRegisterAgent;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isClientOnly)
        {
            if (Spy == null)
            {
                try
                {
                    Spy = GetPlayer.Instance.getPlayer();
                    spyCol = Spy.GetComponent<Collider>();
                }
                catch
                {
                    Debug.LogWarning("if this appears more than two times something is wrong");
                }
            }
            else
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
                        AlertMeter._instance.AddAlert(alertStateInc);
                        AlertMeter._instance.PlayAlertFlash(flashTimer);

                        //GetPlayer.Instance.incAlertFromCamera(alertStateInc);
                    }
                }
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
            if (maxDistance >= (Vector3.Distance(cameraStruct[i].camera.gameObject.transform.position, GetPlayer.Instance.getPlayer().transform.position)))
            {
                if(cameraStruct[i].camera.cameraState == CameraScript.CameraAState.Neither)
                { cameraStruct[i].camera.cameraState = CameraScript.CameraAState.AgentCloseEnough; }

                if (cameraStruct[i].camera.cameraState == CameraScript.CameraAState.AgentCloseEnough)
                {
                    if (cameraStruct[i].camera.isObjectVisible(Spy, spyCol, cameraAlertTime))
                        tempList.Add(cameraStruct[i].camera);
                }
            }
            else
            {
                if (cameraStruct[i].camera.cameraState == CameraScript.CameraAState.AgentCloseEnough)
                { cameraStruct[i].camera.cameraState = CameraScript.CameraAState.Neither; }
            }
        }
        return tempList;
    }
    public bool isCameraAvailable(int index)
    {
        return cameraStruct[index].camera.cameraActive;
    }
    //hacker funktioanlaties
    public void turnOnRadio(int index)
    {
        if (!radioStruct[index].radio.on)
        {
            radioStruct[index].radio.on = true;
            radioStruct[index].radio.index = index;
        }
    }
    public RenderTexture updateHackerCameraView(int index)
    {
        //use this function by typing something like this
        cameraStruct[index].camera.cameraState = CameraScript.CameraAState.HackerUsesTheCamera;
        return cameraStruct[index].cameraView;      
    }

    public void shutDownCamera(int index)
    {
        if (cameraStruct[index].camera.cameraActive)
        {
            cameraStruct[index].camera.cameraState = CameraScript.CameraAState.Disabled;
            cameraStruct[index].camera.cameraActive = false;
            StartCoroutine(acivateCamera(cameraStruct[index], shutDownTimer, index));
            cameraStruct[index].camera.lightSource.SetActive(false);
            GetPlayer.Instance.CmdCameraGoneOffline(index);
        }
    }
    private IEnumerator acivateCamera(CameraStruct cameraStruct, float waitTime,int index)
    {
        yield return new WaitForSeconds(waitTime);
        cameraStruct.camera.cameraActive = true;
        cameraStruct.camera.lightSource.SetActive(true);
        afterShutdown(cameraStruct);
        GetPlayer.Instance.CmdCameraBackOnline(index);
    }
    private void afterShutdown(CameraStruct cameraStruct)
    {
        if (maxDistance >= (Vector3.Distance(cameraStruct.camera.gameObject.transform.position, GetPlayer.Instance.getPlayer().transform.position)))
        {
            cameraStruct.camera.cameraState = CameraScript.CameraAState.AgentCloseEnough;
        }
        else
        {
            cameraStruct.camera.cameraState = CameraScript.CameraAState.Neither;
        }
        
    }
    public void afterHackerIsDone()
    {
        for (int i = 0; i < cameraStruct.Length; i++)
        {
            if (cameraStruct[i].camera.cameraState == CameraScript.CameraAState.HackerUsesTheCamera)
            {
                if (maxDistance >= (Vector3.Distance(cameraStruct[i].camera.gameObject.transform.position, GetPlayer.Instance.getPlayer().transform.position)))
                {

                    cameraStruct[i].camera.cameraState = CameraScript.CameraAState.AgentCloseEnough;
                }

                else
                {
                    cameraStruct[i].camera.cameraState = CameraScript.CameraAState.Neither;
                }
            }
        }
    }
}
