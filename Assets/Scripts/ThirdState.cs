using StateStuff;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThirdState : State<AI>
{
    private static ThirdState _instance;

    IEnumerator waiter(AI _owner)
    {
        yield return new WaitForSeconds(10);
        _owner.dead = false;
        _owner.stateMachine.ChangeState(FirstState.Instance);
    }

    private ThirdState()
    {
        if (_instance != null)
        {

            return;
        }

        _instance = this;
    }


    public static ThirdState Instance
    {
        get
        {
            if (_instance == null)
            {
                new ThirdState();
            }
            return _instance;
        }
    }

    public override void EnterState(AI _owner)
    {
        _owner.changeAnimation("STUNNED");
        _owner.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
        Debug.Log("Enter 3");
        _owner.StartCoroutine(waiter(_owner));
    }

    public override void ExitState(AI _owner)
    {
        _owner.gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        Debug.Log("Exit 3");

    }

    public override void movePos(AI _owner)
    {

    }

    public override void UpdateState(AI _owner)
    {

    }

    public override void test222(AI _owner)
    {

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
