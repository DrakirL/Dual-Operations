using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance { get; private set; }

    [Tooltip("Enter the scene to be loaded when game over")]
    public string reloadSceneNameOnGameOver;
    public float reloadTime = 3;

    // Maybe lose scene?
    public GameObject loseImage;
    public GameObject winImage;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (AlertMeter._instance.IsFull())
        {
            LoseState();
        }
    }

    void LoseState()
    {
        // Lose
        // reload scene?
        loseImage.SetActive(true);
        StartCoroutine(Reload(loseImage));
                    
    }

    void WinState()
    {
        winImage.SetActive(true);
        StartCoroutine(Reload(winImage));
    }

    void UpdateMap()
    {
        // generator interact?
        // update part of or whole map?
    }

    IEnumerator Reload(GameObject o)
    {
        yield return new WaitForSeconds(reloadTime);
        SceneManager.LoadScene(reloadSceneNameOnGameOver);
        o.SetActive(false);
    }
}
