using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HackerButton : MonoBehaviour
{
   [HideInInspector] public HackerButtonHandler HBH;
    [SerializeField] HackOptions[] preHackOptions;
    [SerializeField] HackOptions[] postHackOptions;
    [SerializeField] int hackableNumber;
    [SerializeField] bool isHacked = false;

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
    public void shutDownCamera()
    {
        CameraManager.Instance.shutDownCamera(hackableNumber);
    }
}
