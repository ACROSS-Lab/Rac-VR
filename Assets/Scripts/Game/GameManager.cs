using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector] public int score = 0;
    
    [SerializeField] TextMeshProUGUI scoreText;

    float timer = 120f;
    
    void Awake()
    {
        instance = this;
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    public void MinusScore(int points)
    {
        score -= points;
        if (score < 0) score = 0;
        scoreText.text = score.ToString();
    }
}