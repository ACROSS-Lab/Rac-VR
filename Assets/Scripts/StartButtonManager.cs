using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonManager : MonoBehaviour
{

    [SerializeField] private Button startButton;
    [SerializeField] private TMPro.TextMeshProUGUI debugText;

    private bool changeInteractableRequested;

    void OnEnable() {
        GameManager.Instance.OnGameStateChanged += HandleStartButtonOnStateChanged;
    }

    void OnDisable() {
        GameManager.Instance.OnGameStateChanged -= HandleStartButtonOnStateChanged;
    }

    void Start() {
        startButton.interactable = false;
        changeInteractableRequested = false;
    }

    void Update() {
        if (GameManager.Instance.IsGameReadyToStart() && startButton.interactable == false) {
            GameManager.Instance.SetGameReadyToStart(false);
            startButton.interactable = true;
        }

        if(changeInteractableRequested) {
            startButton.interactable = false;
            changeInteractableRequested = false;
        }
    }

    public void HandleStartButtonOnStateChanged(GameState newState) {
        if (newState == GameState.IDLE) {
            changeInteractableRequested = true;
        }
    }
}
