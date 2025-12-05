using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class TutorialManager : MonoBehaviour, IGameManager
{
    public static TutorialManager instance;

    [Header("Tutorial Elements")]
    [SerializeField] GameObject movePoint1;
    [SerializeField] GameObject teleportPoint;
    [SerializeField] GameObject movePoint2;
    [SerializeField] GameObject wastesContainer;
    [SerializeField] GameObject bin;
    [SerializeField] GameObject NPC;

    [Header("UI Elements")]
    [SerializeField] GameObject move1Canvas;
    [SerializeField] GameObject teleportCanvas;
    [SerializeField] GameObject noteCanvas;
    [SerializeField] GameObject move2Canvas;
    [SerializeField] GameObject pickupCanvas;
    [SerializeField] GameObject dropCanvas;
    [SerializeField] GameObject characterCanvas;
    [SerializeField] GameObject dialogueCanvas;
    [SerializeField] GameObject binCanvas;
    [SerializeField] GameObject finishCanvas;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] SlicedFilledImage loadingBarFill;
    
    [SerializeField] GameObject walkingModeCanvas;
    [SerializeField] GameObject jumpingModeCanvas;

    [Header("Movement")]
    [SerializeField] InputActionReference mainButton;
    [SerializeField] ControllerInputActionManager rightControllerInput;
    [SerializeField] float cooldownTime = 2.5f;

    [Header("Sound effects")]
    [SerializeField] AudioSource endTutorialSound;

    int numWastesCollected = 0;
    int numWastesProcessed = 0;
    bool firstClick = true;
    bool isCollected = false;
    float switchCoolDownTimer = 0;

    void Awake()
    {
        instance = this;
        Game.RegisterManager(this);
    }

    void Start()
    {
        StartCoroutine(WaitForInventoryUpdate());

        walkingModeCanvas.SetActive(false);
        jumpingModeCanvas.SetActive(false);
    }

    void Update()
    {
        SwitchMovementMode();
    }

    IEnumerator WaitForInventoryUpdate()
    {
        yield return new WaitUntil(() => Inventory.instance != null);
        Inventory.instance.OnInventoryUpdated += CheckAmountCollected;
    }

    void OnDisable()
    {
        if (Inventory.instance == null) Debug.Log("Inventory is null");
        Inventory.instance.OnInventoryUpdated -= CheckAmountCollected;
    }

    public void Reached1stDestination()
    {
        if(rightControllerInput.smoothMotionEnabled)
        {
            movePoint1.SetActive(false);
            move1Canvas.SetActive(false);
            teleportPoint.SetActive(true);
            teleportCanvas.SetActive(true);
            noteCanvas.SetActive(true);
        }
    }

    public void Reached1stDestinationNoTeleport()
    {
        movePoint1.SetActive(false);
        move1Canvas.SetActive(false);
        characterCanvas.SetActive(true);
        NPC.SetActive(true);
    }

    public void ReachedTeleportDestination()
    {
        if (!rightControllerInput.smoothMotionEnabled)
        {
            teleportPoint.SetActive(false);
            teleportCanvas.SetActive(false);
            characterCanvas.SetActive(true);
            noteCanvas.SetActive(false);
            NPC.SetActive(true);
        }
    }

    public void Reached2ndDestination()
    {
        move2Canvas.SetActive(false);
        movePoint2.SetActive(false);
        binCanvas.SetActive(true);
    }

    public void CheckAmountCollected()
    {
        numWastesCollected = Inventory.instance.wastes.Count;
        if (numWastesCollected == 1 && !isCollected)
        {
            pickupCanvas.SetActive(false);
            dropCanvas.SetActive(false);
            move2Canvas.SetActive(true);
            movePoint2.SetActive(true);
            bin.SetActive(true);
            isCollected = true;
        }
    }

    public void AddScore(WasteType type, int points)
    {
        numWastesProcessed++;
        if (numWastesProcessed == 1)
        {
            binCanvas.SetActive(false);
            finishCanvas.SetActive(true);
            endTutorialSound.Play();
        }
    }

    public void MinusScore(WasteType type, int points)
    {
        numWastesProcessed--;
        if (numWastesProcessed == 1)
        {
            binCanvas.SetActive(false);
            finishCanvas.SetActive(true);
        }
    }

    public void IncrementCharactersTalkedTo()
    {
        if (!firstClick) return;

        characterCanvas.SetActive(false);
        wastesContainer.SetActive(true);
        pickupCanvas.SetActive(true);
        dropCanvas.SetActive(true);
        firstClick = false;
    }

    public void FinishDialogue()
    {
        if (!firstClick) return;

        characterCanvas.SetActive(false);
        wastesContainer.SetActive(true);
        pickupCanvas.SetActive(true);
        dropCanvas.SetActive(true);
        firstClick = false;
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

    void SwitchMovementMode()
    {
        switchCoolDownTimer += Time.deltaTime;
        if (switchCoolDownTimer < cooldownTime) return;

        if (mainButton.action.WasPressedThisFrame())
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

            switchCoolDownTimer = 0;
        }
    }

    void DisplayMovementCanvas(GameObject canvas)
    {
        walkingModeCanvas.GetComponent<TweenFadeOut>().ForceEnd();
        jumpingModeCanvas.GetComponent<TweenFadeOut>().ForceEnd();
        canvas.SetActive(true);
    }
}
