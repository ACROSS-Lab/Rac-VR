using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;


public class ManageScore : MonoBehaviour
{
   [SerializeField] private TMPro.TextMeshProUGUI gameScoreText;
    public InputDevice _leftController ;
    
    private int score = 0;

    public InputHelpers.Button button = InputHelpers.Button.PrimaryButton;



    private void Start()
    {
        if (!_leftController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref _leftController);

    }

    void Update()
    {
        float val;

       _leftController.TryReadSingleValue(button, out val);
        if (val > 0)
            ResetScore();

    }


    private void InitializeInputDevice(InputDeviceCharacteristics inputCharacteristics, ref InputDevice inputDevice)
    {
        List<InputDevice> devices = new List<InputDevice>();
        //Call InputDevices to see if it can find any devices with the characteristics we're looking for
        InputDevices.GetDevicesWithCharacteristics(inputCharacteristics, devices);

        //Our hands might not be active and so they will not be generated from the search.
        //We check if any devices are found here to avoid errors.
        if (devices.Count > 0)
        {
            inputDevice = devices[0];
        }
    }


   

    public void IncrementScore(){
       score++;
       gameScoreText.text = ""+score;
   } 
 
   public void ResetScore(){
       score = 0;
       gameScoreText.text = "0";
   }
}
