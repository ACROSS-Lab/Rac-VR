using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateDisplay : MonoBehaviour
{

    public string texttoDisplay = "";
    [SerializeField] private TMPro.TextMeshProUGUI gameStateText;
     
    void Start()
    {
        gameStateText.text = GameManager.Instance.GetCurrentState().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        gameStateText.text =  GameManager.Instance.GetCurrentState().ToString() + " \n " + texttoDisplay;

    }
}
