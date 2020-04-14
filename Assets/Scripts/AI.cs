using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateStuff;

public class AI : MonoBehaviour
{
    //   public bool switchState = false;
    public Transform [] goal;
    public StateMachine<AI> stateMachine { get; set; }
    public int destPoint = 0;
    public GameObject radio;
    public bool radioTurnOff;

    private void Start()
    {

        stateMachine = new StateMachine<AI>(this);
        stateMachine.ChangeState(FirstState.Instance);
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        radioTurnOff = radio.GetComponent<radioInterract>().on;
        stateMachine.Update();
    }
}
