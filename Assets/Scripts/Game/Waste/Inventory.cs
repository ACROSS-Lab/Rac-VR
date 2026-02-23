using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public event Action OnInventoryUpdated;

    [Header("Inventory Settings")]
    [SerializeField] int capacity = 5;
    [SerializeField] Mesh[] meshes;

    [Header("Inventory Feedback")]
    [SerializeField] GameObject feedbackCanvas;
    [SerializeField] ParticleSystem feedbackParticles;

    [Header("Sound effects")]
    [SerializeField] AudioClip[] addWasteSounds;
    [SerializeField] AudioClip[] getWasteSounds;
    [SerializeField][Range(0.5f, 2f)] float minPitch = 0.9f;
    [SerializeField][Range(0.5f, 2f)] float maxPitch = 1.1f;

    [Header("Follow Settings")]
    [SerializeField] Vector3 offset;
    [SerializeField] Transform targetTransform;
    [SerializeField] float smoothSpeed;

    public List<Waste> wastes {get;  private set; }
    [SerializeField] XRInteractionManager interactionManager;
    AudioSource audioSource;

    void Awake()
    {
        instance = this;
        wastes = new List<Waste>();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void LateUpdate()
    {
        transform.position = targetTransform.position + offset;
        transform.rotation = Quaternion.Euler(0, targetTransform.rotation.y, 0);
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

        SendingHaptics.instance.SendRightHaptic();
            
        if (audioSource.isPlaying) audioSource.Stop();
        if (addWasteSounds.Length > 0)
        {
            audioSource.clip = addWasteSounds[UnityEngine.Random.Range(0, addWasteSounds.Length)];
            audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            audioSource.Play();
        }

        if (feedbackParticles != null)
        {
            if (feedbackParticles.isPlaying) feedbackParticles.Stop();
            feedbackParticles.Play();
        }
        
        OnInventoryUpdated?.Invoke();
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

        SendingHaptics.instance.SendRightHaptic();

        if (audioSource.isPlaying) audioSource.Stop();
        if (getWasteSounds.Length > 0)
        {
            audioSource.clip = getWasteSounds[UnityEngine.Random.Range(0, getWasteSounds.Length)];
            audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            audioSource.Play();
        }

        OnInventoryUpdated?.Invoke();
    }

    public void ClearInventory()
    {
        wastes = new List<Waste>();
        CycleMesh();
    }

    private void DisplayFeedback()
    {
        if (feedbackCanvas == null) return;

        if (feedbackCanvas.activeInHierarchy)
        {
            feedbackCanvas.GetComponent<TweenFadeOut>().ForceEnd();
        }
        feedbackCanvas.SetActive(true);
    }
}