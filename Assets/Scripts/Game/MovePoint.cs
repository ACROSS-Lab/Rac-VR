using UnityEngine;

public class MovePoint : MonoBehaviour
{
    [SerializeField] Collider characterCollider;

    void OnTriggerEnter(Collider other)
    {
        if (other == characterCollider)
        {
            TutorialManager.instance.MoveToDestination();
        }
    }
}