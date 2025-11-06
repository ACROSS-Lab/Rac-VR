using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class TutorialManager : MonoBehaviour, IGameManager
{
    public static TutorialManager instance;

    [Header("Tutorial Elements")]
    [SerializeField] GameObject movePoint;
    [SerializeField] GameObject wastesContainer;
    [SerializeField] GameObject binContainer;
    [SerializeField] GameObject NPC;

    [Header("UI Elements")]
    [SerializeField] GameObject moveCanvas;
    [SerializeField] GameObject pickupCanvas;
    [SerializeField] GameObject dropCanvas;
    [SerializeField] GameObject characterCanvas;
    [SerializeField] GameObject binCanvas;
    [SerializeField] GameObject finishCanvas;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] SlicedFilledImage loadingBarFill;

    [Header("Movement")]
    [SerializeField] InputActionReference mainButton;
    [SerializeField] ControllerInputActionManager rightControllerInput;

    [Header("Sound effects")]
    [SerializeField] AudioSource endTutorialSound;

    int numWastesCollected = 0;
    int numWastesProcessed = 0;

    void Awake()
    {
        instance = this;
        Game.RegisterManager(this);
    }

    void Start()
    {
        StartCoroutine(WaitForInventoryUpdate());
    }

    void Update()
    {
        if (mainButton.action.WasPressedThisFrame())
        {
            rightControllerInput.smoothMotionEnabled = !rightControllerInput.smoothMotionEnabled;
        }
    }

    IEnumerator WaitForInventoryUpdate()
    {
        yield return new WaitUntil(() => Inventory.instance != null);
        Inventory.instance.OnInventoryUpdated += CheckAmountCollected;
    }

    void OnDisable()
    {
        Inventory.instance.OnInventoryUpdated -= CheckAmountCollected;
    }

    public void MoveToDestination()
    {
        movePoint.SetActive(false);
        moveCanvas.SetActive(false);
        characterCanvas.SetActive(true);
        NPC.SetActive(true);
    }

    public void CheckAmountCollected()
    {
        numWastesCollected = Inventory.instance.wastes.Count;
        if (numWastesCollected == 5)
        {
            pickupCanvas.SetActive(false);
            dropCanvas.SetActive(false);
            binCanvas.SetActive(true);
            binContainer.SetActive(true);
        }
    }

    public void AddScore(WasteType type, int points)
    {
        numWastesProcessed++;
        if (numWastesProcessed == 5)
        {
            binCanvas.SetActive(false);
            finishCanvas.SetActive(true);
            endTutorialSound.Play();
        }
    }

    public void MinusScore(WasteType type, int points)
    {
        numWastesProcessed++;
        if (numWastesProcessed == 5)
        {
            binCanvas.SetActive(false);
            finishCanvas.SetActive(true);
        }
    }

    public void IncrementCharactersTalkedTo()
    {
        wastesContainer.SetActive(true);
        pickupCanvas.SetActive(true);
        dropCanvas.SetActive(true);
    }

    public void TurnOffCharacterCanvas()
    {
        characterCanvas.SetActive(false);
    }

    public void LoadMainScene()
    {
        StartCoroutine(LoadMainSceneOperation());
    }
    
    IEnumerator LoadMainSceneOperation()
    {
        finishCanvas.SetActive(false);
        loadingPanel.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync("Rac_MainScene_NonGP");
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
}
