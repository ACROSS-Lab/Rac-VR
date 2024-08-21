using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{

    [Header("Display Settings")]
    [SerializeField] private TMPro.TextMeshProUGUI timerText;
    [SerializeField] private Color startColor = new Color(0,201,0,255);
    [SerializeField] private Color midColor = new Color(255,218,0,255);
    [SerializeField] private Color endColor = new Color(255,0,0,255);
    
    private static float timerDuration = 120;
    
    private bool timerRunning = false;
    private float midTime;
    public float timeRemaining;

    public bool InfoSentToGAMA = false;

    public static Timer Instance = null;

    // ############################################################


    private void Awake()
    {
        Instance = this;

    }


    void Start() {
        timeRemaining = timerDuration;
        midTime = timeRemaining / 2;
        DisplayTime(timeRemaining-1);
        timerRunning = true;
    }

 

    void Update() {

        if (timerRunning)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    DisplayTime(timeRemaining);
                }
                else
                {
                    Reset();
                 
                }
            }
           
        
    }

    // ############################################################

    private void DisplayTime(float time) {
        time += 1;
        float minutes = Mathf.FloorToInt(time / 60); 
        float seconds = Mathf.FloorToInt(time % 60);
        if (timerText != null) {
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            if (time > midTime) {
                timerText.color = Color.Lerp(midColor, startColor, (time - midTime) / midTime);
            } else {
                timerText.color = Color.Lerp(endColor, midColor, (time) / midTime);
            }
        }
        
    }

    public void Reset() {
        timerRunning = false;
        timeRemaining = timerDuration;
        Dictionary<string, string> args = new Dictionary<string, string> {
            {"id",ConnectionManager.Instance.getUseMiddleware() ? ConnectionManager.Instance.GetConnectionId()  : ("\"" + ConnectionManager.Instance.GetConnectionId() +  "\"") }
        };
        ConnectionManager.Instance.SendExecutableAsk("desactive_player", args);
        SceneManager.LoadScene("EndingMenu");
    }

   
    // ############################################################

    

    public static float GetTimerDuration() {
        return timerDuration;
    }

    public void SetTimerRunning(bool running) {
        timerRunning = running;
    }

    public bool IsTimerRunning() {
        return timerRunning;
    }

    
}
