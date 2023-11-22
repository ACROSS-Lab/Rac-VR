using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonManager : MonoBehaviour
{

    [SerializeField] private Button startButton;

    void OnEnable() {
        startButton.onClick.AddListener(SetGameState);
    }

    void OnDisable() {
        startButton.onClick.RemoveListener(SetGameState);
    }

    void Start() {
        startButton.interactable = false;
    }

    void Update() {
        if (GameManager.Instance.IsGameReadyToStart() && startButton.interactable == false) {
            GameManager.Instance.SetGameReadyToStart(false);
            startButton.interactable = true;
        }
    }

    private void SetGameState() {
        GameManager.Instance.UpdateGameState(GameState.GAME);
    }
}
