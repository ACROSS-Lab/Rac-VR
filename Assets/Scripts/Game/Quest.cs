using TMPro;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public int ID;
    [SerializeField] GameObject trashes;
    [SerializeField] TextMeshProUGUI remainingText;
    [SerializeField] TextMeshProUGUI correctText;
    [SerializeField] TextMeshProUGUI incorrectText;

    public int remaining {get; private set;}
    int correct = 0;
    int incorrect = 0;

    void Start()
    {
        Waste[] wastes = trashes.GetComponentsInChildren<Waste>();
        foreach (Waste waste in wastes)
        {
            waste.questID = ID;
        }

        remaining = wastes.Length;
        remainingText.text = remaining.ToString();

        trashes.SetActive(true);
    }

    public void DecreaseRemaining()
    {
        remaining--;
        remainingText.text = remaining.ToString();

        if (remaining == 0)
        {
            Game.Manager.CompleteQuest(this);
        }
    }
    public void IncreaseCorrect()
    {
        correct++;
        correctText.text = correct.ToString();
    }
    public void IncreaseIncorrect()
    {
        incorrect++;
        incorrectText.text = incorrect.ToString();
    }
}