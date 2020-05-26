using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Detection timer in minutes, set before entering runtime")]
    [Range(0f, 60f)]
    [SerializeField] float timer;
    float startTime;
    [SerializeField] TextMeshProUGUI text;
    bool paused;
    bool depleted = false;
    [SerializeField] Text hackerText;

    public enum TimerTypes
    {
        agent, 
        minigame,
        hacker
    }

    public TimerTypes timerType;

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

        if (IsDepleted() && !depleted)
        {
            if (timerType == TimerTypes.agent)
            {
                GameManager._instance.LoseState();
                depleted = true;
            }
            else
                return;
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
            {
                if (timerType == TimerTypes.agent)
                {
                    text.text = string.Format("{0}:{1}", minutes, seconds);
                }
                else if (timerType == TimerTypes.hacker)
                {
                    hackerText.text = string.Format("{0}:{1}", minutes, seconds);
                }
            }
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
