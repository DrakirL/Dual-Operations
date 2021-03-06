﻿using StateStuff;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SecondState : State<AI>
{
    private static SecondState _instance;
    public float flashTimer = 1;
    Quaternion lookRotation;
    Vector3 direction;

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

    IEnumerator lol(AI _owner)
    {
        yield return new WaitForSeconds(3);
        if(_owner.dead == false)
        {
            Debug.Log("fyfan vad död du är sopa lol xd");
            AlertMeter._instance.AddAlert(100);
        }

    }

    public override void EnterState(AI _owner)
    {
        _owner.changeAnimation("GUN DRAW");
        _owner.StartCoroutine(lol(_owner));
        Debug.Log("Enter 2");

        _owner.gameObject.GetComponent<NavMeshAgent>().isStopped = true;

        //Filip 
        var obj = _owner.gameObject.GetComponentInChildren<ChangeAlertnessTexture>();
        obj.setTexture = ChangeAlertnessTexture.SetTexture.Alerted;

        AlertMeter._instance.AddAlert(10);
       // _owner.warning.Play(true);
        AlertMeter._instance.PlayAlertFlash(flashTimer);
        //_owner.gameObject.GetComponent<NavMeshAgent>().destination = _owner.goal2.position;
    }

    public override void ExitState(AI _owner)
    {
        Debug.Log("Exit 2");

        //_owner.gameObject.GetComponent<NavMeshAgent>().isStopped = false;

        //Filip 
        var obj = _owner.gameObject.GetComponentInChildren<ChangeAlertnessTexture>();
        obj.setTexture = ChangeAlertnessTexture.SetTexture.Unsuspected;

      //  _owner.warning.Stop();
    }
    public override void movePos(AI _owner)
    {
        NavMeshAgent agent = _owner.gameObject.GetComponent<NavMeshAgent>();
        agent.destination = _owner.goal[_owner.destPoint].position;
        _owner.destPoint = (_owner.destPoint + 1) % _owner.goal.Length;
    }

    public override void UpdateState(AI _owner)
    {
      //  if (!_owner.gameObject.GetComponent<NavMeshAgent>().pathPending && _owner.gameObject.GetComponent<NavMeshAgent>().remainingDistance < 0.5f)
       // {
   //         _owner.stateMachine.ChangeState(FirstState.Instance);
        //}

        if (_owner.dead)
        {
            Debug.Log("WORK pLZ");
            _owner.stateMachine.ChangeState(ThirdState.Instance);
        }

        test222(_owner);

        direction = (_owner.player.transform.position - _owner.transform.position).normalized;
        lookRotation = Quaternion.LookRotation(direction);
        _owner.transform.rotation = Quaternion.Slerp(_owner.transform.rotation, lookRotation, Time.deltaTime * _owner.rotationSpeed);
        

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
    }

    public override void OnDrawGizmos(AI _owner)
    {

        // MÅLAR RAYCASTEN, BRA FÖR BUGGTEST I TEORIN FÖR DET FUNKAR FAN INTE 

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
