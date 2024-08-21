using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;


public class ManageScore : MonoBehaviour
{
   [SerializeField] private TMPro.TextMeshProUGUI gameScoreText;
    public InputDevice _leftController ;

    public bool resetWaste = true;

    private int score = 0;

    public static ManageScore Instance = null; 

    public GameObject XROrigin;  

   private void Awake()
    {
        Instance = this;
        XROrigin = GameObject.FindGameObjectWithTag("Player");

    }

    private void Start()
    {
        if (!_leftController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref _leftController);

       
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


    public void sendInformation()
    {
        Dictionary<string, string> args = new Dictionary<string, string> {
            {"id",ConnectionManager.Instance.getUseMiddleware() ? ConnectionManager.Instance.GetConnectionId()  : ("\"" + ConnectionManager.Instance.GetConnectionId() +  "\"") },
            {"score", "" +score}
        };


        ConnectionManager.Instance.SendExecutableAsk("update_score", args);

    }


    public void IncrementScore(){
       score++;
       gameScoreText.text = ""+score;

        PlayerPrefs.SetFloat("score", score);
        sendInformation();

   } 
 
   public void ResetScore(){
       score = 0;
       gameScoreText.text = "0";
   }
}




