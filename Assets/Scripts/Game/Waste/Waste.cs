using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Waste : MonoBehaviour
{
    [HideInInspector] public bool fromInventory = false;
    [SerializeField] WasteType wasteType;

    [Header("Sound effects")]
    [SerializeField] AudioClip[] selectSounds;
    [SerializeField][Range(0.5f, 2f)] float minPitch = 0.9f;
    [SerializeField][Range(0.5f, 2f)] float maxPitch = 1.1f;

    [Header("Physics")]
    [Tooltip("The maximum speed the physics engine will use to separate this object from others. Lower is gentler.")]
    [SerializeField] private float maxDepenetrationVelocity = 1.5f;
    [Tooltip("How much 'air resistance' to add on release. Higher values slow it down faster.")]
    [SerializeField] private float temporaryDrag = 5f;
    [Tooltip("How long the damping effect should last, in seconds.")]
    [SerializeField] private float dampDuration = 0.5f;

    XRGrabInteractable interactable;
    bool firstSelected = false;
    MeshRenderer meshRenderer;
    AudioSource audioSource;
    Rigidbody rb;
    float orignalDrag, orignialAngularDrag;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        orignalDrag = rb.linearDamping;
        orignialAngularDrag = rb.angularDamping;
    }
    
    void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();

        interactable.hoverEntered.AddListener(HoverEnter);
        interactable.hoverExited.AddListener(HoverExit);
        interactable.selectEntered.AddListener(SelectEnter);
        interactable.selectExited.AddListener(SelectExit);
    }

    void Update()
    {
        if(!firstSelected)
        {
            if (interactable.isSelected)
            {
                firstSelected = true;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!firstSelected || interactable.isSelected) return;

        if (other.tag == "Bin")
        {
            GarbageClassification(other.GetComponent<Bin>());
            gameObject.SetActive(false);
            return;
        }

        else if (other.tag == "Inventory" && !fromInventory)
        {
            Inventory.instance.AddWaste(this);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Ground_RAC")
        {
            fromInventory = false;
        }
    }

    void GarbageClassification(Bin bin)
    {
        if ((bin.binType & wasteType) != 0)
        {
            bin.CorrectBin(wasteType, 1);
        }
        else
        {
            bin.WrongBin(wasteType, 0);
        }

        SendingHaptics.instance.SendRightHaptic();
    }

    void HoverEnter(HoverEnterEventArgs args)
    {
        meshRenderer.material.SetFloat("_Outline", 1.0f);
    }

    void HoverExit(HoverExitEventArgs args)
    {
        meshRenderer.material.SetFloat("_Outline", 0.0f);
    }

    void SelectEnter(SelectEnterEventArgs args)
    {
        if (selectSounds.Length > 0)
        {
            audioSource.clip = selectSounds[Random.Range(0, selectSounds.Length)];
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.Play();
        }
    }

    void SelectExit(SelectExitEventArgs args)
    {
        StartCoroutine(DampPhysicsOnRelease());
    }
    
    IEnumerator DampPhysicsOnRelease()
    {
        rb.maxDepenetrationVelocity = maxDepenetrationVelocity;
        rb.linearDamping = temporaryDrag;
        rb.angularDamping = temporaryDrag;

        yield return new WaitForSeconds(dampDuration);

        rb.maxDepenetrationVelocity = Physics.defaultMaxDepenetrationVelocity;
        rb.linearDamping = orignalDrag;
        rb.angularDamping = orignialAngularDrag;
    }
}
