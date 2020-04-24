using UnityEngine;

public class LoadScene : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] float reloadTime;

    private void Start()
    {
        GameManager._instance.LoadScene(sceneName, reloadTime);
    }   
}
