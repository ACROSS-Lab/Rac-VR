using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManagement : MonoBehaviour
{   
    //Solid waste
    public GameObject UASolidWasteLvl1;
    public GameObject UASolidWasteLvl2;
    public GameObject UASolidWasteLvl3;
    public GameObject UASolidWasteLvl4;
    public GameObject UASolidWasteLvl5;

    public GameObject CanalSolidWasteLvl1;
    public GameObject CanalSolidWasteLvl2;
    public GameObject CanalSolidWasteLvl3;
    public GameObject CanalSolidWasteLvl4;
    public GameObject CanalSolidWasteLvl5;

    //Wastewater
        //UA
    
    public Color TreeColorLvl1;
    public Color TreeColorLvl2;
    public Color TreeColorLvl3;
    public Color TreeColorLvl4;
    public Color TreeColorLvl5;

        //Fields
    public Color FieldWaterColorLvl1;
    public Color FieldWaterColorLvl2;
    public Color FieldWaterColorLvl3;
    public Color FieldWaterColorLvl4;
    public Color FieldWaterColorLvl5;

        //Canals
    public GameObject CanalWasteWaterLvl1;
    public GameObject CanalWasteWaterLvl2;
    public GameObject CanalWasteWaterLvl3;
    public GameObject CanalWasteWaterLvl4;
    public GameObject CanalWasteWaterLvl5;
    
    
    public Color CanalWaterColorLvl1;
    public Color CanalWaterColorLvl2;
    public Color CanalWaterColorLvl3;
    public Color CanalWaterColorLvl4;
    public Color CanalWaterColorLvl5;

    //Production
    public GameObject ProductionLvl1;
    public GameObject ProductionLvl2;
    public GameObject ProductionLvl3;
    //public GameObject ProductionLvl4;
    //public GameObject ProductionLvl5;


    public Material material_tree1 ;
    public Material material_tree2 ;
    public Material material_plant ;
    public Material material_canalWater ;
    public Material material_fieldWater ;
    public Material material_grass ;


    // Start is called before the first frame update
    void Start()
    {
        ProductionLvl1.SetActive(false);
        ProductionLvl2.SetActive(false);
        ProductionLvl3.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
}
