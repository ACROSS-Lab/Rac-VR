using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageId : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI idText;

    void Start()
    {
        idText.text = "No village";
    }

    void Update()
    {
        if (GameManager.Instance.GetVillageId() != -1) {
            idText.text = "You are village " + (GameManager.Instance.GetVillageId() + 1);
        }
    }
}
