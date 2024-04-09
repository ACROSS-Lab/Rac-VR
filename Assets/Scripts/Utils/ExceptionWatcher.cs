using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

using UnityEngine.SceneManagement;

public class ExceptionWatcher : MonoBehaviour
{
    private XRInteractionManager interactionManager;
    public float hideDuration = 0.1f; // Durée pendant laquelle l'objet sera caché

    private void Start()
    {
        // Récupération du GameObject avec XRInteractionManager
        interactionManager = FindObjectOfType<XRInteractionManager>();
        if (interactionManager == null)
        {
            Debug.LogWarning("Aucun XRInteractionManager trouvé dans la scène.");
            return;
        }

        Application.logMessageReceived += HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {

        if (type == LogType.Exception &&
            logString.Contains("NullReferenceException") &&
            stackTrace.Contains("UnityEngine.InputSystem.InputActionState.ApplyProcessors[TValue]"))
        {
            if (interactionManager != null)
            {
                StartCoroutine(HideObjectTemporarily(interactionManager.gameObject, hideDuration));
            }
        }
    }

    IEnumerator HideObjectTemporarily(GameObject obj, float duration)
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        yield return new WaitForSeconds(duration);
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }
}
