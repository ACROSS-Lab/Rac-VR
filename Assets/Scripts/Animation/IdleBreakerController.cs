using UnityEngine;

public class IdleBreakerController : MonoBehaviour
{
    public Animator animator;
    public float baseDelay = 5f;
    public float randomOffset = 1.5f;

    private float timer;
    private float nextDelay;

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextDelay)
        {
            animator.SetTrigger("IdleBreak");
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        timer = 0f;
        nextDelay = baseDelay + Random.Range(-randomOffset, randomOffset);
    }
}
