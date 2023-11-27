using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayManager : MonoBehaviour
{
    [SerializeField] private GameObject timerOverlay;
    [SerializeField] private GameObject startOverlay;

    private GameState currentState;

    private bool overlayUpdateRequested;

    void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += UpdateOverlayOnStateChanged;
    }
    

    void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= UpdateOverlayOnStateChanged;
    }

    void Start() {
        timerOverlay.SetActive(false);
        currentState = GameState.MENU;
    }

    void LateUpdate() {
        if (overlayUpdateRequested) {
            overlayUpdateRequested = false;
            timerOverlay.SetActive(currentState == GameState.GAME);
            startOverlay.SetActive(currentState != GameState.GAME);
        }
    }

    private void UpdateOverlayOnStateChanged(GameState newState) {
        currentState = newState;
        overlayUpdateRequested = true;
    }
}
