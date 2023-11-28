using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageScore : MonoBehaviour
{
   [SerializeField] private TMPro.TextMeshProUGUI gameScoreText;
    
   private int score = 0;

  
   public void IncrementScore(){
       score++;
       gameScoreText.text = ""+score;
   } 

   public void ResetScore(){
       score = 0;
       gameScoreText.text = "0";
   }
}
