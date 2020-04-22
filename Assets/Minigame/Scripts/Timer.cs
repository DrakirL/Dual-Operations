using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Detection timer in minutes, set before entering runtime")]
    [Range(0f, 60f)]
    public float timer;
    float startTime;
    public TextMeshProUGUI text;
    bool paused;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        // Minutes to seconds
        timer *= 60;
        startTime = timer;
    }

    private void FixedUpdate()
    {
        UpdateTimer();
    }

    private void Update()
    {

        if (IsDepleted())
        {
            Debug.Log("Timer reached 0");
        }
    }
    void UpdateTimer()
    {
        if (!paused)
        {
            timer -= Time.fixedDeltaTime;
            string minutes = Mathf.Floor(timer / 60).ToString("00");
            string seconds = Mathf.Floor(timer % 60).ToString("00");

            if (!IsDepleted())
                text.text = string.Format("{0}:{1}", minutes, seconds);
        }        
    }

    public void AddTime(float seconds)
    {
        timer += seconds;
    }

    public void ResetTimer()
    {
        timer = startTime;
    }

    public void TogglePause()
    {
        paused = !paused;
    }

    public bool IsDepleted() => timer <= 0 ? true : false;
}
