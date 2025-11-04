using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class SendingHaptics : MonoBehaviour
{
    public static SendingHaptics instance;

    [SerializeField] HapticImpulsePlayer leftHapticPlayer;
    [SerializeField] HapticImpulsePlayer rightHapticPlayer;
    [SerializeField] float hapticAmplitude = 0.5f;
    [SerializeField] float hapticDuration = 0.2f;

    void Awake()
    {
        instance = this;
    }

    public void SendLeftHaptic()
    {
        leftHapticPlayer.SendHapticImpulse(hapticAmplitude, hapticDuration);
    }

    public void SendRightHaptic()
    {
        rightHapticPlayer.SendHapticImpulse(hapticAmplitude, hapticDuration);
    }
}