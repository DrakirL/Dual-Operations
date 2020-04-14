using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radioInterract : MonoBehaviour
{
    Transform xd123;
    public bool on;
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
    {
        if (GameObject.FindGameObjectWithTag("Guard").GetComponent<Transform>().position.x >= (xd123.position.x - 2) && GameObject.FindGameObjectWithTag("Guard").GetComponent<Transform>().position.x <= (xd123.position.x + 2))
        {
            if (GameObject.FindGameObjectWithTag("Guard").GetComponent<Transform>().position.z >= (xd123.position.z - 2) && GameObject.FindGameObjectWithTag("Guard").GetComponent<Transform>().position.z <= (xd123.position.z + 2))
            {
                if (on == true)
                {
                    StartCoroutine(waiter());
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkPlayer();
        checkGuard();
    }
}
