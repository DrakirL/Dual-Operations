using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class SlideDoor : NetworkBehaviour, IInteractable
{
    [Header("These fields are not required and can be empty")]
    public Animator anim1;
    public Animator anim2;
    public GameObject door;
    public new GameObject light;
    public Sprite lightSpriteOn;
    public Sprite lightSpriteOff;
    [SerializeField] int keyCode;
    [Tooltip("if agent tries to open a door he/she don't have access to, this amount of alertness will  be added")]
    [SerializeField] float alertInc = 20;
    [SerializeField] float alertFlashTime = 20;

    BoxCollider col;
    public bool active = true;

    private void Start()
    {
        
        col = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if(active)
            col.enabled = DoorIsClosed();

        if(light != null)
        {
            if (DoorIsClosed())
                light.GetComponent<SpriteRenderer>().sprite = lightSpriteOff;
            else
                light.GetComponent<SpriteRenderer>().sprite = lightSpriteOn;
        }      
    }

    public void GetInteracted(List<int> io)
    {
        if (active)
        {
            if (io.Contains(keyCode))
            {
                GetPlayer.Instance.openDoorServer(gameObject.name);
            }
            else
            {
                AlertMeter._instance.AddAlert(alertInc);
                AlertMeter._instance.PlayAlertFlash(alertFlashTime);
            }
        }
        else
        {
            AlertMeter._instance.AddAlert(alertInc);
            AlertMeter._instance.PlayAlertFlash(alertFlashTime);
        }
    }
    
    public void RpcPlayOpenAnimation()
    {
        if (DoorIsClosed())
        {
            if (anim1 != null)
                anim1.Play("Open");
            if (anim2 != null)
                anim2.Play("Open");
        }
    }

    bool DoorIsClosed() => (anim1.GetCurrentAnimatorStateInfo(0).IsName("CloseIdle")) ? true : false;
}
