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
        

        // Start is called before the first frame update
        void Start()
        {
            if (isLocalPlayer)
            {
                MinigameManager.Instance.GetHackerCanvas(PlayerCanvasObject);
                camera.SetActive(true);
                PlayerCanvasObject.SetActive(true);
                StartCoroutine(a());
            }
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
            CameraManager.Instance.shutDownCamera(hackableNumber);
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
