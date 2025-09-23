using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManageSceneLoading : MonoBehaviour
{
    public Slider progressSlider;
    public GameObject loadingScreen;
    public GameObject toDisable;
    [SerializeField] private TMPro.TextMeshProUGUI progressText;

    static public ManageSceneLoading Instance;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

  
    public void LoadScene(string scene)
    {
        StartCoroutine(loadASync(scene));
    }

    IEnumerator loadASync(string scene)
    {

        toDisable.SetActive(false);
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

       
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressText.SetText("" + Mathf.Round(progress * 100)  + "%");
            progressSlider.value = progress;

            yield return null;
        }

    }

}
