using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PlayableCharacterManager))]
public class PlayableCharacterManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayableCharacterManager manager = (PlayableCharacterManager)target;
        if (GUILayout.Button("Select Previous Character"))
        {
            
        }
        if (GUILayout.Button("Select Next Character"))
        {
            
        }
    }
}
