using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Waste : MonoBehaviour
{
    [HideInInspector] public bool fromInventory = false;
    [SerializeField] WasteType wasteType;

    XRGrabInteractable interactable;
    bool firstSelected = false;
    MeshRenderer meshRenderer;
    
    void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
        meshRenderer = GetComponent<MeshRenderer>();

        interactable.hoverEntered.AddListener(HoverEnter);
        interactable.hoverExited.AddListener(HoverExit);
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
            bin.PlusScore(wasteType, 1);
        }
        else
        {
            bin.MinusScore();
        }

        GameManager.instance.SendRightHaptic();
    }

    public void HoverEnter(HoverEnterEventArgs args)
    {
        meshRenderer.material.SetFloat("_Outline", 1.0f);
    }

    public void HoverExit(HoverExitEventArgs args)
    {
        meshRenderer.material.SetFloat("_Outline", 0.0f);
    }
}
