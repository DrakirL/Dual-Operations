using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Mirror;

public class GameManager : NetworkBehaviour
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
        if(AlertMeter._instance != null)
        {
            if (AlertMeter._instance.IsFull() && !state)
            {
                LoseState();
                state = true;
            }
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

    [Command]
    public void CmdLoadScene(string sceneName, float time)
    {
        LoadScene(sceneName, time);
        RpcLoadScene(sceneName, time);
    }

    public void LoadScene(string sceneName, float time)
    {
        StartCoroutine(Delay(time, sceneName));        
    }

    [ClientRpc] 
    void RpcLoadScene(string sceneName, float time)
    {
        LoadScene(sceneName, time);
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
