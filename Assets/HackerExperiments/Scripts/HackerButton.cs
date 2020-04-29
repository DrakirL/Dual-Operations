using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

public class HackerButton : NetworkBehaviour
{
   [HideInInspector] public HackerButtonHandler HBH;
    [SerializeField] HackOptions[] preHackOptions;
    [SerializeField] HackOptions[] postHackOptions;
    [SerializeField] int hackableNumber;
    [SerializeField] bool isHacked = false;
   [HideInInspector] public HackerScript hackerS;
    public GameObject minigame;

    [SerializeField] bool isHoverOverThisButton = false;
    
    void Update()
    {
        
        if (Input.GetMouseButtonDown(1) && isHoverOverThisButton)
        {
            if (isHacked)
            {
                HBH.setUpOptions(postHackOptions);
            }
            else
            {
                HBH.setUpOptions(preHackOptions);
            }
        }
    }
    public void OnPointerEnter()
    {
        isHoverOverThisButton = true;
    }

    public void OnPointerExit()
    {
        isHoverOverThisButton = false;
    }
    public void hack()
    {
        isHacked = true;
    }
    public void getCamera()
    {
        HBH.setUpCameraWatch(hackableNumber);
    }
    public void turnOnRadio()
    {
        HBH.turnOnRadio(hackableNumber);
    }
    
   // [ClientRpc]
    public void RpcShutDownCamera()
    {
        hackerS.RpcShutDownCamera(hackableNumber);
        //CameraManager.Instance.shutDownCamera(hackableNumber);
    }
    public void LoadMinigame()
    {
        MinigameManager.Instance.Activate(this, minigame);
    }
}
