using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelManager levelManager = (LevelManager)target;
        if (GUILayout.Button("Add Current Level"))
        {
            levelManager.AddCurrentLevel();
        }
        if (GUILayout.Button("Switch to Current Level"))
        {
            levelManager.SwitchToCurrentLevel();
        }
    }
}
