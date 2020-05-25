using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;
using UnityEngine.UI;

public class HackerButton : NetworkBehaviour
{
   [HideInInspector] public HackerButtonHandler HBH;
    [SerializeField] HackOptions[] preHackOptions;
    [SerializeField] HackOptions[] postHackOptions;
    public int hackableNumber;
    [SerializeField] bool isHacked = false;
   [HideInInspector] public HackerScript hackerS;
    public enum HackableType
    {
        camera,
        radio
    }
  public HackableType hackableType;
    public GameObject minigame;

    [SerializeField] bool isHoverOverThisButton = false;
    [SerializeField] Sprite hackedTexture;
    [SerializeField] Sprite usingTexture;
    [Tooltip("leave this blank for radios")]
    [SerializeField] Sprite shutdownCameraTexture;

    Image image;
    private void Start()
    {
        image = gameObject.GetComponent<Image>();
    }
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
        image.sprite = hackedTexture;
    }
    public void changeTextureUsing()
    {
        image.sprite = usingTexture;
    }
    public void changeTextureHacked()
    {
        image.sprite = hackedTexture;
    }
    void changeTextureShutdown()
    {
        image.sprite = shutdownCameraTexture;
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
        changeTextureShutdown();
    }
    public void LoadMinigame()
    {
        MinigameManager.Instance.Activate(this, minigame);
    }
}
