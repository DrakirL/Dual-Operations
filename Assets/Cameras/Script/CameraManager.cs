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

[System.Serializable]
public struct RadioStruct
{
    public radioInterract radio;
}

public class CameraManager : MonoBehaviour
{
    private static CameraManager instance;
    public static CameraManager Instance { get { return instance; } }
    // Use this for initialization
    [SerializeField] CameraStruct[] cameraStruct;
    [SerializeField] RadioStruct[] radioStruct;

    [SerializeField] float cameraAlertTime;
    GameObject Spy;
    Collider spyCol;

    [SerializeField] float shutDownTimer = 10;

    [Tooltip("this is the variable that defines how much the alert state increase every cameraAlertTime-seconds")]
    [SerializeField] float alertStateInc;

    

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        try
        {
            Spy = GetPlayer.Instance.getPlayer();
            spyCol = Spy.GetComponent<Collider>();
        }
        catch
        {
            Debug.LogWarning("if this appears over three times something is wrong");
        }
        //shutDownCamera(cameraStruct[0]);

    }

    // Update is called once per frame
    void Update()
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
                Debug.LogWarning("if this appears over three times something is wrong");
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
                    //Debug.Log(alertTimes[i].gameObject.name + " has spoted the agent!");
                    AlertMeter._instance.AddAlert(alertStateInc/2);
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
            if (cameraStruct[i].camera.isObjectVisible(Spy, spyCol, cameraAlertTime))
                tempList.Add(cameraStruct[i].camera);
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
        }
    }
    public RenderTexture updateHackerCameraView(int index)
    {
        //use this function by typing something like this
        return cameraStruct[index].cameraView;
    }

    public void shutDownCamera(int index)
    {
        if (cameraStruct[index].camera.cameraActive)
        {
            cameraStruct[index].camera.cameraActive = false;
            StartCoroutine(acivateCamera(cameraStruct[index], shutDownTimer));
            cameraStruct[index].camera.lightSource.SetActive(false);
        }
    }
    private IEnumerator acivateCamera(CameraStruct cameraStruct, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        cameraStruct.camera.cameraActive = true;
        cameraStruct.camera.lightSource.SetActive(true);
    }
}
