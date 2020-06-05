using StateStuff;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FirstState : State<AI>
{
    private static FirstState _instance;
    Quaternion lookRotation;
    Vector3 direction;

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

        _owner.startMoveCheck();
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
        Debug.Log(_owner.player.GetComponent<AgentControllerScript>().USE_SPEED);
        

        if (_owner.dead)
        {
            _owner.stateMachine.ChangeState(ThirdState.Instance);
        }
        test222(_owner);
        if (!_owner.gameObject.GetComponent<NavMeshAgent>().pathPending && _owner.gameObject.GetComponent<NavMeshAgent>().remainingDistance < 0.5f && _owner.radioTurnOff == false)
        {   
            movePos(_owner);
        }
        
        if(_owner.radioTurnOff == true)
        {
            _owner.gameObject.GetComponent<NavMeshAgent>().destination = _owner.radio.transform.GetChild(0).position;
        }

        if (_owner.player.GetComponent<AgentControllerScript>().isWalking)
        {
            if (_owner.player.GetComponent<AgentControllerScript>().USE_SPEED > 2)
            {
                _owner.maxRadius2 = 4;
            }
            else if (_owner.player.GetComponent<AgentControllerScript>().USE_SPEED > 1.25)
            {
                _owner.maxRadius2 = 3;
            }
            else
            {
                _owner.maxRadius2 = 2;
            }
        }
        else
        {
            _owner.maxRadius2 = 0;
        }

        if (_owner.radiustest123)
        {
            direction = (_owner.player.transform.position - _owner.transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation, lookRotation, Time.deltaTime * _owner.rotationSpeed);
            _owner.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
        }
        else { _owner.gameObject.GetComponent<NavMeshAgent>().isStopped = false; }

        if (_owner.fovtest123)
        {
            _owner.stateMachine.ChangeState(SecondState.Instance);
        }
        if (_owner.transform.rotation != _owner.rotateSave && _owner.GetComponent<NavMeshAgent>().velocity == new Vector3(0, 0, 0) && !_owner.radioTurnOff)
            _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation, _owner.rotateSave, Time.deltaTime * 0.5f);
    }

    public override void test222(AI _owner)
    {
        bool inFov(Transform checkingObject, Transform target, float maxAngle, float maxRadius)

       {
           Vector3 direction = (target.position - checkingObject.position).normalized;
           direction.y *= 0;

           RaycastHit hit;
           if (Physics.Raycast(checkingObject.position, (target.position - checkingObject.position).normalized, out hit, maxRadius))
           {
               if (hit.transform.gameObject.tag == "Player")
               {
                   float angle = Vector3.Angle(checkingObject.forward, direction);

                   if (angle <= maxAngle)
                   {
                       return true;
                   }
               }
           }
           return false;
       }

       _owner.fovtest123 = inFov(_owner.transform, _owner.player, _owner.maxAngle, _owner.maxRadius);

        bool inRadius(Transform checkingObject, Transform target, float maxRadius2)
        {
            Vector3 direction = (target.position - checkingObject.position).normalized;
            direction.y *= 0;

            RaycastHit hit;
            if (Physics.Raycast(checkingObject.position, (target.position - checkingObject.position).normalized, out hit, maxRadius2))
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    return true;
                }
            }
            return false;
        }
        _owner.radiustest123 = inRadius(_owner.transform, _owner.player, _owner.maxRadius2);
    }

    public override void OnDrawGizmos(AI _owner)
    {
        
       //  MÅLAR RAYCASTEN, BRA FÖR BUGGTEST I TEORIN FÖR DET FUNKAR FAN INTE
         
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_owner.transform.position, _owner.maxRadius);

        Vector3 fovLine1 = Quaternion.AngleAxis(_owner.maxAngle, _owner.transform.up) * _owner.transform.forward * _owner.maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-_owner.maxAngle, _owner.transform.up) * _owner.transform.forward * _owner.maxRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_owner.transform.position, fovLine1);
        Gizmos.DrawRay(_owner.transform.position, fovLine2);

        if (!_owner.fovtest123)
            Gizmos.color = Color.red;
        else
            Gizmos.color = Color.green;
        Gizmos.DrawRay(_owner.transform.position, (_owner.player.position - _owner.transform.position).normalized * _owner.maxRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(_owner.transform.position, _owner.transform.forward * _owner.maxRadius);
        
    }

    
}
