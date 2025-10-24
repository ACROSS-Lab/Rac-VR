using UnityEngine;

public class Bin : MonoBehaviour
{
    public WasteType binType;
    [SerializeField] GameObject plusScoreCanvas;
    [SerializeField] GameObject minusScoreCanvas;
    [SerializeField] ParticleSystem correctParticle;
    [SerializeField] ParticleSystem falseParticle;
    [SerializeField] AudioClip correctSound;
    [SerializeField] AudioClip falseSound;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void DisplayFeedback(GameObject feedbackCanvas)
    {
        if (feedbackCanvas.activeInHierarchy)
        {
            feedbackCanvas.GetComponent<TweenFadeOut>().ForceEnd();
        }
        feedbackCanvas.SetActive(true);
    }

    public void CorrectBin(WasteType wasteType, int score)
    {
        GameManager.instance.AddScore(wasteType, score);

        if (correctParticle.isPlaying) correctParticle.Stop();
        correctParticle.Play();

        if (audioSource.isPlaying) audioSource.Stop();
        if (correctSound != null)
        {
            audioSource.clip = correctSound;
            audioSource.Play();
        }
    }
    
    public void WrongBin()
    {
        if (falseParticle.isPlaying) falseParticle.Stop();
        falseParticle.Play();

        if (audioSource.isPlaying) audioSource.Stop();
        if (falseSound != null)
        {
            audioSource.clip = falseSound;
            audioSource.Play();
        }
    }
}