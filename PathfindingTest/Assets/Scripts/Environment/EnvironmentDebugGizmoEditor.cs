//C# Example (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnvironmentDebugGizmo))]
[CanEditMultipleObjects]
public class EnvironmentDebugGizmoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnvironmentDebugGizmo debugGizmo = (EnvironmentDebugGizmo)target;
        DrawDefaultInspector();

        if(GUILayout.Button("Scan"))
        {
            debugGizmo.PerformScan();
        }
    }
}