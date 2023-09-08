using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ConnectionClass
{
    public List<int> solidwasteClass;
    public List<int> waterwasteClass;
    public List<int> productionClass;
    public List<float> waterwaste;

    public static ConnectionClass CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ConnectionClass>(jsonString);
    }

    public void displaySolidClass(int classValue){
        switch(classValue){
            case 0:
                // Urban Areas
                if (UASolidWasteLvl2[0].activeSelf) {
                    foreach(GameObject item in UASolidWasteLvl2) {item.SetActive(false);}

                    if (UASolidWasteLvl3[0].activeSelf) {
                        foreach(GameObject item in UASolidWasteLvl3) {item.SetActive(false);}

                        if (UASolidWasteLvl4[0].activeSelf) {
                            foreach(GameObject item in UASolidWasteLvl4) {item.SetActive(false);}

                            if (UASolidWasteLvl5[0].activeSelf) {
                                foreach(GameObject item in UASolidWasteLvl5) {item.SetActive(false);}
                            }
                        }
                    }
                }


                // Canals
                if (CanalSolidWasteLvl2[0].activeSelf) {
                    foreach(GameObject item in CanalSolidWasteLvl2) {item.SetActive(false);}

                    if (CanalSolidWasteLvl3[0].activeSelf) {
                        foreach(GameObject item in CanalSolidWasteLvl3) {item.SetActive(false);}

                        if (CanalSolidWasteLvl4[0].activeSelf) {
                            foreach(GameObject item in CanalSolidWasteLvl4) {item.SetActive(false);}

                            if (CanalSolidWasteLvl5[0].activeSelf) {
                                foreach(GameObject item in CanalSolidWasteLvl5) {item.SetActive(false);}
                            }
                        }
                    }
                }

                // Fields
                if (FieldSolidWasteLvl2[0].activeSelf) {
                    foreach(GameObject item in FieldSolidWasteLvl2) {item.SetActive(false);}

                    if (FieldSolidWasteLvl3[0].activeSelf) {
                        foreach(GameObject item in FieldSolidWasteLvl3) {item.SetActive(false);}

                        if (FieldSolidWasteLvl4[0].activeSelf) {
                            foreach(GameObject item in FieldSolidWasteLvl4) {item.SetActive(false);}

                            if (FieldSolidWasteLvl5[0].activeSelf) {
                                foreach(GameObject item in FieldSolidWasteLvl5) {item.SetActive(false);}
                            }
                        }
                    }
                }
                break;

            case 1:
                // Urban Areas
                if (UASolidWasteLvl2[0].activeSelf) {
                    if (UASolidWasteLvl3[0].activeSelf) {
                        foreach(GameObject item in UASolidWasteLvl3) {item.SetActive(false);}

                        if (UASolidWasteLvl4[0].activeSelf) {
                            foreach(GameObject item in UASolidWasteLvl4) {item.SetActive(false);}

                            if (UASolidWasteLvl5[0].activeSelf) {
                                foreach(GameObject item in UASolidWasteLvl5) {item.SetActive(false);}
                            }
                        }
                    }
                } 
                else {
                    foreach(GameObject item in UASolidWasteLvl2) {item.SetActive(true);}
                }

                // Canals
                if (CanalSolidWasteLvl2[0].activeSelf) {
                    if (CanalSolidWasteLvl3[0].activeSelf) {
                        foreach(GameObject item in CanalSolidWasteLvl3) {item.SetActive(false);}

                        if (CanalSolidWasteLvl4[0].activeSelf) {
                            foreach(GameObject item in CanalSolidWasteLvl4) {item.SetActive(false);}

                            if (CanalSolidWasteLvl5[0].activeSelf) {
                                foreach(GameObject item in CanalSolidWasteLvl5) {item.SetActive(false);}
                            }
                        }
                    }
                } 
                else {
                    foreach(GameObject item in CanalSolidWasteLvl2) {item.SetActive(true);}
                }

                // Fields
                if (FieldSolidWasteLvl2[0].activeSelf) {
                    if (FieldSolidWasteLvl3[0].activeSelf) {
                        foreach(GameObject item in FieldSolidWasteLvl3) {item.SetActive(false);}

                        if (FieldSolidWasteLvl4[0].activeSelf) {
                            foreach(GameObject item in FieldSolidWasteLvl4) {item.SetActive(false);}

                            if (FieldSolidWasteLvl5[0].activeSelf) {
                                foreach(GameObject item in FieldSolidWasteLvl5) {item.SetActive(false);}
                            }
                        }
                    }
                } 
                else {
                    foreach(GameObject item in FieldSolidWasteLvl2) {item.SetActive(true);}
                }
                break;
            case 2:
                // Urban Areas
                if (UASolidWasteLvl2[0].activeSelf) {
                    if (UASolidWasteLvl3[0].activeSelf) {
                        if (UASolidWasteLvl4[0].activeSelf) {
                            foreach(GameObject item in UASolidWasteLvl4) {item.SetActive(false);}

                            if (UASolidWasteLvl5[0].activeSelf) {
                                foreach(GameObject item in UASolidWasteLvl5) {item.SetActive(false);}
                            }
                        }
                    }
                    else 
                    {
                        foreach(GameObject item in UASolidWasteLvl3) {item.SetActive(true);}
                    }
                } 
                else {
                    foreach(GameObject item in UASolidWasteLvl2) {item.SetActive(true);}
                    foreach(GameObject item in UASolidWasteLvl3) {item.SetActive(true);}
                }

                // Canals
                if (CanalSolidWasteLvl2[0].activeSelf) {
                    if (CanalSolidWasteLvl3[0].activeSelf) {
                        if (CanalSolidWasteLvl4[0].activeSelf) {
                            foreach(GameObject item in CanalSolidWasteLvl4) {item.SetActive(false);}

                            if (CanalSolidWasteLvl5[0].activeSelf) {
                                foreach(GameObject item in CanalSolidWasteLvl5) {item.SetActive(false);}
                            }
                        }
                    }
                    else 
                    {
                        foreach(GameObject item in CanalSolidWasteLvl3) {item.SetActive(true);}
                    }
                } 
                else {
                    foreach(GameObject item in CanalSolidWasteLvl2) {item.SetActive(true);}
                    foreach(GameObject item in CanalSolidWasteLvl3) {item.SetActive(true);}
                }

                // Fields
                if (FieldSolidWasteLvl2[0].activeSelf) {
                    if (FieldSolidWasteLvl3[0].activeSelf) {
                        if (FieldSolidWasteLvl4[0].activeSelf) {
                            foreach(GameObject item in FieldSolidWasteLvl4) {item.SetActive(false);}

                            if (FieldSolidWasteLvl5[0].activeSelf) {
                                foreach(GameObject item in FieldSolidWasteLvl5) {item.SetActive(false);}
                            }
                        }
                    }
                    else 
                    {
                        foreach(GameObject item in FieldSolidWasteLvl3) {item.SetActive(true);}
                    }
                } 
                else {
                    foreach(GameObject item in FieldSolidWasteLvl2) {item.SetActive(true);}
                    foreach(GameObject item in FieldSolidWasteLvl3) {item.SetActive(true);}
                }
                break;
            case 3:
                // Urban Areas
                if (!UASolidWasteLvl5[0].activeSelf) {
                    if (!UASolidWasteLvl4[0].activeSelf) {
                        foreach(GameObject item in UASolidWasteLvl4) {item.SetActive(true);}

                        if (!UASolidWasteLvl3[0].activeSelf) {
                            foreach(GameObject item in UASolidWasteLvl3) {item.SetActive(true);}

                            if (!UASolidWasteLvl2[0].activeSelf) {
                                foreach(GameObject item in UASolidWasteLvl2) {item.SetActive(true);}
                            }
                        } 
                    }
                } 
                else {
                    foreach(GameObject item in UASolidWasteLvl5) {item.SetActive(false);}
                }

                // Canals
                if (!CanalSolidWasteLvl5[0].activeSelf) {
                    if (!CanalSolidWasteLvl4[0].activeSelf) {
                        foreach(GameObject item in CanalSolidWasteLvl4) {item.SetActive(true);}

                        if (!CanalSolidWasteLvl3[0].activeSelf) {
                            foreach(GameObject item in CanalSolidWasteLvl3) {item.SetActive(true);}

                            if (!CanalSolidWasteLvl2[0].activeSelf) {
                                foreach(GameObject item in CanalSolidWasteLvl2) {item.SetActive(true);}
                            }
                        } 
                    }
                } 
                else {
                    foreach(GameObject item in CanalSolidWasteLvl5) {item.SetActive(false);}
                }

                // Fields
                if (!FieldSolidWasteLvl5[0].activeSelf) {
                    if (!FieldSolidWasteLvl4[0].activeSelf) {
                        foreach(GameObject item in FieldSolidWasteLvl4) {item.SetActive(true);}

                        if (!FieldSolidWasteLvl3[0].activeSelf) {
                            foreach(GameObject item in FieldSolidWasteLvl3) {item.SetActive(true);}

                            if (!FieldSolidWasteLvl2[0].activeSelf) {
                                foreach(GameObject item in FieldSolidWasteLvl2) {item.SetActive(true);}
                            }
                        } 
                    }
                } 
                else {
                    foreach(GameObject item in FieldSolidWasteLvl5) {item.SetActive(false);}
                }
                break;
            default :
                //Urban Areas
                if (!UASolidWasteLvl5[0].activeSelf) {
                    foreach(GameObject item in UASolidWasteLvl5) {item.SetActive(true);}

                    if (!UASolidWasteLvl4[0].activeSelf) {
                        foreach(GameObject item in UASolidWasteLvl4) {item.SetActive(true);}

                        if (!UASolidWasteLvl3[0].activeSelf) {
                            foreach(GameObject item in UASolidWasteLvl3) {item.SetActive(true);}

                            if (!UASolidWasteLvl2[0].activeSelf) {
                                foreach(GameObject item in UASolidWasteLvl2) {item.SetActive(true);}
                            }
                        } 
                    }
                } 
                
                //Canals
                if (!CanalSolidWasteLvl5[0].activeSelf) {
                    foreach(GameObject item in CanalSolidWasteLvl5) {item.SetActive(true);}

                    if (!CanalSolidWasteLvl4[0].activeSelf) {
                        foreach(GameObject item in CanalSolidWasteLvl4) {item.SetActive(true);}

                        if (!CanalSolidWasteLvl3[0].activeSelf) {
                            foreach(GameObject item in CanalSolidWasteLvl3) {item.SetActive(true);}

                            if (!CanalSolidWasteLvl2[0].activeSelf) {
                                foreach(GameObject item in CanalSolidWasteLvl2) {item.SetActive(true);}
                            }
                        } 
                    }
                } 

                //Fields
                if (!FieldSolidWasteLvl5[0].activeSelf) {
                    foreach(GameObject item in FieldSolidWasteLvl5) {item.SetActive(true);}

                    if (!FieldSolidWasteLvl4[0].activeSelf) {
                        foreach(GameObject item in FieldSolidWasteLvl4) {item.SetActive(true);}

                        if (!FieldSolidWasteLvl3[0].activeSelf) {
                            foreach(GameObject item in FieldSolidWasteLvl3) {item.SetActive(true);}

                            if (!FieldSolidWasteLvl2[0].activeSelf) {
                                foreach(GameObject item in FieldSolidWasteLvl2) {item.SetActive(true);}
                            }
                        } 
                    }
                } 
                break;
        }
    }

    public void displayWaterClass(int classValue){
        switch(classValue){
            case 0:
                // Canals (Dead fish)
                if (CanalWasteWaterLvl2[0].activeSelf) {
                    foreach(GameObject item in CanalWasteWaterLvl2) {item.SetActive(false);}

                    if (CanalWasteWaterLvl3[0].activeSelf) {
                        foreach(GameObject item in CanalWasteWaterLvl3) {item.SetActive(false);}

                        if (CanalWasteWaterLvl4[0].activeSelf) {
                            foreach(GameObject item in CanalWasteWaterLvl4) {item.SetActive(false);}

                            if (CanalWasteWaterLvl5[0].activeSelf) {
                                foreach(GameObject item in CanalWasteWaterLvl5) {item.SetActive(false);}
                            }
                        }
                    }
                }
                break;
            case 1:
                // Canals (Dead fish)
                if (CanalWasteWaterLvl2[0].activeSelf) {
                    if (CanalWasteWaterLvl3[0].activeSelf) {
                        foreach(GameObject item in CanalWasteWaterLvl3) {item.SetActive(false);}

                        if (CanalWasteWaterLvl4[0].activeSelf) {
                            foreach(GameObject item in CanalWasteWaterLvl4) {item.SetActive(false);}

                            if (CanalWasteWaterLvl5[0].activeSelf) {
                                foreach(GameObject item in CanalWasteWaterLvl5) {item.SetActive(false);}
                            }
                        }
                    }
                } 
                else {
                    foreach(GameObject item in CanalWasteWaterLvl2) {item.SetActive(true);}
                }
                break;
            case 2:
                if (CanalWasteWaterLvl2[0].activeSelf) {
                    if (CanalWasteWaterLvl3[0].activeSelf) {
                        if (CanalWasteWaterLvl4[0].activeSelf) {
                            foreach(GameObject item in CanalWasteWaterLvl4) {item.SetActive(false);}

                            if (CanalWasteWaterLvl5[0].activeSelf) {
                                foreach(GameObject item in CanalWasteWaterLvl5) {item.SetActive(false);}
                            }
                        }
                    }
                    else 
                    {
                        foreach(GameObject item in CanalWasteWaterLvl3) {item.SetActive(true);}
                    }
                } 
                else {
                    foreach(GameObject item in CanalWasteWaterLvl2) {item.SetActive(true);}
                    foreach(GameObject item in CanalWasteWaterLvl3) {item.SetActive(true);}
                }
                break;
            case 3:
                if (!CanalWasteWaterLvl5[0].activeSelf) {
                    if (!CanalWasteWaterLvl4[0].activeSelf) {
                        foreach(GameObject item in CanalWasteWaterLvl4) {item.SetActive(true);}

                        if (!CanalWasteWaterLvl3[0].activeSelf) {
                            foreach(GameObject item in CanalWasteWaterLvl3) {item.SetActive(true);}

                            if (!CanalWasteWaterLvl2[0].activeSelf) {
                                foreach(GameObject item in CanalWasteWaterLvl2) {item.SetActive(true);}
                            }
                        } 
                    }
                } 
                else {
                    foreach(GameObject item in CanalWasteWaterLvl5) {item.SetActive(false);}
                }
                break;
            default :
                if (!CanalWasteWaterLvl5[0].activeSelf) {
                    foreach(GameObject item in CanalWasteWaterLvl5) {item.SetActive(true);}

                    if (!CanalWasteWaterLvl4[0].activeSelf) {
                        foreach(GameObject item in CanalWasteWaterLvl4) {item.SetActive(true);}

                        if (!CanalWasteWaterLvl3[0].activeSelf) {
                            foreach(GameObject item in CanalWasteWaterLvl3) {item.SetActive(true);}

                            if (!CanalWasteWaterLvl2[0].activeSelf) {
                                foreach(GameObject item in CanalWasteWaterLvl2) {item.SetActive(true);}
                            }
                        } 
                    }
                } 
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

    