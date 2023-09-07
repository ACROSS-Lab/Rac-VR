using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ConnectionClass
{
    public List<int> solidwaste;
    public List<int> waterwaste;
    public List<int> production;

    public static ConnectionClass CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ConnectionClass>(jsonString);
    }

    public void displaySolidClass(int classValue){
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

    public void displayWaterClass(int classValue){
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

    