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
     //   _owner.gameObject.GetComponent<NavMeshAgent>().destination = _owner.goal.position;
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exit 1");
    }

    public override void movePos(AI _owner)
    {
        NavMeshAgent agent = _owner.gameObject.GetComponent<NavMeshAgent>();
        agent.destination = _owner.goal[_owner.destPoint].position;
        _owner.destPoint = (_owner.destPoint + 1) % _owner.goal.Length;
    }

    public override void UpdateState(AI _owner)
    {

        if(!_owner.gameObject.GetComponent<NavMeshAgent>().pathPending && _owner.gameObject.GetComponent<NavMeshAgent>().remainingDistance < 0.5f && _owner.radioTurnOff == false)
        {   
            movePos(_owner);
        }
        
        if(_owner.radioTurnOff == true)
        {
            _owner.gameObject.GetComponent<NavMeshAgent>().destination = _owner.radio.transform.GetChild(0).position;
        }
    }
}
