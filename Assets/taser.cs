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
    //private float reloadTime = 2f;
    public bool tasorReady = true;
    [SerializeField] AgentControllerScript agent;
    [SerializeField] AnimationClip Shoot;
    [SerializeField] AnimationClip Reload;
    //[SerializeField] Transform lightStartPos;
    [SerializeField] GameObject laserDisplay;
    [Range(0,2)]
    [SerializeField] float laserDisplayTime = 0.3f;
    GameObject laserParent;

    // Start is called before the first frame update
    void Start()
    {
        laserParent = laserDisplay.transform.parent.gameObject;
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
             
                CmdShoot();
                //#bästadesign som fixat tasorn <3
                tasorSkott--;
                tasorReady = false;

                // Sound pls
                DualOperationsAudioPlayer.Instance.Tasor();

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
                    StartCoroutine(Cooldown(Shoot.length + Reload.length));
                    //StartCoroutine(shootNoMoreAmmoAfter(0.633f));
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
        /*if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
        {
            agent.changeFPanimationState("SPY_WALK");
            agent.changeAnimationStateState("WALK");
        }
        else
        {
            agent.changeFPanimationState("SPY_IDLE");
            agent.changeAnimationStateState("IDLE");
        }*/
        tasorReady = true;
    }
    /*private IEnumerator Cooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
        {
            agent.changeFPanimationState("SPY_WALK");
            agent.changeAnimationStateState("WALK");
        }
        else
        {
            agent.changeFPanimationState("SPY_IDLE");
            agent.changeAnimationStateState("IDLE");
        }
        tasorReady = true;
    }
    private IEnumerator shootNoMoreAmmoAfter(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
        {
            agent.changeFPanimationState("SPY_WALK");
            agent.changeAnimationStateState("WALK");
        }
        else
        {
            agent.changeFPanimationState("SPY_IDLE");
            agent.changeAnimationStateState("IDLE");
        }
    }*/

    [Command]
    public void CmdShoot()
    {
        //RpcDispalyBeam(aimCam.transform.forward.normalized * range);
        RaycastHit hit;
        if (Physics.Raycast(aimCam.transform.position, aimCam.transform.forward, out hit, range))
        {
            RpcDispalyBeam(hit.point);
            AI target = hit.transform.GetComponent<AI>();
            Debug.Log(target);
            if (target != null)
            {
                target.dead = true;
            }
        }
    }

   //GameObject spawnedBeam;
    [ClientRpc]
    private void RpcDispalyBeam(Vector3 point)
    {
        laserDisplay.SetActive(true);
     //   GameObject spawnedBeam = Instantiate(laserDisplay, lightStartPos);
        LineRenderer LR = laserDisplay.transform.GetChild(0).gameObject.GetComponent<LineRenderer>();
        //LR.SetPosition(0, lightStartPos.position);
        LR.SetPosition(1, point);
        StartCoroutine(destroyBeam());
        laserDisplay.transform.parent = null;

    }
    IEnumerator destroyBeam()
    {
        yield return new WaitForSeconds(laserDisplayTime);
        laserDisplay.SetActive(false);
        laserDisplay.transform.parent = laserParent.transform;
        laserDisplay.transform.localPosition = Vector3.zero;
        //  Destroy(spawnedBeam);
    }
}
