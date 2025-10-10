using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [SerializeField] int capacity = 5;

    List<Waste> wastes = new List<Waste>();
    XRInteractionManager interactionManager;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;

        interactionManager = FindFirstObjectByType<XRInteractionManager>();
    }

    public void AddWaste(Waste item)
    {
        if(wastes.Count >= capacity) return;

        wastes.Add(item);
        item.transform.position = transform.position;
        item.gameObject.SetActive(false);
    }

    public void GetWaste(SelectEnterEventArgs args)
    {
        if (wastes.Count == 0) return;

        Waste item = wastes[wastes.Count - 1];
        wastes.RemoveAt(wastes.Count - 1);
        item.gameObject.SetActive(true);

        IXRSelectInteractor interactor = args.interactorObject;
        XRGrabInteractable grabInteractable = item.GetComponent<XRGrabInteractable>();
        interactionManager.SelectEnter(interactor, grabInteractable);
    }
}