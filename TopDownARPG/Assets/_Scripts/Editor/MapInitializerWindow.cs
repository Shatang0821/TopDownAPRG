using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapInitializerWindow : EditorWindow
{
    private int mapWidth = 16;
    private int mapHeight = 16;
    private GameObject parentObject;
    private List<GameObject> prefabList = new List<GameObject>();
    private List<bool> prefabWalkableList = new List<bool>();

    [MenuItem("Window/Map Initializer")]
    public static void ShowWindow()
    {
        GetWindow<MapInitializerWindow>("Map Initializer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Initialize Map", EditorStyles.boldLabel);

        mapWidth = EditorGUILayout.IntField("Map Width", mapWidth);
        mapHeight = EditorGUILayout.IntField("Map Height", mapHeight);
        parentObject = (GameObject)EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(GameObject), true);

        GUILayout.Label("Add Prefabs to List", EditorStyles.boldLabel);
        if (GUILayout.Button("Add Prefab"))
        {
            prefabList.Add(null);
            prefabWalkableList.Add(false);
        }

        for (int i = 0; i < prefabList.Count; i++)
        {
            prefabList[i] = (GameObject)EditorGUILayout.ObjectField($"Prefab {i + 1}", prefabList[i], typeof(GameObject), false);
            prefabWalkableList[i] = EditorGUILayout.Toggle("Walkable", prefabWalkableList[i]);
        }

        if (GUILayout.Button("Initialize"))
        {
            Dictionary<GameObject, bool> prefabTable = new Dictionary<GameObject, bool>();
            for (int i = 0; i < prefabList.Count; i++)
            {
                if (prefabList[i] != null)
                {
                    prefabTable[prefabList[i]] = prefabWalkableList[i];
                }
            }
            MapEditorWindow.ShowWindow(mapWidth, mapHeight, parentObject, prefabList,prefabWalkableList);
            Close();
        }
    }
}