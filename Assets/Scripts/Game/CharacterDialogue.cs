using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(LocalizedKey))]
public class CharacterDialogue : MonoBehaviour
{
    [SerializeField] float distanceToDisplay = 5f;
    [SerializeField] float displayTime = 5f;
    [SerializeField] GameObject canvasDialogue;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] AudioSource audioSource;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] XRSimpleInteractable interactable;
    [SerializeField] bool isObjectiveNPC = true;

    Transform camTransform;
    Animator animator;
    Coroutine talkingCoroutine;
    bool hasTalked = false;
    bool finishedTalking = false;

    void OnEnable()
    {
        if(interactable != null)
        {
            interactable.hoverEntered.AddListener(HoverEnter);
            interactable.hoverExited.AddListener(HoverExit);
            interactable.selectEntered.AddListener((args) => DisplayDialouge());
        }
    }

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
        animator.speed = Random.Range(0.85f, 1.15f);
    }

    void Update()
    {
        RotateTowardsCamera();
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        if (hasTalked) return;
        
        float distance = Vector3.Distance(camTransform.position, transform.position);
        if (distance < distanceToDisplay)
        {
            animator.SetBool("isWaving", true);
        }
        else
        {
            animator.SetBool("isWaving", false);
        }
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

        if (!hasTalked)
        {
            hasTalked = true;
        }
    
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
        if (!finishedTalking)
        {
            if (isObjectiveNPC) Game.Manager.IncrementCharactersTalkedTo();
            finishedTalking = true;
        }
        if(displayTime > 0)
        {
            yield return new WaitForSeconds(displayTime);
            TurnOffDialouge();
        }
    }

    public void HoverEnter(HoverEnterEventArgs args)
    {
        skinnedMeshRenderer.materials[0].SetFloat("_Outline", 1.0f);
    }

    public void HoverExit(HoverExitEventArgs args)
    {
        skinnedMeshRenderer.materials[0].SetFloat("_Outline", 0.0f);
    }

    public void LoadTutorialScene()
    {
        SceneManager.LoadScene("RAC_Tuto_NonGP");
    }
}
