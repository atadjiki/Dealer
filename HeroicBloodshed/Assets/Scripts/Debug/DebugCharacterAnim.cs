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
            if (GUILayout.Button("Perform Attack"))
            {
                characterComponent.HandleEvent(AbilityID.Attack, Constants.CharacterEvent.PERFORM_ABILITY);
            }
            if (GUILayout.Button("Reload"))
            {
                characterComponent.HandleEvent(AbilityID.Reload, Constants.CharacterEvent.PERFORM_ABILITY);
            }
            if (GUILayout.Button("Skip Turn"))
            {
                characterComponent.HandleEvent(AbilityID.SkipTurn, Constants.CharacterEvent.PERFORM_ABILITY);
            }
            if (GUILayout.Button("Heal"))
            {
                characterComponent.HandleEvent(AbilityID.Heal, Constants.CharacterEvent.PERFORM_ABILITY);
            }
            if (GUILayout.Button("Kill"))
            {
                characterComponent.HandleEvent(null, Constants.CharacterEvent.KILLED);
            }
        }
    }
}