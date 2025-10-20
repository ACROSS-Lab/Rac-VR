using UnityEngine;

public class Bin : MonoBehaviour
{
    public WasteType binType;
    public GameObject plusScoreCanvas;
    public GameObject minusScoreCanvas;
    public ParticleSystem correctParticle;
    public ParticleSystem falseParticle;

    private void DisplayFeedback(GameObject feedbackCanvas)
    {
        if (feedbackCanvas.activeInHierarchy)
        {
            feedbackCanvas.GetComponent<TweenFadeOut>().ForceEnd();
        }
        feedbackCanvas.SetActive(true);
    }

    public void PlusScore(int score)
    {
        GameManager.instance.AddScore(score);
        DisplayFeedback(plusScoreCanvas);
        if (correctParticle.isPlaying) correctParticle.Stop();
        correctParticle.Play();
    }
    
    public void MinusScore(int score)
    {
        GameManager.instance.MinusScore(score);
        DisplayFeedback(minusScoreCanvas);
        if (falseParticle.isPlaying) falseParticle.Stop();
        falseParticle.Play();
    }
}