using UnityEditor;
using UnityEngine;

public class MapInitializerWindow : EditorWindow
{
    private int mapWidth = 32;
    private int mapHeight = 32;
    private float cubeSize = 1.0f; // 新增方块大小
    private GameObject parentObject;

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
        cubeSize = EditorGUILayout.FloatField("Cube Size", cubeSize); // 新增方块大小输入字段
        parentObject = (GameObject)EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(GameObject), true);

        if (GUILayout.Button("Initialize"))
        {
            MapEditorWindow.ShowWindow(mapWidth, mapHeight, cubeSize, parentObject);
            Close();
        }
    }
}