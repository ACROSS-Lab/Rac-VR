using UnityEngine;

public class FallProofReset: MonoBehaviour
{
    [SerializeField] Collider player;
    Vector3 startPos;

    void Start()
    {
        startPos = player.transform.position;
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other == player)
        {
            player.transform.position = startPos;
        }
    }
}
