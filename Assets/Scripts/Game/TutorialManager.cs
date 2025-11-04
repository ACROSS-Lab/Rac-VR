using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
        Debug.Log("Added " + points + " points to " + type.ToString());
        numWastesProcessed++;
        if (numWastesProcessed == 5)
        {
            binCanvas.SetActive(false);
            finishCanvas.SetActive(true);
        }
    }

    public void MinusScore(WasteType type, int points)
    {
        Debug.Log("Subtracted " + points + " points from " + type.ToString());
        numWastesProcessed++;
        if (numWastesProcessed == 5)
        {
            binCanvas.SetActive(false);
            finishCanvas.SetActive(true);
        }
    }

    public void IncrementCharactersTalkedTo()
    {
        characterCanvas.SetActive(false);
        wastesContainer.SetActive(true);
        pickupCanvas.SetActive(true);
        dropCanvas.SetActive(true);
    }
    
    public void LoadMainGameScene()
    {
        SceneManager.LoadScene("RAC_MainScene_NonGP");
    }
}
