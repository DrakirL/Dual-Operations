using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CreditScript : MonoBehaviour
{
    [SerializeField] GameObject leaveObject;
    [SerializeField] float movingSpeed;
    [SerializeField] TextMeshPro escText;
    [SerializeField] float escTextDelay = 0.6f;
    [SerializeField] float escTextDuration = 10;

    private void Start()
    {
        escText.gameObject.SetActive(false);
        StartCoroutine(displayEscText(escTextDelay, escTextDuration));
    }

    float time = 0;
    bool dis = false;
    bool canLeave = false;
    IEnumerator displayEscText(float timeBeforeDisplay, float duration)
    {
        yield return new WaitForSeconds(timeBeforeDisplay);
        canLeave = true;
        time = 0;
        escText.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(duration);
        dis = true;
    }
    
    void Update()
    {
        if (!dis)
        {
            time += Time.deltaTime;
            Color color = escText.color;
            color.a = (1 - Mathf.Cos(5f*time));
            escText.color = color;
        }
        else
        {
            Color color = escText.color;
            color.a = Mathf.Lerp(color.a, 0, 0.1f);
            escText.color = color;
        }
        transform.position += Vector3.up * movingSpeed * Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.Escape) && canLeave)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }


    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject == leaveObject)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
