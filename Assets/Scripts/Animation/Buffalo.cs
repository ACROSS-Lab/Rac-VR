using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Buffalo : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem heart;
    [SerializeField] float baseDelay = 5f;
    [SerializeField] float randomOffset = 1.5f;
    [SerializeField] string petStateName;
    [SerializeField] float petDistance = 1.5f;
    [SerializeField] XRSimpleInteractable interactable;
    [SerializeField] float minSpeed;
    [SerializeField] float minPetTime;

    Transform camTransform;
    XRBaseInteractor interactor;
    Vector3 lastInteractorPos;
    bool isHovering;
    float petTime;
 
    float timer;
    float nextDelay;

    void Awake()
    {
        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);
    }

    void OnDestroy()
    {
        interactable.hoverEntered.RemoveListener(OnHoverEnter);
        interactable.hoverExited.RemoveListener(OnHoverExit);
    }

    void Start()
    {
        camTransform = Camera.main.transform;
        ResetTimer();
    }

    void Update()
    {
        if (isHovering && interactor != null)
        {
            Pet();
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= nextDelay)
            {
                animator.SetTrigger("IdleBreak");
                ResetTimer();
            }
        }
        
    }

    void ResetTimer()
    {
        timer = 0f;
        nextDelay = baseDelay + Random.Range(-randomOffset, randomOffset);
    }

    public void Pet()
    {
        Vector3 contactPoint = GetComponent<BoxCollider>().ClosestPoint(camTransform.position);
        float distance = Vector3.Distance(camTransform.position, contactPoint);
        if (distance > petDistance) return;

        PetLoop();
    }

    void OnHoverEnter(HoverEnterEventArgs args)
    {
        if (args.interactorObject is XRBaseInteractor interactor)
        {
            this.interactor = interactor;
            lastInteractorPos = this.interactor.transform.position;
        }
        
        isHovering = true;
        petTime = 0f;
        animator.SetBool("IsPetting", false);
    }

    void OnHoverExit(HoverExitEventArgs args)
    {
        isHovering = false;
        interactor = null;
        petTime = 0f;
        animator.SetBool("IsPetting", false);
        if (heart.isPlaying) heart.Stop();
    }

    void PetLoop()
    {
        Vector3 currentPos = interactor.transform.position;
        float speed = Vector3.Distance(currentPos, lastInteractorPos) / Time.deltaTime;
        lastInteractorPos = currentPos;
        if (speed > minSpeed)
        {
            petTime += Time.deltaTime;
        }
        else
        {
            petTime -= Time.deltaTime;
        }

        petTime = Mathf.Clamp(petTime, 0, minPetTime + 0.1f);

        if (petTime >= minPetTime)
        {
            Debug.Log("Petting");
            animator.SetBool("IsPetting", true);
            if (!heart.isPlaying) heart.Play();
        }

    }
}
