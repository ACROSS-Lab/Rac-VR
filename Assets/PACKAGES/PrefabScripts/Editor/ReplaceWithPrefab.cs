using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ReplaceWithPrefabs : EditorWindow
{
    public List<GameObject> prefabs = new List<GameObject>();
    private SerializedObject serializedObject;
    private SerializedProperty prefabsProperty;

    [MenuItem("My project/ReplaceWithRandomPrefabs")]
    static void CreateReplaceWithRandomPrefabs()
    {
        EditorWindow.GetWindow<ReplaceWithPrefabs>();
    }

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        prefabsProperty = serializedObject.FindProperty("prefabs");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Random Prefabs");

        serializedObject.Update();
        EditorGUILayout.PropertyField(prefabsProperty, true);
        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Replace"))
        {
            var selection = Selection.gameObjects;

            for (var i = selection.Length - 1; i >= 0; --i)
            {
                var selected = selection[i];
                var randomPrefab = GetRandomPrefab();

                if (randomPrefab == null)
                {
                    Debug.LogError("No prefabs available");
                    break;
                }

                var prefabAssetType = PrefabUtility.GetPrefabAssetType(randomPrefab);
                GameObject newObject;

                if (prefabAssetType == PrefabAssetType.Regular || prefabAssetType == PrefabAssetType.Variant)
                {
                    newObject = (GameObject)PrefabUtility.InstantiatePrefab(randomPrefab);
                }
                else
                {
                    newObject = Instantiate(randomPrefab);
                    newObject.name = randomPrefab.name;
                }

                if (newObject == null)
                {
                    Debug.LogError("Error instantiating prefab");
                    break;
                }

                Undo.RegisterCreatedObjectUndo(newObject, "Replace With Random Prefabs");
                newObject.transform.parent = selected.transform.parent;
                newObject.transform.localPosition = selected.transform.localPosition;
                newObject.transform.localRotation = selected.transform.localRotation;
                newObject.transform.localScale = selected.transform.localScale;
                newObject.transform.SetSiblingIndex(selected.transform.GetSiblingIndex());
                Undo.DestroyObjectImmediate(selected);
            }
        }

        GUI.enabled = false;
        EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);
    }

    private GameObject GetRandomPrefab()
    {
        if (prefabs.Count == 0)
            return null;

        int randomIndex = Random.Range(0, prefabs.Count);
        return prefabs[randomIndex];
    }
}
