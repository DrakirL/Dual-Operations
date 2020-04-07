using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StateStuff;

public class AI : MonoBehaviour
{
    //   public bool switchState = false;
    public Transform goal;
    public Transform goal2;
    public StateMachine<AI> stateMachine { get; set; }

    private void Start()
    {
        stateMachine = new StateMachine<AI>(this);
        stateMachine.ChangeState(FirstState.Instance);
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {

        stateMachine.Update();
    }
}
