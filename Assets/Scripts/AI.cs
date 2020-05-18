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
    public float maxAngle = 50;
    public float maxRadius = 5;
    [HideInInspector]
    public Transform player;
    [HideInInspector]
    public GameObject detectCount;
    [HideInInspector]
    public ParticleSystem warning;
    public bool dead = false;    


    private void Start()
    {
        stateMachine = new StateMachine<AI>(this);
        stateMachine.ChangeState(FirstState.Instance);
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        warning = GetComponent<ParticleSystem>();
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
