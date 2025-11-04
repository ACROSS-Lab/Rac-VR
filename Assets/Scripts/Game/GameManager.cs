using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IGameManager
{
    public static GameManager instance;

    public Dictionary<WasteType, int> wasteTypeScores { get; private set; }
    public int characterTalked { get; private set; }

    public event Action<WasteType, int> OnScoreUpdated;
    public event Action<int> OnCharactersTalkedUpdated;

    [SerializeField] string sceneName = "RAC_MainScene_NonGP";

    [Header("Session Presets")]
    [SerializeField] List<WastePreset> allPlayPresets;
    int currentPresetIndex = 0;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            Game.RegisterManager(this);
            SceneManager.sceneLoaded += OnSceneLoaded;

            wasteTypeScores = new Dictionary<WasteType, int>()
            {
                { WasteType.Recyclable, 0 },
                { WasteType.Organic, 0 },
                { WasteType.MetalPaper, 0 },
                { WasteType.NonRecyclable, 0 },
            };
            characterTalked = 0;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == sceneName)
        {
            Game.RegisterManager(this);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    public List<WastePreset> GetNextTwoPresets()
{
    if (currentPresetIndex + 1 < allPlayPresets.Count)
    {
        List<WastePreset> nextPresets = new List<WastePreset>
        {
            allPlayPresets[currentPresetIndex],
            allPlayPresets[currentPresetIndex + 1]
        };

        currentPresetIndex += 2;
        return nextPresets;
    }
    else
    {
        Debug.LogWarning("No more presets available.");
        return new List<WastePreset>();
    }
}

    public void ResetPresetIndex()
    {
        currentPresetIndex = 0;
    }

    public void ResetSessionScore()
    {
        characterTalked = 0;
        wasteTypeScores[WasteType.Recyclable] = 0;
        wasteTypeScores[WasteType.Organic] = 0;
        wasteTypeScores[WasteType.MetalPaper] = 0;
        wasteTypeScores[WasteType.NonRecyclable] = 0;
        
        OnCharactersTalkedUpdated?.Invoke(characterTalked);
        foreach (var pair in wasteTypeScores)
        {
            OnScoreUpdated?.Invoke(pair.Key, pair.Value);
        }
    }

    public void AddScore(WasteType type, int points)
    {
        wasteTypeScores[type] += points;
        OnScoreUpdated?.Invoke(type, wasteTypeScores[type]);
    }

    public void IncrementCharactersTalkedTo()
    {
        characterTalked++;
        OnCharactersTalkedUpdated?.Invoke(characterTalked);
    }
}