using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManagement : MonoBehaviour
{   
    //Solid waste
    public List<GameObject> UASolidWasteLvl1;
    public List<GameObject> UASolidWasteLvl2;
    public List<GameObject> UASolidWasteLvl3;
    public List<GameObject> UASolidWasteLvl4;
    public List<GameObject> UASolidWasteLvl5;

    public List<GameObject> CanalSolidWasteLvl1;
    public List<GameObject> CanalSolidWasteLvl2;
    public List<GameObject> CanalSolidWasteLvl3;
    public List<GameObject> CanalSolidWasteLvl4;
    public List<GameObject> CanalSolidWasteLvl5;

    public List<GameObject> FieldSolidWasteLvl1;
    public List<GameObject> FieldSolidWasteLvl2;
    public List<GameObject> FieldSolidWasteLvl3;
    public List<GameObject> FieldSolidWasteLvl4;
    public List<GameObject> FieldSolidWasteLvl5;


    //Wastewater
    public List<GameObject> CanalWasteWaterLvl1;
    public List<GameObject> CanalWasteWaterLvl2;
    public List<GameObject> CanalWasteWaterLvl3;
    public List<GameObject> CanalWasteWaterLvl4;
    public List<GameObject> CanalWasteWaterLvl5;
    /**
    public Color TreeWasteWaterLvl1;
    public Color TreeWasteWaterLvl2;
    public Color TreeWasteWaterLvl3;
    public Color TreeWasteWaterLvl4;
    public Color TreeWasteWaterLvl5;

    public Color FieldWasteWaterLvl1;
    public Color FieldWasteWaterLvl2;
    public Color FieldWasteWaterLvl3;
    public Color FieldWasteWaterLvl4;
    public Color FieldWasteWaterLvl5;
    */


    //Production

    // Start is called before the first frame update
    void Start()
    {   
        // Solid Waste Init
        foreach(GameObject item in UASolidWasteLvl1) {item.SetActive(true);}
        foreach(GameObject item in UASolidWasteLvl2) {item.SetActive(false);}
        foreach(GameObject item in UASolidWasteLvl3) {item.SetActive(false);}
        foreach(GameObject item in UASolidWasteLvl4) {item.SetActive(false);}
        foreach(GameObject item in UASolidWasteLvl5) {item.SetActive(false);}
            
        foreach(GameObject item in CanalSolidWasteLvl1) {item.SetActive(true);}
        foreach(GameObject item in CanalSolidWasteLvl2) {item.SetActive(false);}
        foreach(GameObject item in CanalSolidWasteLvl3) {item.SetActive(false);}
        foreach(GameObject item in CanalSolidWasteLvl4) {item.SetActive(false);}
        foreach(GameObject item in CanalSolidWasteLvl5) {item.SetActive(false);}

        foreach(GameObject item in FieldSolidWasteLvl1) {item.SetActive(true);}
        foreach(GameObject item in FieldSolidWasteLvl2) {item.SetActive(false);}
        foreach(GameObject item in FieldSolidWasteLvl3) {item.SetActive(false);}
        foreach(GameObject item in FieldSolidWasteLvl4) {item.SetActive(false);}
        foreach(GameObject item in FieldSolidWasteLvl5) {item.SetActive(false);}


        // Wastewater Init
        foreach(GameObject item in CanalWasteWaterLvl1) {item.SetActive(true);}
        foreach(GameObject item in CanalWasteWaterLvl2) {item.SetActive(false);}
        foreach(GameObject item in CanalWasteWaterLvl3) {item.SetActive(false);}
        foreach(GameObject item in CanalWasteWaterLvl4) {item.SetActive(false);}
        foreach(GameObject item in CanalWasteWaterLvl5) {item.SetActive(false);}
    }

    // Update is called once per frame
    void Update()
    {

    }
    
}
