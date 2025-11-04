using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using UnityEngine.InputSystem;

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
    [SerializeField] GameObject[] scoreContainers;
    [SerializeField] GameObject[] scoreChecks;

    [SerializeField] TextMeshProUGUI charactersTalkedTo;
    [SerializeField] GameObject characterScoreContainer;
    [SerializeField] GameObject characterScoreCheck;

    float timer = 0;
    List<WastePreset> activePresets;
    bool inGame = false;
    bool[] reachedScores = new bool[] { false, false };
    Vector3 initialXRRigPosition;

    void Awake()
    {
        
    }

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

        if (newCount >= 3)
        {
            characterScoreCheck.SetActive(true);
            characterScoreContainer.SetActive(false);
        }

        if (CheckWinCondition())
        {
            EndSession();
        }
    }

    public void StartNextSession()
    {
        activePresets = GameManager.instance.GetNextTwoPresets();

        if (activePresets.Count == 0)
        {
            Debug.Log("All sessions complete! Restarting game or loading main menu.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        GameManager.instance.ResetSessionScore();

        for (int i = 0; i < activePresets.Count; i++)
        {
            Instantiate(activePresets[i]);
            GarbageTypeToString(activePresets[i], i);
            ActiveCharacter(activePresets[i].characters);
        }
                
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
        foreach (WastePreset preset in activePresets)
        {
            preset.gameObject.SetActive(false);
        }
        Inventory.instance.ClearInventory();

        endGamePanel.SetActive(true);
        objectiveCanvas.SetActive(false);
        inGame = false;
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
    
    public void LoadTutorialScene()
    {
        SceneManager.LoadScene("RAC_Tuto_NonGP");
    }
}