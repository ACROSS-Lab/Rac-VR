using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(LocalizedKey))]
public class CharacterDialogue : MonoBehaviour
{
    [SerializeField] float distanceToDisplay = 5f;
    [SerializeField] GameObject canvasDialogue;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] AudioSource audioSource;

    Transform camTransform;
    Animator animator;
    Coroutine talkingCoroutine;

    void Awake()
    {
        LocalizedKey localizedKey = GetComponent<LocalizedKey>();
        localizedKey.audioSource = audioSource;
        localizedKey.textComponent = dialogueText;

    }

    void Start()
    {
        animator = GetComponent<Animator>();
        camTransform = Camera.main.transform;
    }

    void Update()
    {
        RotateTowardsCamera();
    }

    void RotateTowardsCamera()
    {
        if (!canvasDialogue.activeInHierarchy) return;

        Vector3 direction = canvasDialogue.transform.position - camTransform.position;
        direction.y = 0;
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            canvasDialogue.transform.rotation = targetRotation;
        }
    }

    public void DisplayDialouge()
    {
        float distance = Vector3.Distance(camTransform.position, transform.position);
        if (distance > distanceToDisplay) return;

        if (!canvasDialogue.activeInHierarchy) canvasDialogue.SetActive(true);

        audioSource.Stop();

        animator.SetBool("isTalking", false);
        animator.SetBool("isWaving", false);

        if (talkingCoroutine != null) StopCoroutine(talkingCoroutine);
        talkingCoroutine = StartCoroutine(StartTalkingAnimation());
    }

    public void TurnOffDialouge()
    {
        if (talkingCoroutine != null) StopCoroutine(talkingCoroutine);
        audioSource.Stop();
        animator.SetBool("isTalking", false);
        canvasDialogue.SetActive(false);
    }

    IEnumerator StartTalkingAnimation()
    {
        float length = audioSource.clip.length;
        audioSource.Play();
        animator.SetBool("isTalking", true);
        yield return new WaitForSeconds(length);
        animator.SetBool("isTalking", false);
    }
}
