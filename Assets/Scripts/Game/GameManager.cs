using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    [Header("Haptics")]
    [SerializeField] HapticImpulsePlayer leftHapticPlayer;
    [SerializeField] HapticImpulsePlayer rightHapticPlayer;
    [SerializeField] float hapticAmplitude = 0.5f;
    [SerializeField] float hapticDuration = 0.2f;

    int characterTalked = 0;
    float timer = 0;
    public List<WastePreset> activePresets;
    bool inGame = false;
    bool[] reachedScores = new bool[] {false, false};
    Vector3 initialXRRigPosition;
    Dictionary<WasteType, int> wasteTypeScores = new Dictionary<WasteType, int>()
    {
        { WasteType.Recyclable, 0 },
        { WasteType.Organic, 0 },
        { WasteType.MetalPaper, 0 },
        { WasteType.NonRecyclable, 0 },
    };

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        startGamePanel.SetActive(true);
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

    public void AddScore(WasteType type, int points)
    {
        wasteTypeScores[type] += points;

        int newScore = wasteTypeScores[type];

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
    // public void MinusScore(int points)
    // {
    //     score -= points;
    //     if (score < 0) score = 0;
    // }

    public void StartNextSession()
    {
        if(playPresets.Count == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        activePresets = new List<WastePreset> {playPresets[0], playPresets[1]};
        for (int i = 0; i < activePresets.Count; i++)
        {
            activePresets[i].gameObject.SetActive(true);
            GarbageTypeToString(activePresets[i], i);
            ActiveCharacter(activePresets[i].characters);
        }
                
        playPresets.RemoveRange(0, 2);

        inGame = true;
        timer = sessionTime;

        startGamePanel.SetActive(false);
        endGamePanel.SetActive(false);
        objectiveCanvas.SetActive(true);

        XRRigTransform.position = initialXRRigPosition;

        for (int i = 0; i < scoreTexts.Length; i++)
        {
            reachedScores[i] = false;
            scoreChecks[i].SetActive(false);
            scoreContainers[i].SetActive(true);
        }

        characterTalked = 0;
        charactersTalkedTo.text = characterTalked.ToString();
        characterScoreCheck.SetActive(false);
        characterScoreContainer.SetActive(true);
    }

    private void EndSession()
    {
        inGame = false;
        timer = 0;
        foreach (WastePreset preset in activePresets)
        {
            preset.gameObject.SetActive(false);
        }
        Inventory.instance.ClearInventory();

        endGamePanel.SetActive(true);
        objectiveCanvas.SetActive(false);
    }

    void GarbageTypeToString(WastePreset wastePreset, int index)
    {
        if (wastePreset.wasteType == WasteType.Recyclable)
        {
            scoreTexts[index].text = wasteTypeScores[WasteType.Recyclable].ToString();
        }
        else if (wastePreset.wasteType == WasteType.NonRecyclable)
        {
            scoreTexts[index].text = wasteTypeScores[WasteType.NonRecyclable].ToString();
        } 
        else if (wastePreset.wasteType == WasteType.MetalPaper)
        {
            scoreTexts[index].text = wasteTypeScores[WasteType.MetalPaper].ToString();
        }
        else if (wastePreset.wasteType == WasteType.Organic)
        {
            scoreTexts[index].text = wasteTypeScores[WasteType.Organic].ToString();
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

        if (characterTalked < 3) return false;

        return true;
    }

    void ActiveCharacter(Transform characterContainer)
    {
        GameObject[] characters = new GameObject[characterContainer.childCount];
        for (int i = 0; i < characterContainer.childCount; i++)
        {
            characters[i] = characterContainer.GetChild(i).gameObject;
        }

        for (int i = 0; i < 2; i++)
        {
            int randomIndex = Random.Range(0, characters.Length);
            characters[randomIndex].SetActive(true);

            List<GameObject> tempList = new List<GameObject>(characters);
            tempList.RemoveAt(randomIndex);
            characters = tempList.ToArray();
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

    public void IncrementCharactersTalkedTo()
    {
        characterTalked++;
        charactersTalkedTo.text = characterTalked.ToString();

        if (characterTalked >= 3)
        {
            characterScoreCheck.SetActive(true);
            characterScoreContainer.SetActive(false);
        }

        if (CheckWinCondition())
        {
            EndSession();
        }
    }
}