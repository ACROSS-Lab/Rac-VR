using System.Collections.Generic;
using UnityEngine;

public class FakeManager : MonoBehaviour, IGameManager
{
    [SerializeField] GameObject[] piles;

    Dictionary<int, List<Waste>> pilesWastes = new Dictionary<int, List<Waste>>();

    void Awake()
    {
        Game.RegisterManager(this);
    }

    void Start()
    {
        
    }

    public void AddScore(WasteType type, int points)
    {
        Debug.Log("FakeManager: AddScore");
    }

    public void MinusScore(WasteType type, int points)
    {
        Debug.Log("FakeManager: MinusScore");
    }

    public void IncrementCharactersTalkedTo()
    {
        Debug.Log("FakeManager: IncrementCharactersTalkedTo");
    }
}