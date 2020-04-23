using UnityEngine;
using UnityEngine.SceneManagement;

public class PreLoad : MonoBehaviour
{
    private void Awake()
    {
        if (LoadingSceneIntegration.otherScene > 0)
        {
            Debug.Log("Returning again to the scene: " + LoadingSceneIntegration.otherScene);
            SceneManager.LoadScene(LoadingSceneIntegration.otherScene);
        }
    }
}
