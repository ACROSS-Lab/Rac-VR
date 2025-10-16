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
        if (interactable.isSelected) return;

        if (other.tag == "Bin")
        {
            GarbageClassification(other.GetComponent<Bin>());
            gameObject.SetActive(false);
        }
        
        else if (other.tag == "Inventory")
        {
            Inventory.instance.AddWaste(this);
        }
    }
    
    void GarbageClassification(Bin bin)
    {
        if ((bin.binType & wasteType) != 0)
        {
            GameManager.instance.AddScore(10);
        }
        else
        {
            GameManager.instance.MinusScore(5);
        }
    }
}
