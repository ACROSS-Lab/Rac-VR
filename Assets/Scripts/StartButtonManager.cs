using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonManager : MonoBehaviour
{

    [SerializeField] private Button startButton;
    [SerializeField] private TMPro.TextMeshProUGUI debugText;
    private bool ready = false;

    public Timer timer;
    private bool changeInteractableRequested;

    void OnEnable() {
        SimulationManager.Instance.OnGameStateChanged += HandleStartButtonOnStateChanged;
    }

    void OnDisable() { 
        SimulationManager.Instance.OnGameStateChanged -= HandleStartButtonOnStateChanged;
    }

    void Start() {
        startButton.interactable = false;
        changeInteractableRequested = false;
    }

    void Update() {
        if (!ready && SimulationManager.Instance.GetCurrentState() == GameState.READY && startButton.interactable == false) {
            //GameManager.Instance.SetGameReadyToStart(false);
            startButton.interactable = true;
            ready = true;
        }

        if(changeInteractableRequested) {
            startButton.interactable = false;
            changeInteractableRequested = false;
        }
    }


    public void StartGame()
    {
        timer.SetTimerRunning(true);
        GameObject tuto = GameObject.FindGameObjectWithTag("Tuto");
        tuto.active = false;
    }
    public void HandleStartButtonOnStateChanged(GameState newState) {
        if (newState == GameState.IDLE) {
            changeInteractableRequested = true;

            ready = false;
        }
    }
}
