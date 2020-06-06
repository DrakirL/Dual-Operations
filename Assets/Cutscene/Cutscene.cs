using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Cutscene : MonoBehaviour
{
    VideoPlayer videoPlayer;
    [SerializeField] GameObject renderImg;
    [SerializeField] GameObject fmod;
    
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
        if(fmod != null)
        fmod.SetActive(true);
        Destroy(renderImg);
        Destroy(vp.gameObject);
    }
}
