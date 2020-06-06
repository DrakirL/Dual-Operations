using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class USB : MonoBehaviour
{

    public Image img;
    // Start is called before the first frame update
    void Start()
    {
        img.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if(GameManager._instance.winState)
{
            img.enabled = true;
        }
    }
}
