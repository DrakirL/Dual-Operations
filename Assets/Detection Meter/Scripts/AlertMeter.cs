using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class AlertMeter : NetworkBehaviour
{
    public static AlertMeter _instance { get; private set; }

    [HideInInspector] public TextMeshProUGUI text;
    [HideInInspector] public Slider slider;

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
   [SyncVar] [HideInInspector] public float timeStamp;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        slider = GetComponent<Slider>();

        SetDetected(false);
    }

    private void Update()
    {
        UpdateMeter(); 
    }

    void UpdateMeter()
    {
        // Update value to text
        slider.value = alertValue;
        text.text = alertValue.ToString("0") + "/100";

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
            }
            else if (alertValue >= 100f)
            {
                alertValue = 100f;
            }
        }
        
    }
    void StopMeter()
    {
        if (Time.time > alertTimeStamp)
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
        }
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
        GetPlayer.Instance.addAlertServer(value);
        //UseAddAlert(value);
       /*if(isServer)
        {
            UseAddAlert(value);
        }
        else
        {
            GetPlayer.Instance.addAlertServer(value);
        }*/
    }
    public float getAlert()
    {
        return alertValue;
    }

    // Check if meter has reached the limit
    public bool IsFull() => alertValue >= 100f ? true : false;

    public void setV(System.Single oldValue, System.Single newValue)
    {
        GetPlayer.Instance.addAlertServer(newValue);
        oldValue = newValue;
        alertValue = newValue;
    }
}
