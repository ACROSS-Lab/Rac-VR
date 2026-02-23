using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Waste : MonoBehaviour
{
    [Header("Waste type")]
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

    [Header("Visual effects")]
    [SerializeField] float timeBeforeFlashing = 15f;
    [SerializeField] float flashDuration = 1f;

    [HideInInspector] public bool fromInventory = false;
    [HideInInspector] public Quest quest;

    XRGrabInteractable interactable;
    bool firstSelected = false;
    bool onGround = true;
    MeshRenderer meshRenderer;
    Material material;
    AudioSource audioSource;
    Rigidbody rb;
    float orignalDrag, orignialAngularDrag;
    Collider binCol, inventoryCol;
    float groundedTime;
    bool isHovered = false;
    bool setOutlineTemporary = false;
    

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

        material = meshRenderer.material;
    }

    void FixedUpdate()
    {
        if (binCol != null)
        {
            GarbageClassification(binCol.GetComponent<Bin>());
            gameObject.SetActive(false);
            return;
        }
        else if (inventoryCol != null)
        {
            Inventory.instance.AddWaste(this);
        }

        binCol = null;
        inventoryCol = null;
    }

    void Update()
    {
        NotifyOnGround();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!firstSelected || onGround || interactable.isSelected) return;

        if (other.tag == "Bin")
        {
            binCol = other;
        }
        else if (other.tag == "Inventory" && !fromInventory)
        {
            inventoryCol = other;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ground_RAC")
        {
            fromInventory = false;
            onGround = true;
        }
    }

    void GarbageClassification(Bin bin)
    {
        if ((bin.binType & wasteType) != 0)
        {
            bin.CorrectBin(quest);
        }
        else
        {
            bin.WrongBin(quest);
        }

        SendingHaptics.instance.SendRightHaptic();
    }

    void HoverEnter(HoverEnterEventArgs args)
    {
        isHovered = true;
        groundedTime = 0;
        setOutlineTemporary = false;
        material.SetColor("_Outline_Color", Color.white);
        material.SetFloat("_Outline_Power", 1.0f);
        material.SetFloat("_Outline", 1.0f);
    }

    void HoverExit(HoverExitEventArgs args)
    {
        isHovered = false;
        material.SetFloat("_Outline", 0.0f);
    }

    void SelectEnter(SelectEnterEventArgs args)
    {
        if (selectSounds.Length > 0)
        {
            audioSource.clip = selectSounds[Random.Range(0, selectSounds.Length)];
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.Play();
        }

        if (!firstSelected)
        {
            firstSelected = true;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }

        onGround = false;
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

    void NotifyOnGround()
    {
        if (isHovered) return;

        if (onGround)
        {
            groundedTime += Time.deltaTime;
        }

        if (groundedTime >= timeBeforeFlashing)
        {
            if (!setOutlineTemporary)
            {
                material.SetFloat("_Outline", 1.0f);
                material.SetFloat("_Outline_Power", 0f);
                setOutlineTemporary = true;
            }
            float t = (Mathf.Sin(Time.time * Mathf.PI / flashDuration) + 1) / 2;
            Color c = Color.Lerp(Color.black, Color.white, t);
            material.SetColor("_Outline_Color", c);
        }
    }
}
