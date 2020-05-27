using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScript : MonoBehaviour
{
    [SerializeField] GameObject leaveObject;
    [SerializeField] float movingSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * movingSpeed * Time.deltaTime;
    }


    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject == leaveObject)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
