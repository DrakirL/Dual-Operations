using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertGetText : MonoBehaviour
{
    [SerializeField] Slider alertSlide; 
    float Value;
    [SerializeField] Text StringValue;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Value = AlertMeter._instance.alertValue;
        if (alertSlide != null)
        {
            alertSlide.value = Value;
        }
        if (StringValue != null)
        {
            StringValue.text = Value.ToString("0") + "/100";
        }
    }
}
