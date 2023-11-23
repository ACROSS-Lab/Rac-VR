using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrannyKiller : MonoBehaviour
{
    [SerializeField] private GameObject granny;

    void Start()
    {
        granny.SetActive(false);
    }
}
