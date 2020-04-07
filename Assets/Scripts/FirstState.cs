using StateStuff;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FirstState : State<AI>
{
    private static FirstState _instance;
    Rigidbody lol;
    Transform lmao;
    


    private FirstState()
    {
        if (_instance != null) {
            
            return;
        }

        _instance = this;
    }

    public static FirstState Instance {
        get {
            if(_instance == null) {
                new FirstState();
                
            }
            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {
        Debug.Log("Enter 1");
        _owner.gameObject.GetComponent<NavMeshAgent>().destination = _owner.goal.position;
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exit 1");
    }

    public override void UpdateState(AI _owner)
    {
        if(_owner.gameObject.GetComponent<NavMeshAgent>().remainingDistance < 1)
        {
            _owner.stateMachine.ChangeState(SecondState.Instance);
        }

    }
}
