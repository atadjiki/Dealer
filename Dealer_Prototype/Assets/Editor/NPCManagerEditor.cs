using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(NPCManager))]
public class NPCManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NPCManager levelManager = (NPCManager)target;
        if (GUILayout.Button("Select Previous Character"))
        {
            
        }
        if (GUILayout.Button("Select Next Character"))
        {
            
        }
    }
}
