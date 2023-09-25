//C# Example (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;
using static Constants;

[CustomEditor(typeof(CharacterComponent))]
[CanEditMultipleObjects]
public class DebugCharacterAnim : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(Application.isPlaying)
        {
            CharacterComponent characterComponent = (CharacterComponent)target;
            if (GUILayout.Button("Setup"))
            {
                characterComponent.Debug_Spawn();
            }
            if (GUILayout.Button("Die"))
            {
                characterComponent.HandleEvent(null, Constants.CharacterEvent.DEAD);
            }
        }
    }
}