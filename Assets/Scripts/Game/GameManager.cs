using TMPro;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<GameObject> playPresets;
    [HideInInspector] public int score = 0;

    [SerializeField] float timer = 120f;

    [SerializeField] Transform XRRigTransform;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject startGamePanel;
    [SerializeField] GameObject endGamePanel;
    [SerializeField] TextMeshProUGUI finalScoreText;

    List<GameObject> shuffledPresets = new List<GameObject>();
    List<GameObject> activePresets = new List<GameObject>();
    bool inGame = false;
    Vector3 initialXRRigPosition;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        shuffledPresets = playPresets.OrderBy(x => Random.value).ToList();
        startGamePanel.SetActive(true);
        initialXRRigPosition = XRRigTransform.position;
    }

    void Update()
    {
        if(inGame)
        {
            timer -= Time.deltaTime;
            System.TimeSpan time = System.TimeSpan.FromSeconds(timer);
            timerText.text = time.ToString("mm\\:ss");
            if(timer <= 0)
            {
                EndSession();
            }
        }
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

    public void StartNextSession()
    {
        if(shuffledPresets.Count == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        activePresets = new List<GameObject> { shuffledPresets[0], shuffledPresets[1] };
        foreach (GameObject preset in activePresets)
        {
            preset.SetActive(true);
        }
        
        shuffledPresets.RemoveRange(0, 2);

        inGame = true;
        timer = 120f;

        startGamePanel.SetActive(false);
        endGamePanel.SetActive(false);
        scoreText.transform.parent.parent.gameObject.SetActive(true);
        timerText.transform.parent.parent.gameObject.SetActive(true);

        XRRigTransform.position = initialXRRigPosition;
    }

    private void EndSession()
    {
        inGame = false;
        timer = 0;
        foreach (GameObject preset in activePresets)
        {
            preset.SetActive(false);
        }
        Inventory.instance.ClearInventory();

        endGamePanel.SetActive(true);
        finalScoreText.text = score.ToString();
        scoreText.transform.parent.parent.gameObject.SetActive(false);
        timerText.transform.parent.parent.gameObject.SetActive(false);
    }
}