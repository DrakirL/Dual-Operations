using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class radioInterract : NetworkBehaviour
{
    Transform xd123;
    public bool on;
    public int index;
    public GameObject[] assignedGuards;
    // Start is called before the first frame update
    void Start()
    {
        on = false;
        xd123 = GetComponent<Transform>();
    }
    
    IEnumerator waiter()
    {
        yield return new WaitForSeconds(10);
        on = false;

        //Sound pls
        if (isServer)
            DualOperationsAudioPlayer.Instance.RpcUpdateRadio(0.0f, index);

        GetPlayer.Instance.radioNoLongerInUse(index);
        // CameraManager.Instance.hacker.CmdRadioIsNoTurnedOff(index);
      
    }

        void checkPlayer()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position.x > (xd123.position.x - 2) && GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position.x < (xd123.position.x + 2))
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position.z > (xd123.position.z - 2) && GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position.z < (xd123.position.z + 2))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    on = true;
                }
            }
        }
    }

    void checkGuard()
    {/*
        if (GameObject.FindGameObjectWithTag("Guard").GetComponent<Transform>().position.x >= (xd123.position.x - 2) && GameObject.FindGameObjectWithTag("Guard").GetComponent<Transform>().position.x <= (xd123.position.x + 2))
        {
            if (GameObject.FindGameObjectWithTag("Guard").GetComponent<Transform>().position.z >= (xd123.position.z - 2) && GameObject.FindGameObjectWithTag("Guard").GetComponent<Transform>().position.z <= (xd123.position.z + 2))
            {
                Debug.Log("xd");
                if (on == true)
                {
                    StartCoroutine(waiter());
                }
            }
        }
        */
        if (on)
        {
            for (int i = 0; i < assignedGuards.Length; i++)
            {
                if (assignedGuards[i].transform.position.x >= (xd123.position.x - 2) && assignedGuards[i].transform.position.x <= (xd123.position.x + 2))
                {
                    if (assignedGuards[i].transform.position.z >= (xd123.position.z - 2) && assignedGuards[i].transform.position.z <= (xd123.position.z + 2))
                    {
                        StartCoroutine(waiter());
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
       // checkPlayer();
        checkGuard();
    }
}
