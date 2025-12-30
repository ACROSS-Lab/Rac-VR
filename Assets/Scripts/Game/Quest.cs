using TMPro;
using UnityEngine;

public class Quest : MonoBehaviour
{
    [SerializeField] LocalizedKey descriptionKey;
    [SerializeField] TextMeshProUGUI remainingText;
    [SerializeField] TextMeshProUGUI correctText;
    [SerializeField] TextMeshProUGUI incorrectText;
    [SerializeField] GameObject remainingUI;
    [SerializeField] GameObject finishUI;

    public int remaining {get; private set;}
    int correct = 0;
    int incorrect = 0;

    public void Setup(GameObject trashes, string desKey)
    {
        descriptionKey.localizationKey = desKey;

        Waste[] wastes = trashes.GetComponentsInChildren<Waste>();
        foreach (Waste waste in wastes)
        {
            waste.quest = this;
        }

        remaining = wastes.Length;
        remainingText.text = remaining.ToString();

        gameObject.SetActive(true);
        trashes.SetActive(true);
    }

    public void DecreaseRemaining()
    {
        remaining--;
        if (remaining == 0)
        {
            Game.Manager.CompleteQuest(this);
            remainingUI.SetActive(false);
            finishUI.SetActive(true);
        }
        else
        {
            remainingText.text = remaining.ToString();
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