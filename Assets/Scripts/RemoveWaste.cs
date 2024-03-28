using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class RemoveWaste : MonoBehaviour
{
	[SerializeField] private float durationSec = 120.0f;


	private float currentTime = 0.0f;
    private bool timerOn = false;
	private ManageScore scoreM;
	[SerializeField] private TMPro.TextMeshProUGUI gameInfotext;

	void Start()
   	{
		GameObject g = GameObject.FindWithTag("ScoreManager");
		if (g != null)
			scoreM = g.GetComponent<ManageScore>();
		else
			scoreM = null;
	}

	
	void reset() {
		timerOn = false;
		if (scoreM != null)
        {
			if (scoreM.resetWaste)
            {
				GetComponent<BoxCollider>().enabled = true;
				GetComponent<Renderer>().enabled = true;
			}
			GetComponent<PlayMakerFSM>().enabled = false;

		} else
        {

			gameInfotext.SetText("Đang tải môi trường!");
			SceneManager.LoadScene("RAC_MainScene - Normal");
		}
		

	}
	public void remove() {
		GetComponent<BoxCollider>().enabled = false;
		GetComponent<Renderer>().enabled = false;
		if (scoreM != null)
        {
			currentTime = durationSec;
			timerOn = true;
			scoreM.IncrementScore();
		} else
        {
			gameInfotext.SetText("Tốt lắm!\nBây giờ là lúc dọn dẹp ngôi làng!");
			timerOn = true; 
			currentTime = 2;
		}
		
	} 

    // Update is called once per frame
    void Update()
    {
	if (timerOn) {
		if (currentTime > 0.0f) {
        		currentTime -= Time.deltaTime;
		} else {
			reset();
		}
	}
    }
}
