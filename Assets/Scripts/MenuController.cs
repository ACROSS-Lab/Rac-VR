
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour
{

    public Slider mainSlider;
    [SerializeField] private TMPro.TextMeshProUGUI gameDurationText;

    
    public void Start()
    {
        //Adds a listener to the main slider and invokes a method when the value changes.
        mainSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        gameDurationText.SetText("Game duration: " + mainSlider.value + "s");
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        PlayerPrefs.SetFloat("duration", mainSlider.value);
        gameDurationText.SetText("Game duration: " + mainSlider.value + "s");
    }

   
    public void StartUnlimited()
    {
        ManageSceneLoading.Instance.LoadScene("RAC_MainScene - Unlimited");
    }

    public void StartNormal()
    {

        ManageSceneLoading.Instance.LoadScene("RAC_Tuto");
    }
}
