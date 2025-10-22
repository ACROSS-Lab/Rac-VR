using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Waste Presets")]
    public List<GameObject> playPresets;

    [Header("Session Settings")]
    [SerializeField] float sessionTime = 300f;
    [SerializeField] Transform XRRigTransform;

    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI[] scoreTexts;
    [SerializeField] TextMeshProUGUI[] typeTexts;
    [SerializeField] GameObject scoreCanvas;
    [SerializeField] GameObject startGamePanel;
    [SerializeField] GameObject endGamePanel;
    [SerializeField] GameObject[] scoreContainers;
    [SerializeField] GameObject[] checks;

    [Header("Haptics")]
    [SerializeField] HapticImpulsePlayer leftHapticPlayer;
    [SerializeField] HapticImpulsePlayer rightHapticPlayer;
    [SerializeField] float hapticAmplitude = 0.5f;
    [SerializeField] float hapticDuration = 0.2f;

    float timer = 0;
    List<GameObject> activePresets = new List<GameObject>();
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
        if(inGame)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                EndSession();
            }
        }
    }

    public void AddScore(WasteType type, int points)
    {
        wasteTypeScores[type] += points;

        for (int i = 0; i < activePresets.Count; i++)
        {
            string presetName = activePresets[i].name;
            Debug.Log(presetName);
            if (presetName.Contains("Recyclable") && type == WasteType.Recyclable)
            {
                scoreTexts[i].text = wasteTypeScores[type].ToString();
                break;
            }
            else if (presetName.Contains("Non_Recyclable") && type == WasteType.NonRecyclable)
            {
                scoreTexts[i].text = wasteTypeScores[type].ToString();
                break;
            }
            else if (presetName.Contains("Metal_Paper") && type == WasteType.MetalPaper)
            {
                scoreTexts[i].text = wasteTypeScores[type].ToString();
                break;
            }
            else if (presetName.Contains("Organic") && type == WasteType.Organic)
            {
                scoreTexts[i].text = wasteTypeScores[type].ToString();
                break;
            }
        }

        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (int.Parse(scoreTexts[i].text) >= 10 && !reachedScores[i])
            {
                reachedScores[i] = true;
                checks[i].SetActive(true);
                scoreContainers[i].SetActive(false);
            }
        }
        
        if(reachedScores[0] && reachedScores[1])
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

        activePresets = new List<GameObject> { playPresets[0], playPresets[1] };
        for(int i = 0; i < activePresets.Count; i++)
        {
            activePresets[i].SetActive(true);
            GarbageTypeToString(activePresets[i].name, i);
        }
        
        playPresets.RemoveRange(0, 2);

        inGame = true;
        timer = sessionTime;

        startGamePanel.SetActive(false);
        endGamePanel.SetActive(false);
        scoreCanvas.SetActive(true);

        XRRigTransform.position = initialXRRigPosition;

        for (int i = 0; i < scoreTexts.Length; i++)
        {    
            reachedScores[i] = false;
            checks[i].SetActive(false);
            scoreContainers[i].SetActive(true);
        }
    }

    private void EndSession()
    {
        inGame = false;
        timer = 0;
        foreach (GameObject preset in activePresets)
        {
            preset.SetActive(false);
        }
        Inventory.instance.ClearInventory();

        endGamePanel.SetActive(true);
        scoreCanvas.SetActive(false);
    }

    void GarbageTypeToString(string presetName, int index)
    {
        if (presetName.Contains("Recyclable"))
        {
            typeTexts[index].text = "Plastique recyclable";
            scoreTexts[index].text = wasteTypeScores[WasteType.Recyclable].ToString();
        }
        else if (presetName.Contains("Non_Recyclable"))
        {
            typeTexts[index].text = "Plastique non recyclable";
            scoreTexts[index].text = wasteTypeScores[WasteType.NonRecyclable].ToString();
        }
        else if (presetName.Contains("Metal_Paper"))
        {
            typeTexts[index].text = "Papier/Métal";
            scoreTexts[index].text = wasteTypeScores[WasteType.MetalPaper].ToString();
        }
        else if (presetName.Contains("Organic"))
        {
            typeTexts[index].text = "Organique";
            scoreTexts[index].text = wasteTypeScores[WasteType.Organic].ToString();
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
}