using System.Collections;
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
    private float reloadTime = 2f;
    public bool tasorReady = true;
    [SerializeField] AnimationHandler firstPersonAnimation;
    [SerializeField] AgentControllerScript agent; 

    // Start is called before the first frame update
    void Start()
    {
        //this is the time of the animation
        reloadTime = 2.633f;
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
                StartCoroutine(Cooldown(reloadTime));
                
                agent.changeAnimationStateState("SPY_SHOOT");

            }
        }



        if (bengt)
        {
            CmdShoot();
        }

        tasorText.text = tasorSkott.ToString();
    }

    private IEnumerator Cooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
        {
            agent.changeAnimationStateState("SPY_WALK");
        }
        else
        {
            agent.changeAnimationStateState("SPY_IDLE");
        }
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
