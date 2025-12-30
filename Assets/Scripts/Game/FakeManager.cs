using System.Collections.Generic;
using UnityEngine;

public class FakeManager : MonoBehaviour, IGameManager
{
    [SerializeField] AudioSource completeSound;
    [SerializeField] int totalQuestCount = 2;
    [SerializeField] GameObject questCanvas;

    Dictionary<Quest, bool> activeQuests = new Dictionary<Quest, bool>();
    int questCount = 0;

    void Awake()
    {
        Game.RegisterManager(this);
    }

    void Start()
    {
        
    }

    public void AddScore(Quest quest)
    {
        Quest q = quest;
        q.DecreaseRemaining();
        q.IncreaseCorrect();

        CheckCompleteAllQuests();
    }

    public void MinusScore(Quest quest)
    {
        Quest q = quest;
        q.DecreaseRemaining();
        q.IncreaseIncorrect();

        CheckCompleteAllQuests();
    }

    public void AddQuest(Quest questPrefab, GameObject wastes, string desKey)
    {
        if (!questCanvas.activeInHierarchy) questCanvas.SetActive(true);

        Transform backgroundQuest = questCanvas.transform.GetChild(0);        
        Quest quest = Instantiate(questPrefab, backgroundQuest, false);
        quest.Setup(wastes, desKey);

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