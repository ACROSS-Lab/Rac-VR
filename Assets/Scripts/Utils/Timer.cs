using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    [Header("Display Settings")]
    [SerializeField] private TMPro.TextMeshProUGUI timerText;
    [SerializeField] private Color startColor = new Color(0,201,0,255);
    [SerializeField] private Color midColor = new Color(255,218,0,255);
    [SerializeField] private Color endColor = new Color(255,0,0,255);
    
    private static float timerDuration = 20.0f;
    
    private bool timerRunning = false;
    private float midTime;
    private float timeRemaining;

    // ############################################################

    void Start() {
        Debug.Log("Timer: start");
        timeRemaining = timerDuration;
        midTime = timeRemaining / 2;
        DisplayTime(timeRemaining-1);
    }

   /* void OnEnble() {
        GameManager.Instance.OnGameStateChanged += HandleTimerOnStateChanged;
    }

    void OnDisable() {
        GameManager.Instance.OnGameStateChanged -= HandleTimerOnStateChanged;
    }*/

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
                    //  timerRunning = false;
                    GameManager.Instance.UpdateGameState(GameState.IDLE);
                    ConnectionManager.Instance.SendExecutableExpression("do exploration_over(" + GameManager.Instance.GetVillageId() + ");");
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
    }

    /*private void HandleTimerOnStateChanged(GameState newState) {
        if (newState == GameState.IDLE) {
            Reset();
        }

    }*/

    // ############################################################

    public static void SetTimerDuration(float duration) {
        timerDuration = duration;
    }

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
