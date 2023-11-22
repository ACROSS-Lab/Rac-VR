using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageId : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI idText;

    void Start()
    {
        idText.text = "Village number: ?";
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetVillageId() != -1) {
            idText.text = "Village number: " + (GameManager.Instance.GetVillageId() + 1);
        }
    }
}
