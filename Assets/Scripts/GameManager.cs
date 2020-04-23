using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance { get; private set; }

    [Tooltip("Time it takes for scene to be loaded on win/loss")]
    public float reloadTime = 3;

    [HideInInspector] public int generatorCounter = 0;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

   public void LoseState()
    {
        // Lose
        SceneManager.LoadScene("Lose");
    }

    public void WinState()
    {
        SceneManager.LoadScene("Win");
    }

    void UpdateMap()
    {
        // generator interact?
        // update part of or whole map?
    }
}
