using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CharacterDialogue : MonoBehaviour
{
    [SerializeField] float distanceToDisplay = 5f;
    [SerializeField] GameObject canvasDialogue;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Voiceline[] voicelines;

    Transform camTransform;
    Animator animator;
    Voiceline currentVoiceline;

    void Start()
    {
        animator = GetComponent<Animator>();
        SelectVoiceline(0);
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
        audioSource.Play();

        animator.SetBool("isWaving", false);
        StopCoroutine(StartTalkingAnimation());
        StartCoroutine(StartTalkingAnimation());
    }

    public void TurnOffDialouge()
    {
        canvasDialogue.SetActive(false);
        audioSource.Stop();
        StopCoroutine(StartTalkingAnimation());
        animator.SetBool("isTalking", false);
    }

    public void SelectVoiceline(int index)
    {
        currentVoiceline = voicelines[index];
        audioSource.clip = currentVoiceline.clip;
        dialogueText.text = currentVoiceline.text;
        animator.SetBool("isWaving", true);
    }

    IEnumerator StartTalkingAnimation()
    {
        float length = audioSource.clip.length;
        Debug.Log(length);
        animator.SetBool("isTalking", true);
        yield return new WaitForSeconds(length);
        animator.SetBool("isTalking", false);
    }
}

[Serializable]
public class Voiceline
{
    public string text;
    public AudioClip clip;
}
