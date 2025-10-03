using UnityEngine;

public class CharacterDialouge : MonoBehaviour
{
    [SerializeField] Transform camTransform;
    [SerializeField] Transform playerTransform;
    [SerializeField] GameObject canvasDialogue;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float distanceToDisplay = 5f;

    bool isInZone = false;

    void Start()
    {
        if (camTransform == null && Camera.main != null)
            camTransform = Camera.main.transform;
    }

    void Update()
    {
        RotateTowardsCamera();
        DisplayDialouge();
    }

    void RotateTowardsCamera()
    {
        if (camTransform == null || canvasDialogue == null)
            return;

        Vector3 direction = canvasDialogue.transform.position - camTransform.position;
        direction.y = 0;
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            canvasDialogue.transform.rotation = targetRotation;
        }
    }

    void DisplayDialouge()
    {
        if (canvasDialogue == null || playerTransform == null)
            return;

        float distance = Vector3.Distance(playerTransform.position, transform.position);
        if (distance <= distanceToDisplay)
        {
            if (!isInZone)
            {
                TurnOnOffDialouge(true);
                isInZone = true;
            }    
        }
        else
        {
            if (isInZone)
            {
                isInZone = false;
                TurnOnOffDialouge(false);
            }   
        }
    }

    public void TurnOnOffDialouge(bool turnOn)
    {
        canvasDialogue.SetActive(turnOn);
        if (turnOn)
        {
            if (!audioSource.isPlaying) audioSource.Play();
        }
        else 
        {
            audioSource.Stop();
        }
    }

    public void ReplayDialouge()
    {
        audioSource.Stop();
        audioSource.Play();
    }
}
