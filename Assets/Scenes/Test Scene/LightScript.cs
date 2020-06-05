using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{

   

    void Start()
        {
            InvokeRepeating("MakeVisible", 0.0f, 0.5f);
        }

        void MakeVisible()
        {
            if (gameObject.activeSelf == false)
            {
                if (Camera.current != null)
                {
                    Vector3 pos = Camera.current.WorldToViewportPoint(transform.position);
                    if (pos.z > 0 && pos.x >= 0.0f && pos.x <= 1.0f && pos.y >= 0.0f && pos.y <= 1.0f)
                    {
                        if (isDebug) Debug.Log(pos.ToString("F4"));
                        gameObject.SetActive(true);
                    }
                }
            }
        }
    }

