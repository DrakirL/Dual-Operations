using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror
{ 
    public class HackerScript : NetworkBehaviour
    {
        [SerializeField] GameObject camera;
        [SerializeField] Text missionTime; 
        public GameObject PlayerCanvasObject;
        [SerializeField] HackerButtonHandler HB;
        

        // Start is called before the first frame update
        void Start()
        {
            PlayerCanvasObject.SetActive(false);
            if (isLocalPlayer)
            {
                MinigameManager.Instance.GetHackerCanvas(PlayerCanvasObject);
                camera.SetActive(true);
                PlayerCanvasObject.SetActive(true);
                //StartCoroutine(a());
            }
            StartCoroutine(delayedSetHacker());
            //CameraManager.Instance.hacker = this;
          
        }
        IEnumerator delayedSetHacker()
        {
            yield return new WaitForSeconds(1);
            GetPlayer.Instance.Hs = this;
        }
        // Update is called once per frame
        void Update()
        {
            TimeScript.time -= Time.deltaTime;
            int sec = (int)TimeScript.time % 60;
            int min = (int)TimeScript.time / 60;

            string timeString = min.ToString();            
            if(sec < 10)
            {
                timeString = timeString + ":0" + sec.ToString(); 
            }
            else
            {
                timeString = timeString + ":" + sec.ToString();
            }

            missionTime.text = timeString;
        }
        [ClientRpc]
        public void RpcShutDownCamera(int hackableNumber)
        {
            HB.forcedDown(hackableNumber);
            CameraManager.Instance.shutDownCamera(hackableNumber);
        }

        public void cameraBackOnline(int index)
        {
            HB.cameraBackOnline(index);
        }
        public void cameraGoneOffline(int index)
        {
            HB.cameraGoneOffline(index);
        }
        public void UsingCamera(int index)
        {
            HB.UsingCamera(index);
        }
        public void RadioBackOnline(int index)
        {
            HB.RadioBackOnline(index);
        }


        IEnumerator a()
        {
            yield return new WaitForSeconds(0.5f);
            GameObject ca = AlertMeter._instance.gameObject;
            ca.active = false;
            ca.active = true;
        }
    }
}
