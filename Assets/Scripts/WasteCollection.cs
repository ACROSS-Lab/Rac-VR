using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteCollectionInfo : MonoBehaviour
{
    
    public bool ground_choice;
    public bool river_choice;
    public int nb_waste;
    public bool sendInfoWasteCollection;
    public bool pointInterstVisited;
    public int pointInterestIndex;

    // Start is called before the first frame update
    void Start()
    {
        ground_choice = false;
        river_choice = false;
        nb_waste = 0;
        sendInfoWasteCollection = false;
        pointInterstVisited = false;
        pointInterestIndex = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
