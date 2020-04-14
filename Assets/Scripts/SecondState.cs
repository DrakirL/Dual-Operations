using StateStuff;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SecondState : State<AI>
{
    private static SecondState _instance;
    
    
    private SecondState()
    {
        if (_instance != null)
        {
            
            return;
        }

        _instance = this;
    }

    public static SecondState Instance
    {
        get
        {
            if (_instance == null)
            {
                new SecondState();
            }
            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Enter 2");
//        _owner.gameObject.GetComponent<NavMeshAgent>().destination = _owner.goal2.position;
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exit 2");
    }

    public override void movePos(AI _owner)
    {
        NavMeshAgent agent = _owner.gameObject.GetComponent<NavMeshAgent>();
        agent.destination = _owner.goal[_owner.destPoint].position;
        _owner.destPoint = (_owner.destPoint + 1) % _owner.goal.Length;
    }

    public override void UpdateState(AI _owner)
    {
        if (!_owner.gameObject.GetComponent<NavMeshAgent>().pathPending && _owner.gameObject.GetComponent<NavMeshAgent>().remainingDistance < 0.5f)
        {
            _owner.stateMachine.ChangeState(FirstState.Instance);
        }
    }
}
