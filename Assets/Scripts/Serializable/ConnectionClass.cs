using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ConnectionClass
{
    public List<int> solidwasteSoilClass;
    public List<int> solidwasteCanalClass;
    public List<int> waterwasteClass;
    public List<int> productionClass;

    public DisplayManagement dm;

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

    private void displaylvlonly(int lvl, GameObject lvl2, GameObject lvl3, GameObject lvl4, GameObject lvl5)
    {
        
    }


    public void displaySolidClass(int classValueSoil, int classValueCanal){
        switch(classValueSoil){
            case 0:
                // Urban Areas
                Debug.Log("dm: " + dm);
                Debug.Log("dm.UASolidWasteLvl2: " + dm.UASolidWasteLvl2);
                displaylvl1(dm.UASolidWasteLvl2, dm.UASolidWasteLvl3, dm.UASolidWasteLvl4, dm.UASolidWasteLvl5);

                break;

            case 1:
                // Urban Areas
                displaylvl2(dm.UASolidWasteLvl2, dm.UASolidWasteLvl3, dm.UASolidWasteLvl4, dm.UASolidWasteLvl5);

                break;
            case 2:
                // Urban Areas
                displaylvl3(dm.UASolidWasteLvl2, dm.UASolidWasteLvl3, dm.UASolidWasteLvl4, dm.UASolidWasteLvl5);

                break;
            case 3:
                // Urban Areas
                displaylvl4(dm.UASolidWasteLvl2, dm.UASolidWasteLvl3, dm.UASolidWasteLvl4, dm.UASolidWasteLvl5);

                break;
            default :
                // Urban Areas
                displaylvl5(dm.UASolidWasteLvl2, dm.UASolidWasteLvl3, dm.UASolidWasteLvl4, dm.UASolidWasteLvl5);

                break;
        }

        switch(classValueCanal){
            case 0:
                // Canals
                displaylvl1(dm.CanalSolidWasteLvl2, dm.CanalSolidWasteLvl3, dm.CanalSolidWasteLvl4, dm.CanalSolidWasteLvl5);

                break;

            case 1:
                // Canals
                displaylvl2(dm.CanalSolidWasteLvl2, dm.CanalSolidWasteLvl3, dm.CanalSolidWasteLvl4, dm.CanalSolidWasteLvl5);

                break;
            case 2:
                // Canals
                displaylvl3(dm.CanalSolidWasteLvl2, dm.CanalSolidWasteLvl3, dm.CanalSolidWasteLvl4, dm.CanalSolidWasteLvl5);

                break;
            case 3:
                // Canals
                displaylvl4(dm.CanalSolidWasteLvl2, dm.CanalSolidWasteLvl3, dm.CanalSolidWasteLvl4, dm.CanalSolidWasteLvl5);

                break;
            default :
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
        switch(classValue){
            case 0:
                dm.material_tree1.SetColor("_Pollution_Color1", dm.TreeColorLvl1);
                dm.material_tree2.SetColor("_Pollution_Color", dm.TreeColorLvl1);
                dm.material_plant.SetColor("_Pollution_Color", dm.TreeColorLvl1);
                dm.material_canalWater.SetColor("_Color0", dm.CanalWaterColorLvl1);
                dm.material_fieldWater.SetColor("_Color0", dm.FieldWaterColorLvl1);
                dm.material_grass.SetColor("_PollutionColor", dm.TreeColorLvl1);
                break;
            case 1:
                dm.material_tree1.SetColor("_Pollution_Color1", dm.TreeColorLvl2);
                dm.material_tree2.SetColor("_Pollution_Color", dm.TreeColorLvl2);
                dm.material_plant.SetColor("_Pollution_Color", dm.TreeColorLvl2);
                dm.material_canalWater.SetColor("_Color0", dm.CanalWaterColorLvl2);
                dm.material_fieldWater.SetColor("_Color0", dm.FieldWaterColorLvl2);
                dm.material_grass.SetColor("_PollutionColor", dm.TreeColorLvl2);
                break;
            case 2:
                dm.material_tree1.SetColor("_Pollution_Color1", dm.TreeColorLvl3);
                dm.material_tree2.SetColor("_Pollution_Color", dm.TreeColorLvl3);
                dm.material_plant.SetColor("_Pollution_Color", dm.TreeColorLvl3);
                dm.material_canalWater.SetColor("_Color0", dm.CanalWaterColorLvl3);
                dm.material_fieldWater.SetColor("_Color0", dm.FieldWaterColorLvl3);
                dm.material_grass.SetColor("_PollutionColor", dm.TreeColorLvl3);
                break;
            case 3:
                dm.material_tree1.SetColor("_Pollution_Color1", dm.TreeColorLvl4);
                dm.material_tree2.SetColor("_Pollution_Color", dm.TreeColorLvl4);
                dm.material_plant.SetColor("_Pollution_Color", dm.TreeColorLvl4);
                dm.material_canalWater.SetColor("_Color0", dm.CanalWaterColorLvl4);
                dm.material_fieldWater.SetColor("_Color0", dm.FieldWaterColorLvl4);
                dm.material_grass.SetColor("_PollutionColor", dm.TreeColorLvl4);
                break;
            default :
                dm.material_tree1.SetColor("_Pollution_Color1", dm.TreeColorLvl5);
                dm.material_tree2.SetColor("_Pollution_Color", dm.TreeColorLvl5);
                dm.material_plant.SetColor("_Pollution_Color", dm.TreeColorLvl5);
                dm.material_canalWater.SetColor("_Color0", dm.CanalWaterColorLvl5);
                dm.material_fieldWater.SetColor("_Color0", dm.FieldWaterColorLvl5);
                dm.material_grass.SetColor("_PollutionColor", dm.TreeColorLvl5);
                break;
        }  
    }

    public void displayProductionClass(int classValue){
        switch(classValue){
            case 0:
                if (!dm.ProductionLvl1.activeSelf)
                {
                    dm.ProductionLvl1.SetActive(true);
                    dm.ProductionLvl2.SetActive(false);
                    dm.ProductionLvl3.SetActive(false);
                }
                break;
            case 1:
                if (!dm.ProductionLvl2.activeSelf)
                {
                    dm.ProductionLvl1.SetActive(false);
                    dm.ProductionLvl2.SetActive(true);
                    dm.ProductionLvl3.SetActive(false);
                }
                break;
            default :
                if (!dm.ProductionLvl3.activeSelf)
                {
                    dm.ProductionLvl1.SetActive(false);
                    dm.ProductionLvl2.SetActive(false);
                    dm.ProductionLvl3.SetActive(true);
                }
                break;
        }
    }
}

    