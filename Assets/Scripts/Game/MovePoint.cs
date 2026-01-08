using UnityEngine;
using UnityEngine.Events;

public class MovePoint : MonoBehaviour
{
    [SerializeField] Collider characterCollider;
    [SerializeField] UnityEvent onTriggered;

    void Awake()
    {
        if (characterCollider == null)
        {
            characterCollider = GameObject.FindWithTag("Player").GetComponent<Collider>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == characterCollider)
        {
            onTriggered?.Invoke();
        }
    }
}