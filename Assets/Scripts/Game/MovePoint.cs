using UnityEngine;
using UnityEngine.Events;

public class MovePoint : MonoBehaviour
{
    [SerializeField] Collider characterCollider;
    [SerializeField] UnityEvent onTriggered;

    void OnTriggerEnter(Collider other)
    {
        if (other == characterCollider)
        {
            onTriggered?.Invoke();
        }
    }
}