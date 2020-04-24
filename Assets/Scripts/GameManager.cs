using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton reference
    public static GameManager _instance { get; private set; }
    [Tooltip("Time it takes before next scene will be loaded")]
    public float reloadTime = 0.5f;
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
        if (AlertMeter._instance.IsFull() && !state)
        {
            LoseState();
            state = true;
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
    public void WinState()
    {
        LoadScene("Win", reloadTime);
    }
    // General scene load function with loadtime as a parameter
    public void LoadScene(string sceneName, float time)
    {
        StartCoroutine(Delay(time, sceneName));
        
    }
    // dunno
    void UpdateMap()
    {
        // generator interact?
        // update part of or whole map?
    }
    IEnumerator Delay(float time, string s)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(s);
    }
}
