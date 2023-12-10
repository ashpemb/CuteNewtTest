using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Test))]
public class WFCEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Test myScript = (Test)target;
        if (GUILayout.Button("Create Tilemap"))
        {
            myScript.CreateWFC();
            myScript.CreateTilemap();
        }
        
        if (GUILayout.Button("Clear Tilemap"))
        {
            myScript.ClearOutputTilemap();
        }
    }
}
