using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
//olika beroende server/klient
//upptäckt boolean
//

public class AlertMeter : NetworkBehaviour
{
    public static AlertMeter _instance { get; private set; }

    public TextMeshProUGUI text;
    public Slider slider;
    public Image alertImage;

    [Tooltip("Time in seconds for meter to start decrease")]
    [SerializeField] float alertDecreaseTimer = 3f;
    [SerializeField] float alertDecreaseSpeed = 1f;
    [SerializeField] float alertDecreaseValue = 1f;
    [Space(10)]

    [Tooltip("Time in seconds for meter to start increase")]
    [SerializeField] float alertIncreaseTimer = 1f;
    [SerializeField] float alertIncreaseSpeed = 1f;
    [SerializeField] float alertIncreaseValue = 1f;

    [Space(10)]
    //[SyncVar(hook = nameof(setV))]
    [SyncVar] [SerializeField] public float alertValue;

    [SerializeField] bool detected;
    float alertTimeStamp;
   /*[SyncVar]*/ [HideInInspector] public float timeStamp;
   public float tmpCounter = 0;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Destroy(gameObject);
            Debug.LogError("more than one AlertMeter detected, don't ignore this error");
        }
    }

    private void Start()
    {
        //text = GetComponentInChildren<TextMeshProUGUI>();
        //slider = GetComponent<Slider>();

        SetDetected(false);
    }

    private void Update()
    {
        // Update value to text
        slider.value = alertValue;
        text.text = alertValue.ToString("0") + "/100";
        try
        {
            if (NetworkServer.localConnection.connectionId == 0)
            {
                UpdateMeter();
                tmpCounter += Time.deltaTime;
            }
        }
        catch
        {
            //nothing should happen here, you are not on the server 
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayAlertFlash(2);
        }
    }

    void UpdateMeter()
    {       
        if(detected)
            StartMeter();
        else
            StopMeter();      
    }

    void StartMeter()
    {     
        if(Time.time > alertTimeStamp)
        {
            // Increase the alert meter after set time
            if (Time.time > timeStamp + alertIncreaseSpeed && alertValue <= 100f)
            {
                timeStamp += Time.time + alertIncreaseSpeed;
                AddAlert(alertIncreaseValue);
                tmpCounter = 0;
            }
            else if (alertValue >= 100f)
            {
                alertValue = 100f;
            }
        }       
    }
    void StopMeter()
    {
        if(tmpCounter > alertDecreaseTimer)
        {
            alertValue = Mathf.Clamp(alertValue-alertDecreaseValue, 0, 100);
            tmpCounter--; 
        }
     
        /*if (Time.time > alertTimeStamp)
        {
            // Increase the alert meter after set time
            if (Time.time > timeStamp + alertDecreaseSpeed && alertValue >= 0)
            {
                timeStamp += Time.time + alertDecreaseSpeed;
                  AddAlert(-alertDecreaseValue);
            }
            else if (alertValue <= 0f)
            {
                alertValue = 0f;
            }
        }*/
    }
    public void SetDetected(bool boolToSet)
    {
        if(boolToSet != detected)
        {
            detected = boolToSet;

            if (detected)
                alertTimeStamp = Time.time + alertIncreaseTimer;
            else
                alertTimeStamp = Time.time + alertDecreaseTimer;
        }    
    }

    // Add alert value to the meter
    public void AddAlert(float value)
    {
        //if (NetworkServer.localConnection.connectionId == 1)
        //{
            //UseAddAlert(value);
            if (isServer)
            {
                //Debug.Log("server add alert");
                alertValue = Mathf.Clamp(alertValue + value, 0, 100);
                timeStamp = Time.time;
            }
            else
            {
                //Debug.Log("client add alert");
                GetPlayer.Instance.addAlertServer(value);
            }
        //}
    }
    public void AddAlert(float value, float flashLength)
    {
        //if (NetworkServer.localConnection.connectionId == 1)
        //{
        //UseAddAlert(value);
        if (isServer)
        {
            //Debug.Log("server add alert");
            alertValue = Mathf.Clamp(alertValue + value, 0, 100);
            timeStamp = Time.time;
            GetPlayer.Instance.addAlertServerServer(value, flashLength);

        }
        else
        {
            //Debug.Log("client add alert");
            GetPlayer.Instance.addAlertServer(value, flashLength);
        }
        //}
    }
    public float getAlert()
    {
        return alertValue;
    }

    // Check if meter has reached the limit
    public bool IsFull() => alertValue >= 100f ? true : false;

    public void PlayAlertFlash(float time)
    {
        StartCoroutine(FadeImage(time));
    }
    [ClientRpc]
    public void RpcPlayAlertFlashOnClient(float time)
    {
        Debug.LogWarning("thisHappend");
        StartCoroutine(FadeImage(time));
    }

    IEnumerator FadeImage(float time)
    {
        // Fade from opaque to transparent
        for (float i = 1; i >= 0; i -= Time.deltaTime / time)
        {
            // Set color with i as alpha
            alertImage.color = new Color(alertImage.color.r, alertImage.color.g, alertImage.color.b, i);
            yield return null;
        }
    }

    /* public void setV(System.Single oldValue, System.Single newValue)
     {
         GetPlayer.Instance.addAlertServer(newValue);
         oldValue = newValue;
         alertValue = newValue;
     }*/
}
