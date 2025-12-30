using UnityEngine;

public class Bin : MonoBehaviour
{
    public WasteType binType;
    [SerializeField] GameObject plusScoreCanvas;
    [SerializeField] GameObject minusScoreCanvas;
    [SerializeField] ParticleSystem correctParticle;
    [SerializeField] ParticleSystem falseParticle;

    [Header("Sound effects")]
    [SerializeField] AudioClip[] correctSounds;
    [SerializeField] AudioClip[] falseSounds;
    [SerializeField][Range(0.5f, 2f)] float minPitch = 0.9f;
    [SerializeField][Range(0.5f, 2f)] float maxPitch = 1.1f;

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

    public void CorrectBin(Quest quest)
    {
        Game.Manager.AddScore(quest);

        if (correctParticle.isPlaying) correctParticle.Stop();
        correctParticle.Play();

        if (audioSource.isPlaying) audioSource.Stop();
        if (correctSounds != null)
        {
            audioSource.clip = correctSounds[Random.Range(0, correctSounds.Length)];
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.Play();
        }
    }
    
    public void WrongBin(Quest quest)
    {
        Game.Manager.MinusScore(quest);

        if (falseParticle.isPlaying) falseParticle.Stop();
        falseParticle.Play();

        if (audioSource.isPlaying) audioSource.Stop();
        if (falseSounds.Length > 0)
        {
            audioSource.clip = falseSounds[Random.Range(0, falseSounds.Length)];
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.Play();
        }
    }
}