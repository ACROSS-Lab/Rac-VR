using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [SerializeField] int capacity = 5;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float smoothSpeed;
    [SerializeField] GameObject feedbackCanvas;
    [SerializeField] Mesh[] meshes;

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

    void LateUpdate()
    {
        // float cameraYaw = cameraTransform.eulerAngles.y;
        // Quaternion targetRotation = Quaternion.Euler(0, cameraYaw, 0);
        // transform.parent.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(Vector3.zero), Time.deltaTime * smoothSpeed);
    }

    void CycleMesh()
    {
        MeshFilter meshFilter = GetComponentInChildren<MeshFilter>();
        if (meshFilter != null)
        {
            if (wastes.Count == 0)
            {
                meshFilter.mesh = meshes[0];
            }
            else if(wastes.Count > 0 && wastes.Count < capacity)
            {
                meshFilter.mesh = meshes[1];
            }
            else if(wastes.Count == capacity)
            {
                meshFilter.mesh = meshes[2];
                DisplayFeedback();
            }
        }
    }

    public void AddWaste(Waste item)
    {
        if(wastes.Count >= capacity) return;

        wastes.Add(item);
        item.transform.position = transform.position;
        item.gameObject.SetActive(false);

        CycleMesh();

        GameManager.instance.SendLeftHaptic();
    }

    public void GetWaste(SelectEnterEventArgs args)
    {
        if (wastes.Count == 0) return;

        Waste item = wastes[wastes.Count - 1];
        wastes.RemoveAt(wastes.Count - 1);
        item.gameObject.SetActive(true);
        item.fromInventory = true;

        IXRSelectInteractor interactor = args.interactorObject;
        XRGrabInteractable grabInteractable = item.GetComponent<XRGrabInteractable>();
        interactionManager.SelectEnter(interactor, grabInteractable);

        CycleMesh();

        GameManager.instance.SendLeftHaptic();
    }

    public void ClearInventory()
    {
        wastes = new List<Waste>();
        CycleMesh();
    }

    private void DisplayFeedback()
    {
        if (feedbackCanvas.activeInHierarchy)
        {
            feedbackCanvas.GetComponent<TweenFadeOut>().ForceEnd();
        }
        feedbackCanvas.SetActive(true);
    }
}