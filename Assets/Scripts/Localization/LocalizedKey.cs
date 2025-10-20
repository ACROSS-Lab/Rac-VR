using UnityEngine;
using TMPro;

public class LocalizedKey : MonoBehaviour
{
    public string localizationKey;
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public TextMeshProUGUI textComponent;

    private void Start()
    {
        if (textComponent == null) GetComponent<TextMeshProUGUI>();

        UpdateText();
        LocalizationManager.OnLanguageChanged += UpdateText;

        if (audioSource != null)
        {
            UpdateAudioClip();
            LocalizationManager.OnLanguageChanged += UpdateAudioClip;
        }
    }

    private void OnDestroy()
    {
        LocalizationManager.OnLanguageChanged -= UpdateText;
        if (audioSource != null)
        {
            LocalizationManager.OnLanguageChanged -= UpdateAudioClip;
        }
    }

    private void UpdateText()
    {
        if (string.IsNullOrEmpty(localizationKey)) return;

        if (textComponent != null)
        {
            textComponent.text = LocalizationManager.Instance.GetLocalizedValue(localizationKey);
        }
    }

    private void UpdateAudioClip()
    {
        if (string.IsNullOrEmpty(localizationKey)) return;

        string currentLanguage = LocalizationManager.Instance.GetCurrentLanguage();
        string audioClipPath = $"Localization/Audio/{currentLanguage}/{localizationKey}";

        AudioClip loadedClip = Resources.Load<AudioClip>(audioClipPath);

        if (loadedClip != null)
        {
            audioSource.clip = loadedClip;
            Debug.Log("loaded Clip");
        }
        else
        {
            Debug.LogWarning($"AudioClip not found at path: '{audioClipPath}'");
        }
    }
}