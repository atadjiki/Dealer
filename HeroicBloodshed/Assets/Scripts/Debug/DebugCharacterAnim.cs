//C# Example (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;
using static Constants;
using System;

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

            foreach(CharacterEvent characterEvent in Enum.GetValues(typeof(CharacterEvent)))
            {
                if (characterEvent == CharacterEvent.ABILITY)
                {
                    foreach (AbilityID abilityID in Enum.GetValues(typeof(AbilityID)))
                    {
                        if (GUILayout.Button(abilityID.ToString()))
                        {
                            characterComponent.HandleEvent(abilityID, characterEvent);
                        }
                    }
                }
                else if (characterEvent == CharacterEvent.HIT)
                {
                    if (GUILayout.Button(characterEvent.ToString()))
                    {
                        DamageInfo damageInfo = WeaponDefinition.Get(WeaponID.Pistol).CalculateDamage();
                        characterComponent.HandleEvent(damageInfo, characterEvent);
                    }
                }
                else
                {
                    if (GUILayout.Button(characterEvent.ToString()))
                    {
                        characterComponent.HandleEvent(null, characterEvent);
                    }
                }

            }
        }
    }
}