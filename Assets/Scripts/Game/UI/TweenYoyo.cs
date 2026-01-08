using DG.Tweening;
using UnityEngine;

public class TweenYoyo : MonoBehaviour
{
    [SerializeField] float duration = 1f;
    [SerializeField] Vector3 moveDirection;
    [SerializeField] bool lookAtCamera;

    Transform camTransform;

    void Start()
    {
        camTransform = Camera.main.transform;
        transform.DOLocalMove(moveDirection, duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    void LateUpdate()
    {
        RotateTowardsCamera();
    }

    void RotateTowardsCamera()
    {
        if (!lookAtCamera) return;

        Vector3 direction = transform.position - camTransform.position;
        direction.y = 0;
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
}

