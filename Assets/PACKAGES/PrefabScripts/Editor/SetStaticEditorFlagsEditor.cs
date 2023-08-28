using UnityEditor;
using UnityEngine;

public class SetStaticEditorFlagsEditor : Editor
{
    [MenuItem("Tools/Set Selections and Children as Static")]
    private static void SetSelectedObjectsStatic()
    {
        // Get the array of currently selected GameObjects in the Unity Editor
        GameObject[] selectedObjects = Selection.gameObjects;

        // Loop through the selected GameObjects and make them and their children static
        foreach (GameObject selectedObject in selectedObjects)
        {
            SetStaticRecursively(selectedObject, true);
        }

        // Refresh the hierarchy to update the GameObjects' static status in the Unity Editor
        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
    }

    private static void SetStaticRecursively(GameObject gameObject, bool isStatic)
    {
        // Set the static property for the current GameObject
        gameObject.isStatic = isStatic;

        // Loop through all the children of the current GameObject
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform child = gameObject.transform.GetChild(i);
            SetStaticRecursively(child.gameObject, isStatic);
        }
    }
}
