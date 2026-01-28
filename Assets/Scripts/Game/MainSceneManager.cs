using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class MainSceneManager : MonoBehaviour, IGameManager
{
    [Header("Session Settings")]
    [SerializeField] int questsPerSession = 3;
    [SerializeField] GameObject[] questPresets;
    [SerializeField] float sessionTime = 300f;
    [SerializeField] TextMeshProUGUI remainingCharactersText;
    [SerializeField] GameObject remainingCharactersUI;
    [SerializeField] GameObject charactersCompletedUI;

    [Header("Sound effects")]
    [SerializeField] AudioSource endSessionSound;
    [SerializeField] AudioSource completedObjectiveSound;

    [Header("UI Elements")]
    [SerializeField] GameObject startGamePanel;
    [SerializeField] GameObject endGamePanel;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] SlicedFilledImage loadingBarFill;
    [SerializeField] GameObject questCanvas;
    [SerializeField] GameObject walkingModeCanvas;
    [SerializeField] GameObject jumpingModeCanvas;
    [SerializeField] GameObject languageCanvas;
    [SerializeField] TMP_Dropdown languageDropdown;

    [Header("Movement Inputs")]
    [SerializeField] GameObject moveProvider;
    [SerializeField] GameObject teleportProvider;
    [SerializeField] InputActionReference buttonA;
    [SerializeField] InputActionReference buttonB;
    [SerializeField] ControllerInputActionManager rightControllerInput;
    [SerializeField] float cooldownTime = 1f;
    [SerializeField] InputActionReference buttonX;
    [SerializeField] InputActionReference buttonY;

    [Header("Height Adjustment")]
    [SerializeField] InputActionReference leftJoystick;
    [SerializeField] XROrigin xrOrigin;
    [SerializeField] float yOffset = 1.5f;

    Dictionary<Quest, bool> activeQuests = new Dictionary<Quest, bool>();

    int remainingCharacters = 3;
    int questCompleted = 0;
    bool inGame = false;
    float timer = 0;

    static int sessionCount = 0;

    void Awake()
    {
        Game.RegisterManager(this);
    }

    void Start()
    {
        moveProvider.SetActive(false);
        teleportProvider.SetActive(false);
        startGamePanel.SetActive(true);
        endGamePanel.SetActive(false);

        walkingModeCanvas.SetActive(false);
        jumpingModeCanvas.SetActive(false);

        yOffset = xrOrigin.CameraYOffset;
    }

    void Update()
    {
        if (inGame)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                EndSession();
            }
        }

        SwitchMovementMode();
        ActiveLanguageCanvas();
        AdjustHeight();
    }

    void OnDisable()
    {
        sessionCount++;
    }

    void InitializeQuests()
    {
        sessionCount %= 2;

        for (int i = 0; i < questsPerSession; i++)
        {
            int index = sessionCount * questsPerSession + i;
            Instantiate(questPresets[index]);
        }
    }

    public void StartNewSession()
    {
        startGamePanel.SetActive(false);
        moveProvider.SetActive(true);
        teleportProvider.SetActive(true);
        questCanvas.SetActive(true);

        InitializeQuests();

        timer = sessionTime;
    }

    private void EndSession()
    {
        timer = 0;
        Inventory.instance.ClearInventory();

        endGamePanel.SetActive(true);
        questCanvas.SetActive(false);
        inGame = false;

        endSessionSound.Play();
    }

    public void AddQuest(Quest questPrefab, GameObject wastes, string desKey)
    {
        // if (!questCanvas.activeInHierarchy) questCanvas.SetActive(true);

        Transform backgroundQuest = questCanvas.transform.GetChild(0);        
        Quest quest = Instantiate(questPrefab, backgroundQuest, false);
        quest.Setup(wastes, desKey);

        activeQuests.Add(quest, false);

        CheckRemainingCharacters();
    }
    
    public void CompleteQuest(Quest quest)
    {
        activeQuests[quest] = true;
        completedObjectiveSound.Play();
        questCompleted++;
    }

    public void AddScore(Quest quest)
    {
        Quest q = quest;
        q.DecreaseRemaining();
        q.IncreaseCorrect();

        CheckCompleteAllQuests();
    }


    public void MinusScore(Quest quest)
    {
        Quest q = quest;
        q.DecreaseRemaining();
        q.IncreaseIncorrect();

        CheckCompleteAllQuests();
    }

    void CheckCompleteAllQuests()
    {
        if (questCompleted < questsPerSession) return;

        foreach (KeyValuePair<Quest, bool> kvp in activeQuests)
        {
            if (!kvp.Value) return;
        }

        EndSession();
    }

    void CheckRemainingCharacters()
    {
        remainingCharacters--;

        if (remainingCharacters == 0)
        {
            remainingCharactersUI.SetActive(false);
            charactersCompletedUI.SetActive(true);
        }
        else
        {
            remainingCharactersText.text = remainingCharacters.ToString();
        }
    }

    public void LoadTutorialScene()
    {
        StartCoroutine(LoadTutorialSceneOperation());
    }
    
    IEnumerator LoadTutorialSceneOperation()
    {
        endGamePanel.SetActive(false);
        loadingPanel.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync("RAC_Tutorial");
        operation.allowSceneActivation = false;
        while (operation.progress < 0.9f)
        {
            loadingBarFill.fillAmount = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }
        loadingBarFill.fillAmount = 1;

        yield return new WaitForSeconds(1);
        operation.allowSceneActivation = true;
    }

    #region Switch Movement Mode
    float switchCooldownTimer = 0;
    bool isBothPressedAB = false;
    void SwitchMovementMode()
    {
        switchCooldownTimer += Time.deltaTime;
        if (switchCooldownTimer < cooldownTime) return;

        bool a = buttonA.action.IsPressed();
        bool b = buttonB.action.IsPressed();

        if (a && b)
        {
            if (!isBothPressedAB)
            {
                rightControllerInput.smoothMotionEnabled = !rightControllerInput.smoothMotionEnabled;
                if (rightControllerInput.smoothMotionEnabled)
                {
                    DisplayMovementCanvas(walkingModeCanvas);
                }
                else
                {
                    DisplayMovementCanvas(jumpingModeCanvas);
                }

                switchCooldownTimer = 0;
                isBothPressedAB = true;
            }
        }
        else
        {
            isBothPressedAB = false;
        }
    }

    void DisplayMovementCanvas(GameObject canvas)
    {
        walkingModeCanvas.GetComponent<TweenFadeOut>().ForceEnd();
        jumpingModeCanvas.GetComponent<TweenFadeOut>().ForceEnd();
        canvas.SetActive(true);
    }
    #endregion 

    #region Language Selection
    bool isBothPressedXY = false;
    void ActiveLanguageCanvas()
    {
        bool x = buttonX.action.IsPressed();
        bool y = buttonY.action.IsPressed();

        if (x && y)
        {
            if (!isBothPressedXY)
            {
                languageCanvas.SetActive(!languageCanvas.activeInHierarchy);
                isBothPressedXY = true;
            }
        }
        else
        {
            isBothPressedXY = false;
        }
    }



    public void ChangeLanguage()
    {
        int value = languageDropdown.value;

        switch (value)
        {
            case 0:
                LocalizationManager.Instance.SetLanguage("French");
                break;
            case 1:
                LocalizationManager.Instance.SetLanguage("English");
                break;
        }

        languageCanvas.SetActive(false);
        languageDropdown.SetValueWithoutNotify(-1);
    }
    #endregion

    void AdjustHeight()
    {
        float value = leftJoystick.action.ReadValue<Vector2>().y;
        
        if (Mathf.Abs(value) < 0.1f) return;

        yOffset += value * 0.02f;
        xrOrigin.CameraYOffset = yOffset;
    }
}