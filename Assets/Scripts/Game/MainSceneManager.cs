using System.Collections;
using System.Collections.Generic;
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

    [Header("Movement Inputs")]
    [SerializeField] GameObject moveProvider;
    [SerializeField] GameObject teleportProvider;
    [SerializeField] InputActionReference buttonA;
    [SerializeField] InputActionReference buttonB;
    [SerializeField] ControllerInputActionManager rightControllerInput;
    [SerializeField] float cooldownTime = 1f;

    Dictionary<Quest, bool> activeQuests = new Dictionary<Quest, bool>();

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
        if (!questCanvas.activeInHierarchy) questCanvas.SetActive(true);

        Transform backgroundQuest = questCanvas.transform.GetChild(0);        
        Quest quest = Instantiate(questPrefab, backgroundQuest, false);
        quest.Setup(wastes, desKey);

        activeQuests.Add(quest, false);
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
    bool isBothPressed = false;
    void SwitchMovementMode()
    {
        switchCooldownTimer += Time.deltaTime;
        if (switchCooldownTimer < cooldownTime) return;

        bool a = buttonA.action.IsPressed();
        bool b = buttonB.action.IsPressed();

        if (a && b)
        {
            if (!isBothPressed)
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
                isBothPressed = true;
            }
        }
        else
        {
            isBothPressed = false;
        }
    }

    void DisplayMovementCanvas(GameObject canvas)
    {
        walkingModeCanvas.GetComponent<TweenFadeOut>().ForceEnd();
        jumpingModeCanvas.GetComponent<TweenFadeOut>().ForceEnd();
        canvas.SetActive(true);
    }
    #endregion 
}