using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Waste : MonoBehaviour
{
    [SerializeField] WasteType wasteType;

    XRGrabInteractable interactable;

    void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(interactable.isSelected) return;

        if (other.tag == "Inventory")
        {
            Inventory.instance.AddWaste(this);
        }

        else if (other.tag == "Bin")
        {
            GarbageClassification(other.GetComponent<Bin>());
            gameObject.SetActive(false);
        }
    }
    
    void GarbageClassification(Bin bin)
    {
        if (bin.binType == wasteType)
        {
            Debug.Log("Correct Bin");
        }
        else
        {
            Debug.Log("Wrong Bin");
        }
    }
}
