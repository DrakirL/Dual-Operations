using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateStuff;
using Mirror;

public class AI : NetworkBehaviour
{
    //   public bool switchState = false;
    public Transform[] goal;
    public StateMachine<AI> stateMachine { get; set; }
    public int destPoint = 0;
    public GameObject radio;
    public bool radioTurnOff;
    public bool fovtest123 = false;
    public bool radiustest123 = false;
    public float maxAngle = 50;
    public float maxRadius = 5;
    public float maxRadius2 = 8;
    public float rotationSpeed = 2f;
    [HideInInspector]
    public Transform player;
    [HideInInspector]
    public GameObject detectCount;
    [HideInInspector]
    public ParticleSystem warning;
    [SyncVar] public bool dead = false;
    [SerializeField] AnimationHandler animationH;
    [HideInInspector]
    public Quaternion rotateSave;
   


    private void Start()
    {
        stateMachine = new StateMachine<AI>(this);
        stateMachine.ChangeState(FirstState.Instance);
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        warning = GetComponent<ParticleSystem>();
        rotateSave = transform.rotation;

    }
    public void changeAnimation(string nameOfAnimation)
    {
        if(isServer)
        {
            RpcChangeGuardAnimation(nameOfAnimation);
        }
    }

    [ClientRpc]
    public void RpcChangeGuardAnimation(string newAnimation)
    {
        if (animationH != null)
        {
            animationH.changeAnimation(newAnimation);
        }
    }
    // IEnumerator routine = AI.guardMoivingState();
    public void startMoveCheck()
    {
        StartCoroutine(guardMoivingState());
    }
    public IEnumerator guardMoivingState()
    {
        Vector2 guardPos = new Vector2(transform.position.x, transform.position.z);
        yield return new WaitForSeconds(0.2f);

        if (stateMachine.currentState == FirstState.Instance)
        {
            if (guardPos == new Vector2(transform.position.x, transform.position.z))
            {
                changeAnimation("IDLE");
            }
            else
            {
                changeAnimation("WALK");
            }

            StartCoroutine(guardMoivingState());
        }
    }

    private void Update()
    {
        if (isServer)// NetworkServer.localConnection.connectionId == 0)
        {
            try
            {
                player = GetPlayer.Instance.getPlayer().transform;
            }
            catch
            {
                goto Done;
            }
            radioTurnOff = radio.GetComponent<radioInterract>().on;

            stateMachine.Update();

            Done:
            return;
        }
    }
}
