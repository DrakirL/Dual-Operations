using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class GetPlayer : NetworkBehaviour
{
    private static GetPlayer instance;
    public static GetPlayer Instance { get { return instance; } }
    public GameObject canvas;
    public Text canvasText;
    public GameObject camera;
    // Use this for initialization
    void Start()
    {
        camera.SetActive(false);
        if (isLocalPlayer)
            camera.SetActive(true);
        transform.position = spawnTransform.Instance.transform.position;
        transform.rotation = spawnTransform.Instance.transform.rotation;
        if(isLocalPlayer)
        {
            try
            {
                canvas = transform.Find("canvas").gameObject;
                //canvasText = canvas.transform.FindChild("tutorial").GetComponent<Text>();
                canvas.active = true;
            }
            catch
            {
                Debug.LogError("Agent don't seems to have a canvas with the name canvas, that the name is canvas is crucial!");
            }
        }

        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        //this works for server but not for client'
        if(Input.GetKeyDown(KeyCode.G))
        {
            AlertMeter._instance.AddAlert(1);
        }
    
    }
    /*public void showCanvas()
    {
        CmdShowCanvas();
    }
    [Command]
    public void CmdShowCanvas()
    {
        RpcCanvas();
    }
    [ClientRpc]
    private void RpcCanvas()
    {
        GameObject ca = AlertMeter._instance.gameObject;
        ca.active = false;
        ca.active = true;
    }*/


    public GameObject getPlayer()
    {
        return this.gameObject;
    }
    public void incAlertFromCamera(float value)
    {
        if (isClientOnly)
        {
            AlertMeter._instance.AddAlert(value);
            AlertMeter._instance.PlayAlertFlash(1f);
        }
    }
    public void addAlertServer(float value)
    {
        CmdAddAlertOnServer(value);
    }
    public void addAlertServer(float value,float flashLength)
    {
        addAlertServer(value);
        AlertMeter._instance.PlayAlertFlash(flashLength);
    }
    [Command]
    private void CmdAddAlertOnServer(float value)
    {
        AlertMeter._instance.alertValue = Mathf.Clamp(AlertMeter._instance.alertValue + value, 0, 100);
        AlertMeter._instance.timeStamp = Time.time;
        AlertMeter._instance.tmpCounter = 0;
        //RpcAddAlertOnClient(AlertMeter._instance.alertValue, AlertMeter._instance.timeStamp);
    }
   // [ClientRpc]
    //private void RpcAddAlertOnClient(float newAlerValue, float newTimeStamp)
    //{
     //   AlertMeter._instance.alertValue = newAlerValue;// Mathf.Clamp(AlertMeter._instance.alertValue + value, 0, 100);
      //  AlertMeter._instance.timeStamp = newTimeStamp;// Time.time;
    //}

    

    //door open functions
    public void openDoorServer(string name)
    {
        CmdOpendorOnServer(name);
    }
    [Command]
    private void CmdOpendorOnServer(string name)
    {
        RpcPlayOpenAnimation(name);
    }
    [ClientRpc]
    private void RpcPlayOpenAnimation(string name)
    {
        GameObject.Find(name).GetComponent<SlideDoor>().RpcPlayOpenAnimation();
    }

    public void addCanvasText(string text)
    {
        canvas.transform.Find("tutorial").GetComponent<Text>().text = text;
        //canvasText.text = text;
    }
    public void removeCanvasText()
    {
        canvas.transform.Find("tutorial").GetComponent<Text>().text = "";
        //canvasText.text = "";
    }
    // Generator functions
    public void ActivateGeneratorItemsServer(int genNum, bool boolToSet)
    {
        CmdActivateGeneratorItems(genNum, boolToSet);
    }
    [Command]
    public void CmdActivateGeneratorItems(int genNum, bool boolToSet)
    {
        RpcActivateGeneratorItems(genNum, boolToSet);
    }
    [ClientRpc]
    void RpcActivateGeneratorItems(int genNum, bool boolToSet)
    {
        for (int i = 0; i < GeneratorItems.Instance.generators[genNum].generatorObjects.Length; i++)
        {
            GeneratorItems.Instance.generators[genNum].generatorObjects[i].SetActive(boolToSet);
        }
    }
    public void LoadScene(string sceneName, float time)
    {
        CmdLoadScene(sceneName, time);
    }
    [Command]
    void CmdLoadScene(string scene, float time)
    {
        RpcLoadScene(scene, time);
    }
    [ClientRpc]
    void RpcLoadScene(string scene, float time)
    {
        GameManager._instance.LoadScene(scene, time);
    }
}
