//C# Example (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnvironmentScanner))]
[CanEditMultipleObjects]
public class EnvironmentScannerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnvironmentScanner scanner = (EnvironmentScanner)target;
        DrawDefaultInspector();

        if(GUILayout.Button("Scan"))
        {
            scanner.PerformScan();
        }
    }

    public void OnSceneGUI()
    {
        EditorGUILayout.LabelField("Title");

    }
}