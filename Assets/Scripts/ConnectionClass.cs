using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ConnectionClass
{
    public List<int> solidwasteClass;
    public List<int> waterwasteClass;
    public List<int> productionClass;
    public List<float> waterwaste;

    public  DisplayManagement dm;

    public static ConnectionClass CreateFromJSON(string jsonString, DisplayManagement dm)
    {
        ConnectionClass cc = JsonUtility.FromJson<ConnectionClass>(jsonString);
        cc.dm = dm;
        return cc;
    }


    private void displaylvl1(GameObject lvl2, GameObject lvl3, GameObject lvl4, GameObject lvl5)
    {
        if (lvl2.activeSelf)
        {
            lvl2.SetActive(false);
            if (lvl3.activeSelf)
            {
                lvl3.SetActive(false);
                if (lvl4.activeSelf)
                {
                    lvl4.SetActive(false);
                    if (lvl5.activeSelf)
                    {
                        lvl5.SetActive(false);
                    }
                }
            }
        }
    }

    private void displaylvl2(GameObject lvl2, GameObject lvl3, GameObject lvl4, GameObject lvl5)
    {
        if (lvl2.activeSelf) {
            if (lvl3.activeSelf) {
                lvl3.SetActive(false);
                if (lvl4.activeSelf) {
                    lvl4.SetActive(false);
                    if (lvl5.activeSelf) {
                        lvl5.SetActive(false);
                    }
                }
            }
        } 
        else {
            lvl2.SetActive(true);
        }
    }

    private void displaylvl3(GameObject lvl2, GameObject lvl3, GameObject lvl4, GameObject lvl5)
    {
        if (lvl2.activeSelf) {
            if (lvl3.activeSelf) {
                if (lvl4.activeSelf) {
                    lvl4.SetActive(false);
                    if (lvl5.activeSelf) {
                        lvl5.SetActive(false);
                    }
                }
            }
            else 
            {
                lvl3.SetActive(true);
            }
        } 
        else {
            lvl2.SetActive(true);
            lvl3.SetActive(true);
        }
    }

    private void displaylvl4(GameObject lvl2, GameObject lvl3, GameObject lvl4, GameObject lvl5)
    {
        if (!lvl5.activeSelf) {
            if (!lvl4.activeSelf) {
                lvl4.SetActive(true);
                if (!lvl3.activeSelf) {
                    lvl3.SetActive(true);
                    if (!lvl2.activeSelf) {
                        lvl2.SetActive(true);
                    }
                } 
            }
        } 
        else {
            lvl5.SetActive(false);
        }
    }

    private void displaylvl5(GameObject lvl2, GameObject lvl3, GameObject lvl4, GameObject lvl5)
    {
        if (!lvl5.activeSelf) {
            lvl5.SetActive(true);

            if (!lvl4.activeSelf) {
                lvl4.SetActive(true);

                if (!lvl3.activeSelf) {
                    lvl3.SetActive(true);

                    if (!lvl2.activeSelf) {
                        lvl2.SetActive(true);
                    }
                } 
            }
        }
    }



    public void displaySolidClass(int classValue){
        switch(classValue){
            case 0:
                // Urban Areas
                displaylvl1(dm.UASolidWasteLvl2, dm.UASolidWasteLvl3, dm.UASolidWasteLvl4, dm.UASolidWasteLvl5);

                // Canals
                displaylvl1(dm.CanalSolidWasteLvl2, dm.CanalSolidWasteLvl3, dm.CanalSolidWasteLvl4, dm.CanalSolidWasteLvl5);

                break;

            case 1:
                // Urban Areas
                displaylvl2(dm.UASolidWasteLvl2, dm.UASolidWasteLvl3, dm.UASolidWasteLvl4, dm.UASolidWasteLvl5);

                // Canals
                displaylvl2(dm.CanalSolidWasteLvl2, dm.CanalSolidWasteLvl3, dm.CanalSolidWasteLvl4, dm.CanalSolidWasteLvl5);

                break;
            case 2:
                // Urban Areas
                displaylvl3(dm.UASolidWasteLvl2, dm.UASolidWasteLvl3, dm.UASolidWasteLvl4, dm.UASolidWasteLvl5);

                // Canals
                displaylvl3(dm.CanalSolidWasteLvl2, dm.CanalSolidWasteLvl3, dm.CanalSolidWasteLvl4, dm.CanalSolidWasteLvl5);

                break;
            case 3:
                // Urban Areas
                displaylvl4(dm.UASolidWasteLvl2, dm.UASolidWasteLvl3, dm.UASolidWasteLvl4, dm.UASolidWasteLvl5);

                // Canals
                displaylvl4(dm.CanalSolidWasteLvl2, dm.CanalSolidWasteLvl3, dm.CanalSolidWasteLvl4, dm.CanalSolidWasteLvl5);

                break;
            default :
                // Urban Areas
                displaylvl5(dm.UASolidWasteLvl2, dm.UASolidWasteLvl3, dm.UASolidWasteLvl4, dm.UASolidWasteLvl5);

                // Canals
                displaylvl5(dm.CanalSolidWasteLvl2, dm.CanalSolidWasteLvl3, dm.CanalSolidWasteLvl4, dm.CanalSolidWasteLvl5);

                break;
        }
    }

    public void displayWaterClass(int classValue){
        switch(classValue){
            case 0:
                // Canals (Dead fish)
                displaylvl1(dm.CanalWasteWaterLvl2, dm.CanalWasteWaterLvl3, dm.CanalWasteWaterLvl4, dm.CanalWasteWaterLvl5);
                break;
            case 1:
                // Canals (Dead fish)
                displaylvl2(dm.CanalWasteWaterLvl2, dm.CanalWasteWaterLvl3, dm.CanalWasteWaterLvl4, dm.CanalWasteWaterLvl5);
                break;
            case 2:
                // Canals (Dead fish)
                displaylvl3(dm.CanalWasteWaterLvl2, dm.CanalWasteWaterLvl3, dm.CanalWasteWaterLvl4, dm.CanalWasteWaterLvl5);
                break;
            case 3:
                // Canals (Dead fish)
                displaylvl4(dm.CanalWasteWaterLvl2, dm.CanalWasteWaterLvl3, dm.CanalWasteWaterLvl4, dm.CanalWasteWaterLvl5);
                break;
            default :
                // Canals (Dead fish)
                displaylvl5(dm.CanalWasteWaterLvl2, dm.CanalWasteWaterLvl3, dm.CanalWasteWaterLvl4, dm.CanalWasteWaterLvl5);
                break;
        }
    }

    public Color computeTreesColor(float colorValue){
        Color c = new Color(0, 0, 0, 0);
        return c;
    }

    public void displayWaterColor(int classValue){
        Renderer renderer_tree1 = GameObject.Find("SM_Arbre_002").GetComponent<Renderer>();
        Material sharedMaterial_tree1 = renderer_tree1.sharedMaterial;

        Renderer renderer_tree2 = GameObject.Find("SM_Bananier_004").GetComponent<Renderer>();
        Material sharedMaterial_tree2 = renderer_tree2.sharedMaterial;

        Renderer renderer_plant = GameObject.Find("SM_GardenPlants_001").GetComponent<Renderer>();
        Material sharedMaterial_plant = renderer_plant.sharedMaterial;

        switch(classValue){
            case 0:
                sharedMaterial_tree1.SetColor("_Pollution_Color1", dm.TreeColorLvl1);
                sharedMaterial_tree2.SetColor("_Pollution_Color", dm.TreeColorLvl1);
                sharedMaterial_plant.SetColor("_Pollution_Color", dm.TreeColorLvl1);
                break;
            case 1:
                sharedMaterial_tree1.SetColor("_Pollution_Color1", dm.TreeColorLvl2);
                sharedMaterial_tree2.SetColor("_Pollution_Color", dm.TreeColorLvl2);
                sharedMaterial_plant.SetColor("_Pollution_Color", dm.TreeColorLvl2);
                break;
            case 2:
                sharedMaterial_tree1.SetColor("_Pollution_Color1", dm.TreeColorLvl3);
                sharedMaterial_tree2.SetColor("_Pollution_Color", dm.TreeColorLvl3);
                sharedMaterial_plant.SetColor("_Pollution_Color", dm.TreeColorLvl3);
                break;
            case 3:
                sharedMaterial_tree1.SetColor("_Pollution_Color1", dm.TreeColorLvl4);
                sharedMaterial_tree2.SetColor("_Pollution_Color", dm.TreeColorLvl4);
                sharedMaterial_plant.SetColor("_Pollution_Color", dm.TreeColorLvl4);
                break;
            default :
                sharedMaterial_tree1.SetColor("_Pollution_Color1", dm.TreeColorLvl5);
                sharedMaterial_tree2.SetColor("_Pollution_Color", dm.TreeColorLvl5);
                sharedMaterial_plant.SetColor("_Pollution_Color", dm.TreeColorLvl5);
                break;
        }  
    }

    public void displayProductionClass(int classValue){
        switch(classValue){
            case 0:
                displaylvl1(dm.ProductionLvl2, dm.ProductionLvl3, dm.ProductionLvl4, dm.ProductionLvl5);
                break;
            case 1:
                displaylvl2(dm.ProductionLvl2, dm.ProductionLvl3, dm.ProductionLvl4, dm.ProductionLvl5);
                break;
            case 2:
                displaylvl3(dm.ProductionLvl2, dm.ProductionLvl3, dm.ProductionLvl4, dm.ProductionLvl5);
                break;
            case 3:
                displaylvl4(dm.ProductionLvl2, dm.ProductionLvl3, dm.ProductionLvl4, dm.ProductionLvl5);
                break;
            default :
                displaylvl5(dm.ProductionLvl2, dm.ProductionLvl3, dm.ProductionLvl4, dm.ProductionLvl5);
                break;
        }
    }
}

    