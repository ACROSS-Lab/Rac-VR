using System.Collections.Generic;
using UnityEngine;

public class FakeManager : MonoBehaviour, IGameManager
{
    [SerializeField] AudioSource completeSound;
    [SerializeField] int totalQuestCount = 2;

    Dictionary<Quest, bool> activeQuests = new Dictionary<Quest, bool>();
    int questCount = 0;

    void Awake()
    {
        Game.RegisterManager(this);
    }

    void Start()
    {
        
    }

    public void AddScore(int ID)
    {
        Quest quest = GetQuest(ID);
        quest.DecreaseRemaining();
        quest.IncreaseCorrect();

        CheckCompleteAllQuests();
    }

    public void MinusScore(int ID)
    {
        Quest quest = GetQuest(ID);
        quest.DecreaseRemaining();
        quest.IncreaseIncorrect();

        CheckCompleteAllQuests();
    }

    Quest GetQuest(int ID)
    {
        foreach (Quest quest in activeQuests.Keys)
        {
            if (quest.ID == ID) return quest;
        }

        Debug.Log("There is no quest for this ID");
        return null;
    }

    public void AddQuest(Quest quest)
    {
        activeQuests.Add(quest, false);
    }

    public void CompleteQuest(Quest quest)
    {
        activeQuests[quest] = true;
        completeSound.Play();
        questCount++;
    }

    void CheckCompleteAllQuests()
    {
        if (questCount < totalQuestCount) return;

        foreach (KeyValuePair<Quest, bool> kvp in activeQuests)
        {
            if (!kvp.Value) return;
        }

        Debug.Log ("Complete all quests");
    }
}