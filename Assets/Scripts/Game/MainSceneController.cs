using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class MainSceneController : MonoBehaviour
{
    [Header("Waste Presets")]
    public List<WastePreset> playPresets;

    [Header("Session Settings")]
    [SerializeField] float sessionTime = 300f;
    [SerializeField] Transform XRRigTransform;

    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI[] scoreTexts;
    [SerializeField] LocalizedKey[] typeKeyTexts;
    [SerializeField] GameObject objectiveCanvas;
    [SerializeField] GameObject startGamePanel;
    [SerializeField] GameObject endGamePanel;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] SlicedFilledImage loadingBarFill;
    [SerializeField] GameObject[] scoreContainers;
    [SerializeField] GameObject[] scoreChecks;

    [SerializeField] TextMeshProUGUI charactersTalkedTo;
    [SerializeField] GameObject characterScoreContainer;
    [SerializeField] GameObject characterScoreCheck;

    [Header("Movement Inputs")]
    [SerializeField] GameObject moveProvider;
    [SerializeField] GameObject teleportProvider;
    [SerializeField] InputActionReference mainButton;
    [SerializeField] InputActionReference secondaryButton;
    [SerializeField] ControllerInputActionManager rightControllerInput;

    [Header("Sound effects")]
    [SerializeField] AudioSource endSessionSound;
    [SerializeField] AudioSource completedObjectiveSound;

    float timer = 0;
    List<WastePreset> activePresets;
    List<GameObject> presetGameObjects;
    bool inGame = false;
    bool[] reachedScores = new bool[] { false, false };
    Vector3 initialXRRigPosition;
    bool isSwitchingMode;

    void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnScoreUpdated -= UpdateScoreUI;
            GameManager.instance.OnCharactersTalkedUpdated-= UpdateCharacterUI;
        }
    }

    void Start()
    {
        GameManager.instance.OnScoreUpdated += UpdateScoreUI;
        GameManager.instance.OnCharactersTalkedUpdated += UpdateCharacterUI;
        initialXRRigPosition = XRRigTransform.position;

        moveProvider.SetActive(false);
        teleportProvider.SetActive(false);
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

    void UpdateScoreUI(WasteType type, int newScore)
    {
        for (int i = 0; i < activePresets.Count; i++)
        {
            if (activePresets[i].wasteType == type)
            {
                scoreTexts[i].text = newScore.ToString();

                if (!reachedScores[i] && newScore >= 10)
                {
                    reachedScores[i] = true;
                    scoreChecks[i].SetActive(true);
                    scoreContainers[i].SetActive(false);
                    completedObjectiveSound.Play();
                }
                break;
            }
        }

        if (CheckWinCondition())
        {
            EndSession();
        }
    }

    void UpdateCharacterUI(int newCount)
    {
        charactersTalkedTo.text = newCount.ToString();

        if (newCount == 3)
        {
            characterScoreCheck.SetActive(true);
            characterScoreContainer.SetActive(false);
            completedObjectiveSound.Play();
        }

        if (CheckWinCondition())
        {
            EndSession();
        }
    }

    public void StartNextSession()
    {
        moveProvider.SetActive(true);
        teleportProvider.SetActive(true);

        activePresets = GameManager.instance.GetNextTwoPresets();

        if (activePresets.Count == 0)
        {
            GameManager.instance.ResetPresetIndex();
            activePresets = GameManager.instance.GetNextTwoPresets();
        }

        GameManager.instance.ResetSessionScore();
        presetGameObjects = new List<GameObject>();

        for (int i = 0; i < activePresets.Count; i++)
        {
            WastePreset preset = Instantiate(activePresets[i]);
            presetGameObjects.Add(preset.gameObject);
            GarbageTypeToString(preset, i);
            ActiveCharacter(preset.characters);
        }

        inGame = true;
                
        timer = sessionTime;
        XRRigTransform.position = initialXRRigPosition;

        startGamePanel.SetActive(false);
        endGamePanel.SetActive(false);
        objectiveCanvas.SetActive(true);

        for (int i = 0; i < scoreTexts.Length; i++)
        {
            reachedScores[i] = false;
            scoreChecks[i].SetActive(false);
            scoreContainers[i].SetActive(true);
        }
    }

    private void EndSession()
    {
        timer = 0;
        foreach (GameObject preset in presetGameObjects)
        {
            preset.SetActive(false);
        }
        Inventory.instance.ClearInventory();

        endGamePanel.SetActive(true);
        objectiveCanvas.SetActive(false);
        inGame = false;

        endSessionSound.Play();
    }

    void GarbageTypeToString(WastePreset wastePreset, int index)
    {
        var scores = GameManager.instance.wasteTypeScores;
        if (wastePreset.wasteType == WasteType.Recyclable)
        {
            scoreTexts[index].text = scores[WasteType.Recyclable].ToString();
        }
        else if (wastePreset.wasteType == WasteType.Organic)
        {
            scoreTexts[index].text = scores[WasteType.Organic].ToString();
        }
        else if (wastePreset.wasteType == WasteType.MetalPaper)
        {
            scoreTexts[index].text = scores[WasteType.MetalPaper].ToString();
        }
        else if (wastePreset.wasteType == WasteType.NonRecyclable)
        {
            scoreTexts[index].text = scores[WasteType.NonRecyclable].ToString();
        }
        
        typeKeyTexts[index].localizationKey = wastePreset.presetKey;
        typeKeyTexts[index].UpdateText();
    }

    bool CheckWinCondition()
    {
        foreach (bool reached in reachedScores)
        {
            if (!reached) return false;
        }

        if (GameManager.instance.characterTalked < 3) return false;

        return true;
    }

    void ActiveCharacter(Transform characterContainer)
    {
        GameObject[] characters = new GameObject[characterContainer.childCount];
        for (int i = 0; i < characterContainer.childCount; i++)
        {
            characters[i] = characterContainer.GetChild(i).gameObject;
            characters[i].SetActive(false); 
        }

        for (int i = 0; i < 2; i++)
        {
            if (characters.Length == 0) break; 

            int randomIndex = Random.Range(0, characters.Length);
            characters[randomIndex].SetActive(true);

            List<GameObject> tempList = new List<GameObject>(characters);
            tempList.RemoveAt(randomIndex);
            characters = tempList.ToArray();
        }
    }

    void SwitchMovementMode()
    {
        bool a = mainButton.action.IsPressed();
        bool b = secondaryButton.action.IsPressed();

        if (a && b)
        {
            if (!isSwitchingMode)
            {
                rightControllerInput.smoothMotionEnabled = !rightControllerInput.smoothMotionEnabled;
            }
            isSwitchingMode = true;
        }
        else
        {
            isSwitchingMode = false;
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

        AsyncOperation operation = SceneManager.LoadSceneAsync("RAC_Tuto_NonGP");
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