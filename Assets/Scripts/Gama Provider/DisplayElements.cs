using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayElements : MonoBehaviour
{
    public int productionC;
    public int waterwasteC;
    public int solidwasteSoilC;
    public int solidwasteCanalC;


    public DisplayManagement dm;
    void Start()
    {
        ConnectionClass cc = new ConnectionClass();
        cc.dm = dm;
        cc.DisplayLevel(productionC, waterwasteC, solidwasteSoilC, solidwasteCanalC);

    } 
}

