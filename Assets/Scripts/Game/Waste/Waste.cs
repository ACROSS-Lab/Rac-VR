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

    XRGrabInteractable interactable;
    bool firstSelected = false;
    MeshRenderer meshRenderer;
    AudioSource audioSource;
    
    void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();

        interactable.hoverEntered.AddListener(HoverEnter);
        interactable.hoverExited.AddListener(HoverExit);
        interactable.selectEntered.AddListener(SelectEnter);
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
            bin.WrongBin();
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
}
