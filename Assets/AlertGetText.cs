using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertGetText : MonoBehaviour
{
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
        StringValue.text = Value.ToString("0") + "/100";
    }
}
