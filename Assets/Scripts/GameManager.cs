using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton reference
    public static GameManager _instance { get; private set; }
    [Tooltip("Time it takes before next scene will be loaded")]
    public float reloadTime = 0.2f;
    public bool winState = false;
    bool state;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (AlertMeter._instance != null)
        {
            if (AlertMeter._instance.IsFull() && !state)
            {
                LoseState();
                state = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
    public void LoseState()
    {
        // Maybe put in alertmeter script, avoiding conflicts atm
        if (!state)
        {
            LoadScene("Lose", reloadTime);
            state = true;
        }       
    }
    public void LoadScene(string sceneName, float time)
    {
        StartCoroutine(Delay(time, sceneName));      
    }
    IEnumerator Delay(float time, string s)
    {
        yield return new WaitForSeconds(time);
        if(s == "Main Menu")
        {
            state = false;
            /*foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
            {
                Destroy(o);
            }*/
        }
        SceneManager.LoadScene(s);
    }
}
