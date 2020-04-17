﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror
{ 
    public class HackerScript : NetworkBehaviour
    {
        [SerializeField] Text missionTime; 
        public GameObject PlayerCanvasObject;
        // Start is called before the first frame update
        void Start()
        {
            if (isLocalPlayer)
            {
                PlayerCanvasObject.SetActive(true);
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
    }
}