﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class taser : NetworkBehaviour
{

    public Camera aimCam;
    public float range;
    public bool bengt = false;
    public Text tasorText;

    public int tasorSkott = 5;
    //private float reloadTime = 2f;
    public bool tasorReady = true;
    [SerializeField] AgentControllerScript agent;
    [SerializeField] AnimationClip Shoot;
    [SerializeField] AnimationClip Reload;

    // Start is called before the first frame update
    void Start()
    {
        //this is the time of the animation
       // reloadTime = 2.633f;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isServer)
        {
            if (Input.GetKeyDown(agent.useTaser) && tasorSkott > 0 && tasorReady == true)
            {
                Debug.Log("fire");
                CmdShoot();
                //#bästadesign som fixat tasorn <3
                tasorSkott--;
                tasorReady = false;

             
                if (tasorSkott >= 1)
                {
                    agent.changeFPanimationState("SPY_SHOOT");
                    agent.changeAnimationStateState("SHOOT");
                    StartCoroutine(Cooldown(Shoot.length + Reload.length));
                    
                }
                else
                {
                    agent.animationFPSHandler.anim.SetBool("hasAmmo", false);
                    CmdreloadNoMore();
                    agent.changeFPanimationState("SPY_SHOOT");
                    agent.changeAnimationStateState("SHOOT");
                    StartCoroutine(Cooldown(Shoot.length));
                }
               
               

            }
        }



        if (bengt)
        {
            CmdShoot();
        }

        tasorText.text = tasorSkott.ToString();
    }

    [Command]
    void CmdreloadNoMore()
    {
        agent.animationFPSHandler.anim.SetBool("hasAmmo", false);
        agent.animationFBHandler.anim.SetBool("hasAmmo", false);
    }
    private IEnumerator Cooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
       
        tasorReady = true;
    }
   

    [Command]
    public void CmdShoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(aimCam.transform.position, aimCam.transform.forward, out hit, range))
        {
            AI target = hit.transform.GetComponent<AI>();
            Debug.Log(target);
            if (target != null)
            {
                target.dead = true;
            }
        }
    }
}
