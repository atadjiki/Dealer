using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnvironmentUtil))]
public class EnvironmentUtilEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Scan and Process"))
        {
            EnvironmentUtil.ProcessEnvironment();
        }
    }
}
