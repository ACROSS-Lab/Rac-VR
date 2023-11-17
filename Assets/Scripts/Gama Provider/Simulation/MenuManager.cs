using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    
    [SerializeField] private GameObject startOverlay;
    [SerializeField] private GameObject ingameOverlay;
    [SerializeField] private GameObject endOverlay;
    [SerializeField] private GameObject crashOverlay;
    [SerializeField] private GameObject waitingOverlay;
    [SerializeField] private GameObject loadingDataOverlay;

    [SerializeField] private Dictionary<GameState, List<GameObject>> overlays;
    [SerializeField] private GameObject handHud;

    private bool updateRequested;
    private GameState curentState;

    void Start() {
        updateRequested = true;
        curentState = GameState.MENU;
    }
    
    void OnEnable() {
        GameManager.OnGameStateChanged += HandleGameStateChange;
    }

    void OnDisable() {
        GameManager.OnGameStateChanged -= HandleGameStateChange;
    }

    void LateUpdate() {
        if (updateRequested) {
            startOverlay.SetActive(curentState == GameState.MENU);
            waitingOverlay.SetActive(curentState == GameState.WAITING);
            loadingDataOverlay.SetActive(curentState == GameState.LOADING_DATA);
            ingameOverlay.SetActive(curentState == GameState.GAME);
            handHud.SetActive(curentState == GameState.GAME);
            endOverlay.SetActive(curentState == GameState.END);
            crashOverlay.SetActive(curentState == GameState.CRASH);
            updateRequested = false;
        }
    }

    private void HandleGameStateChange(GameState newState) {
        Debug.Log("MenuManager: HandleGameStateChange -> " + newState);
        updateRequested = true;
        curentState = newState; 
    }   
}
