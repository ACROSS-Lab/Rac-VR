using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class MiniMapManager : MonoBehaviour
{
    [SerializeField] private GameObject miniMap;
    [SerializeField] private GameObject player;

    [SerializeField] private GameObject playerRepresentation;
    public InputDevice _rightController;

    [SerializeField] private float delayButton = 1.0f;

    private float lastPush = 0.0f;
    public InputHelpers.Button button = InputHelpers.Button.PrimaryButton;



    private void Start()
    {
        if (!_rightController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref _rightController);

    }

    void Update()
    {
        float val;
        lastPush += Time.deltaTime;
        _rightController.TryReadSingleValue(button, out val);
        if ((val > 0) && (lastPush > delayButton))
            ActivateDesactivate();
        if (miniMap.activeSelf)
        {
            Vector3 pos = player.transform.position;
            float x = pos.x * 13.6f + 319.6f - 750f/2f;
            float y =  -1 * (pos.z * (-13.0f) + 112.1f);
            playerRepresentation.transform.localPosition = new Vector3(x,y,0);
        }

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
        lastPush = 0.0f;
        miniMap.SetActive(!miniMap.activeSelf);
      
    }

    
}
