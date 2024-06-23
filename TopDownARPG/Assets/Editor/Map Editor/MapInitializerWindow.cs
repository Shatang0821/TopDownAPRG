using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MapInitializerWindow : EditorWindow
{
    private int mapWidth = 16;              //マップの幅
    private int mapHeight = 16;             //マップの高さ

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
        
        if (GUILayout.Button("Initialize"))
        {
            MapEditorWindow.ShowWindow(mapWidth, mapHeight);
            Close();
        }
    }
}
