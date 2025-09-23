using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class EndingMenuController : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI gameScoretext;
    [SerializeField] private TMPro.TextMeshProUGUI gameBestScoretext;

    // Start is called before the first frame update
    void Start()
    {
        float score = PlayerPrefs.GetFloat("score");
       /* string time = "" + (PlayerPrefs.GetFloat("duration"));
        float bestScore = PlayerPrefs.HasKey(time) ? PlayerPrefs.GetFloat(time) : score;
        if (score >= bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetFloat(time, bestScore);

        }*/

       
        gameScoretext.SetText("Déchets ramassés: " + score);
      //  gameBestScoretext.SetText("Số điểm cao nhất: " + bestScore);
    }

    public void Restart()
    {

        SceneManager.LoadScene("RAC_Tuto");
    }
}
