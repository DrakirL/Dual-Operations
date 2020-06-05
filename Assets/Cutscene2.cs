using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Cutscene2 : MonoBehaviour
{
    VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }
    private void Start()
    {
        PlayCutscene();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndCutscene(videoPlayer);
        }

    }
    public void PlayCutscene()
    {
        //pause timer
        videoPlayer.Play();
        videoPlayer.loopPointReached += EndCutsceneEvent;
    }
    void EndCutsceneEvent(VideoPlayer vp)
    {
        EndCutscene(vp);
    }

    void EndCutscene(VideoPlayer vp)
    {
        SceneManager.LoadScene("Main Menu");
    }
}
