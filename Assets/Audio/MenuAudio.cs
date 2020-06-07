using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuAudio : MonoBehaviour
{
    public static MenuAudio Instance;
    public bool buttons;

    public void Start()
    {
        if(!buttons)
        {
            if (MenuAudio.Instance == null)
                MenuAudio.Instance = this;

            else
                Destroy(this.gameObject);

            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main Level")
            Destroy(this.gameObject);
    }

    public void PlaySound(string path)
    {
        UnityEngine.Debug.Log("Borde spela ett ljud");
        FMODUnity.RuntimeManager.PlayOneShot(path, transform.position);
    }
}
