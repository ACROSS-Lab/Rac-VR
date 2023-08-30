using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ReplaceWithRandomPrefabs : EditorWindow
{
    public List<GameObject> prefabs = new List<GameObject>();

    [MenuItem("My project/ReplaceWithRandomPrefabs")]
    static void CreateReplaceWithRandomPrefabs()
    {
        EditorWindow.GetWindow<ReplaceWithRandomPrefabs>();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Random Prefabs");

        // Display the list of prefabs with remove buttons
        for (int i = 0; i < prefabs.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            prefabs[i] = (GameObject)EditorGUILayout.ObjectField(prefabs[i], typeof(GameObject), false);
            if (GUILayout.Button("Remove", GUILayout.Width(70)))
            {
                prefabs.RemoveAt(i);
                i--;
            }
            EditorGUILayout.EndHorizontal();
        }

        // Add prefab button
        if (GUILayout.Button("Add Prefab"))
        {
            prefabs.Add(null);
        }

        if (GUILayout.Button("Replace"))
        {
            var selection = Selection.gameObjects;

            for (var i = selection.Length - 1; i >= 0; --i)
            {
                var selected = selection[i];
                var randomPrefab = GetRandomPrefabFromList();

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

    private GameObject GetRandomPrefabFromList()
    {
        if (prefabs.Count == 0)
            return null;

        int randomIndex = Random.Range(0, prefabs.Count);
        return prefabs[randomIndex];
    }
}

[CustomEditor(typeof(ReplaceWithRandomPrefabs))]
public class ReplaceWithRandomPrefabsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ReplaceWithRandomPrefabs script = (ReplaceWithRandomPrefabs)target;

        EditorGUILayout.LabelField("Random Prefabs");

        // Display the list of prefabs with remove buttons
        for (int i = 0; i < script.prefabs.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            script.prefabs[i] = (GameObject)EditorGUILayout.ObjectField(script.prefabs[i], typeof(GameObject), false);
            if (GUILayout.Button("Remove", GUILayout.Width(70)))
            {
                script.prefabs.RemoveAt(i);
                i--;
            }
            EditorGUILayout.EndHorizontal();
        }

        // Add prefab button
        if (GUILayout.Button("Add Prefab"))
        {
            script.prefabs.Add(null);
        }
    }
}
