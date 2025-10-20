using DG.Tweening;
using UnityEngine;

public class TweenFadeOut : MonoBehaviour
{
    [SerializeField] float moveDistance = 1f;
    [SerializeField] float duration = 1f;
    [SerializeField] bool lookAtCamera = false;

    CanvasGroup canvasGroup;
    Transform camTransform;
    Vector3 startPos;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        camTransform = Camera.main.transform;
        startPos = transform.localPosition;
    }

    void OnEnable()
    {
        canvasGroup.alpha = 1f;
        FadeOut();
    }

    void Update()
    {
        if(lookAtCamera)
        {
            RotateTowardsCamera();
        }
    }

    void FadeOut()
    {
        Vector3 endPos = startPos + Vector3.up * moveDistance;

        transform.DOLocalMove(endPos, duration).SetEase(Ease.OutQuad);
        canvasGroup.DOFade(0f, duration).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            transform.localPosition = startPos;
            gameObject.SetActive(false);
        });
    }

    void RotateTowardsCamera()
    {
        Vector3 direction = transform.position - camTransform.position;
        direction.y = 0;
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }

    public void ForceEnd()
    {
        transform.localPosition = startPos;
        gameObject.SetActive(false);
    }
}
