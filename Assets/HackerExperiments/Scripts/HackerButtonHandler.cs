using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Mirror;

[System.Serializable]
public struct HackOptions
{
    public string optionText;
    public UnityEvent optionFunctions;
}
public class HackerButtonHandler : NetworkBehaviour
{
    [Tooltip("this is used to connect all buttons to this script")]
    [SerializeField] HackerButton[] allButtons;
     HackerButton[] allCameras;
     HackerButton[] allRadios;
    [Tooltip("these gameobjects NEEDS a image component")]
    [SerializeField] GameObject optionSprite;
    [SerializeField] GameObject optionFrameSprite;
    [Tooltip("this is alterd to change the distance betweent he various options sprites")]
    [SerializeField] float distance;
    [SerializeField] float yOffset;
    [SerializeField] float xOffset;
    public RawImage cameraImage;
    [SerializeField] HackerScript hackerScript;
    HackOptions currentOption;
    bool isMenuUp = false;
    bool isCameraup = false;

    // Start is called before the first frame update
    void Start()
    {
        int amountOfCameras = 0;
        int amountOfRadios = 0;

        cameraImage.enabled = false;
        for (int i = 0; i < allButtons.Length; i++)
        {
            if (allButtons[i].hackableType == HackerButton.HackableType.camera)
            {
                amountOfCameras++;
            }
            else
            {
                amountOfRadios++;
            }
            allButtons[i].HBH = this;
            allButtons[i].hackerS = hackerScript;
        }
        allCameras = new HackerButton[amountOfCameras];
        allRadios = new HackerButton[amountOfRadios];

        for (int i = 0; i < allButtons.Length; i++)
        {
            if (allButtons[i].hackableType == HackerButton.HackableType.camera)
            {
                allCameras[allButtons[i].hackableNumber] = allButtons[i];
            }
            else
            {
                allRadios[allButtons[i].hackableNumber] = allButtons[i];
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            takeDownOptions();
        }
        if(isCameraup)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                takeDownCamera();
                CameraManager.Instance.afterHackerIsDone();
            }
        }
    }
    public void cameraBackOnline(int index)
    {
        allCameras[index].changeTextureHacked();
    }

    public void RadioBackOnline(int index)
    {
        allRadios[index].changeTextureHacked();
    }

    public void setUpCameraWatch(int hackableNumber)
    {
        if (CameraManager.Instance.isCameraAvailable(hackableNumber))
        {
            isCameraup = true;
            cameraImage.enabled = true;
            cameraImage.texture = CameraManager.Instance.updateHackerCameraView(hackableNumber);
        }
        else
        {
            //play error sound?
        }
    }
    private void takeDownCamera()
    {
        cameraImage.enabled = false;
        isCameraup = false;
    }
    public void turnOnRadio(int hackableNumber)
    {
        CameraManager.Instance.turnOnRadio(hackableNumber);
    }
    
    //functions to create and remove the menues
    List<GameObject> tempHolder = new List<GameObject>();
    public void setUpOptions(HackOptions[] options)
    {
        if (isMenuUp)
        {
            takeDownOptions();
        }
        else
        {
            if (!isCameraup)
            {
                isMenuUp = true;
                //add the frame
                GameObject frame = Instantiate(optionFrameSprite);
                frame.transform.parent = transform;
                frame.transform.position =
                    new Vector2(
                        Input.mousePosition.x + xOffset,
                        Input.mousePosition.y);
                tempHolder.Add(frame);

                for (int i = 0; i < options.Length; i++)
                {
                    GameObject option = Instantiate(optionSprite);
                    option.transform.parent = transform;
                    option.transform.position =
                        new Vector2(
                            Input.mousePosition.x + xOffset,
                            Input.mousePosition.y - distance * i + yOffset);
                    tempHolder.Add(option);

                    ButtonIdentifier Button = option.GetComponent<ButtonIdentifier>();
                    Button.HBH = this;
                    Button.UE = options[i].optionFunctions;
                    Button.transform.GetComponentInChildren<Text>().text = options[i].optionText;
                }
            }
        }
    }
    public void takeDownOptions()
    {
        foreach(GameObject go in tempHolder)
        {
            Destroy(go);
        }
        tempHolder.Clear();
        isMenuUp = false;
    }
}
