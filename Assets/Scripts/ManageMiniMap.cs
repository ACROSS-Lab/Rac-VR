using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class MiniMapManager : MonoBehaviour
{
    [SerializeField] private GameObject miniMap;
    public InputDevice _rightController;
   
  
    public InputHelpers.Button button = InputHelpers.Button.PrimaryButton;



    private void Start()
    {
        if (!_rightController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref _rightController);

    }

    void Update()
    {
        float val;

        _rightController.TryReadSingleValue(button, out val);
        if (val > 0)
            ActivateDesactivate();

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




    public void ActivateDesactivate()
    {
        miniMap.SetActive(!miniMap.activeSelf);
    }

    
}
