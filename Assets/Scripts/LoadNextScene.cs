using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField] string sceneName;
    float time;

    private void Start()
    {
        time = Time.time + GameManager._instance.reloadTime;
    }

    private void Update()
    {
        if (Time.time > time)
        SceneManager.LoadScene(sceneName);
    }
}
