using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [Header("Tutorial Elements")]
    [SerializeField] GameObject movePoint;
    [SerializeField] GameObject wastesContainer;
    [SerializeField] GameObject binContainer;

    [Header("UI Elements")]
    [SerializeField] GameObject moveCanvas;
    [SerializeField] GameObject pickupCanvas;
    [SerializeField] GameObject dropCanvas;
    [SerializeField] GameObject characterCanvas;
    [SerializeField] GameObject binCanvas;
    [SerializeField] GameObject finishCanvas;

    [Header("Haptics")]
    [SerializeField] HapticImpulsePlayer leftHapticPlayer;
    [SerializeField] HapticImpulsePlayer rightHapticPlayer;
    [SerializeField] float hapticAmplitude = 0.5f;
    [SerializeField] float hapticDuration = 0.2f;

    int numWastesCollected = 0;
    int numWastesProcessed = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void MoveToDestination()
    {
        movePoint.SetActive(false);
        moveCanvas.SetActive(false);
        wastesContainer.SetActive(true);
        pickupCanvas.SetActive(true);
        dropCanvas.SetActive(true);
    }

    public void IncrementWastesCollected()
    {
        numWastesCollected++;
        if (numWastesCollected == 5)
        {
            pickupCanvas.SetActive(false);
            dropCanvas.SetActive(false);
            binCanvas.SetActive(true);
            binContainer.SetActive(true);
        }
    }

    public void IncrementWastesProcessed()
    {
        numWastesProcessed++;
        if (numWastesProcessed == 5)
        {
            binCanvas.SetActive(false);
            finishCanvas.SetActive(true);
        }
    }
    
    public void SendLeftHaptic()
    {
        leftHapticPlayer.SendHapticImpulse(hapticAmplitude, hapticDuration);
    }

    public void SendRightHaptic()
    {
        rightHapticPlayer.SendHapticImpulse(hapticAmplitude, hapticDuration);
    }
    
    public void LoadMainGameScene()
    {
        GlobalState.IsInTutorial = false;
        SceneManager.LoadScene("RAC_MainScene_NonGP");
    }
}
