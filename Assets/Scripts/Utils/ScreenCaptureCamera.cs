using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenCaptureCamera : MonoBehaviour
{
    [SerializeField] string fileName;
    [SerializeField] [Range(1, 10)] int quality;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ScreenCapture.CaptureScreenshot(Application.dataPath + "/" + fileName + ".png", quality);
            Debug.Log("Screenshot taken at " + Application.dataPath + "/" + fileName + ".png");
        }
    }
}