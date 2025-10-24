using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [Header("Inventory Settings")]
    [SerializeField] int capacity = 5;
    [SerializeField] Mesh[] meshes;

    [Header("Inventory Feedback")]
    [SerializeField] Transform cameraTransform;
    [SerializeField] float smoothSpeed;
    [SerializeField] GameObject feedbackCanvas;
    [SerializeField] AudioClip addWasteSound;
    [SerializeField] AudioClip getWasteSound;

    List<Waste> wastes = new List<Waste>();
    XRInteractionManager interactionManager;
    AudioSource audioSource;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;

        interactionManager = FindFirstObjectByType<XRInteractionManager>();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void LateUpdate()
    {
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

        if (audioSource.isPlaying) audioSource.Stop();
        if (addWasteSound != null)
        {
            audioSource.clip = addWasteSound;
            audioSource.Play();
        }
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

        if (audioSource.isPlaying) audioSource.Stop();
        if (getWasteSound != null)
        {
            audioSource.clip = getWasteSound;
            audioSource.Play();
        }
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