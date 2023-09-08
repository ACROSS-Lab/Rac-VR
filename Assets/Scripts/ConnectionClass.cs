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


    private void displaylvl1(List<GameObject> lvl2, List<GameObject> lvl3, List<GameObject> lvl4, List<GameObject> lvl5)
    {
        if (lvl2[0].activeSelf)
        {
            foreach (GameObject item in lvl2) { item.SetActive(false); }

            if (lvl3[0].activeSelf)
            {
                foreach (GameObject item in lvl3) { item.SetActive(false); }

                if (lvl4[0].activeSelf)
                {
                    foreach (GameObject item in lvl4) { item.SetActive(false); }

                    if (lvl5[0].activeSelf)
                    {
                        foreach (GameObject item in lvl5) { item.SetActive(false); }
                    }
                }
            }
        }
    }

    private void displaylvl2(List<GameObject> lvl2, List<GameObject> lvl3, List<GameObject> lvl4, List<GameObject> lvl5)
    {
        if (lvl2[0].activeSelf) {
            if (lvl3[0].activeSelf) {
                foreach(GameObject item in lvl3) {item.SetActive(false);}

                if (lvl4[0].activeSelf) {
                    foreach(GameObject item in lvl4) {item.SetActive(false);}

                    if (lvl5[0].activeSelf) {
                        foreach(GameObject item in lvl5) {item.SetActive(false);}
                    }
                }
            }
        } 
        else {
            foreach(GameObject item in lvl2) {item.SetActive(true);}
        }
    }
    private void displaylvl3(List<GameObject> lvl2, List<GameObject> lvl3, List<GameObject> lvl4, List<GameObject> lvl5)
    {
        if (lvl2[0].activeSelf) {
                    if (lvl3[0].activeSelf) {
                        if (lvl4[0].activeSelf) {
                            foreach(GameObject item in lvl4) {item.SetActive(false);}

                            if (lvl5[0].activeSelf) {
                                foreach(GameObject item in lvl5) {item.SetActive(false);}
                            }
                        }
                    }
                    else 
                    {
                        foreach(GameObject item in lvl3) {item.SetActive(true);}
                    }
                } 
                else {
                    foreach(GameObject item in lvl2) {item.SetActive(true);}
                    foreach(GameObject item in lvl3) {item.SetActive(true);}
                }
    }

    private void displaylvl4(List<GameObject> lvl2, List<GameObject> lvl3, List<GameObject> lvl4, List<GameObject> lvl5)
    {
        if (!dm.UASolidWasteLvl5[0].activeSelf) {
            if (!dm.UASolidWasteLvl4[0].activeSelf) {
                foreach(GameObject item in dm.UASolidWasteLvl4) {item.SetActive(true);}

                if (!dm.UASolidWasteLvl3[0].activeSelf) {
                    foreach(GameObject item in dm.UASolidWasteLvl3) {item.SetActive(true);}

                    if (!dm.UASolidWasteLvl2[0].activeSelf) {
                        foreach(GameObject item in dm.UASolidWasteLvl2) {item.SetActive(true);}
                    }
                } 
            }
        } 
        else {
            foreach(GameObject item in dm.UASolidWasteLvl5) {item.SetActive(false);}
        }
    }

    private void displaylvl4(List<GameObject> lvl2, List<GameObject> lvl3, List<GameObject> lvl4, List<GameObject> lvl5)
    {
        if (!dm.UASolidWasteLvl5[0].activeSelf) {
            foreach(GameObject item in dm.UASolidWasteLvl5) {item.SetActive(true);}

            if (!dm.UASolidWasteLvl4[0].activeSelf) {
                foreach(GameObject item in dm.UASolidWasteLvl4) {item.SetActive(true);}

                if (!dm.UASolidWasteLvl3[0].activeSelf) {
                    foreach(GameObject item in dm.UASolidWasteLvl3) {item.SetActive(true);}

                    if (!dm.UASolidWasteLvl2[0].activeSelf) {
                        foreach(GameObject item in dm.UASolidWasteLvl2) {item.SetActive(true);}
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


                // Fields
                displaylvl1(dm.FieldSolidWasteLvl2, dm.FieldSolidWasteLvl3, dm.FieldSolidWasteLvl4, dm.FieldSolidWasteLvl5);

                break;

            case 1:
                // Urban Areas
                displaylvl2(dm.UASolidWasteLvl2, dm.UASolidWasteLvl3, dm.UASolidWasteLvl4, dm.UASolidWasteLvl5);

                // Canals
                displaylvl2(dm.CanalSolidWasteLvl2, dm.CanalSolidWasteLvl3, dm.CanalSolidWasteLvl4, dm.CanalSolidWasteLvl5);


                // Fields
                displaylvl2(dm.FieldSolidWasteLvl2, dm.FieldSolidWasteLvl3, dm.FieldSolidWasteLvl4, dm.FieldSolidWasteLvl5);
                
                break;
            case 2:
                // Urban Areas
                displaylvl3(dm.UASolidWasteLvl2, dm.UASolidWasteLvl3, dm.UASolidWasteLvl4, dm.UASolidWasteLvl5);

                // Canals
                displaylvl3(dm.CanalSolidWasteLvl2, dm.CanalSolidWasteLvl3, dm.CanalSolidWasteLvl4, dm.CanalSolidWasteLvl5);


                // Fields
                displaylvl3(dm.FieldSolidWasteLvl2, dm.FieldSolidWasteLvl3, dm.FieldSolidWasteLvl4, dm.FieldSolidWasteLvl5);
                
                break;
            case 3:
                // Urban Areas
                displaylvl4(dm.UASolidWasteLvl2, dm.UASolidWasteLvl3, dm.UASolidWasteLvl4, dm.UASolidWasteLvl5);

                // Canals
                displaylvl4(dm.CanalSolidWasteLvl2, dm.CanalSolidWasteLvl3, dm.CanalSolidWasteLvl4, dm.CanalSolidWasteLvl5);


                // Fields
                displaylvl4(dm.FieldSolidWasteLvl2, dm.FieldSolidWasteLvl3, dm.FieldSolidWasteLvl4, dm.FieldSolidWasteLvl5);
                
                break;
            default :
                // Urban Areas
                displaylvl5(dm.UASolidWasteLvl2, dm.UASolidWasteLvl3, dm.UASolidWasteLvl4, dm.UASolidWasteLvl5);

                // Canals
                displaylvl5(dm.CanalSolidWasteLvl2, dm.CanalSolidWasteLvl3, dm.CanalSolidWasteLvl4, dm.CanalSolidWasteLvl5);


                // Fields
                displaylvl5(dm.FieldSolidWasteLvl2, dm.FieldSolidWasteLvl3, dm.FieldSolidWasteLvl4, dm.FieldSolidWasteLvl5);
                
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
                displaylvl3(dm.CanalWasteWaterLvl2, dm.CanalWasteWaterLvl3, dm.CanalWasteWaterLvl4, dm.CanalWasteWaterLvl5);
                break;
            case 3:
                displaylvl4(dm.CanalWasteWaterLvl2, dm.CanalWasteWaterLvl3, dm.CanalWasteWaterLvl4, dm.CanalWasteWaterLvl5);
                break;
            default :
                displaylvl5(dm.CanalWasteWaterLvl2, dm.CanalWasteWaterLvl3, dm.CanalWasteWaterLvl4, dm.CanalWasteWaterLvl5);
                break;
        }
    }

    public void displayProductionClass(int classValue){
        switch(classValue){
            case 0:
                // code block
                break;
            case 1:
                // code block
                break;
            case 2:
                // code block
                break;
            case 3:
                // code block
                break;
            default :
                // code block
                break;
        }
    }
}

    