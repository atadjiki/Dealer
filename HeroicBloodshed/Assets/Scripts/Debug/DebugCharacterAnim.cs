//C# Example (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;
using static Constants;
using System;

public class DebugCharacterAnim : MonoBehaviour
{
    private CharacterComponent characterComponent;

    [SerializeField] private CharacterID Debug_CharacterID;
    [SerializeField] private WeaponID Debug_WeaponID;

    private void Awake()
    {
        Spawn();
    }

    public void Spawn()
    {
        //delete the existing character component
        if(characterComponent != null)
        {
            Destroy(characterComponent);
            characterComponent = null;
        }

        characterComponent = this.gameObject.AddComponent<CharacterComponent>();
        characterComponent.SetID(Debug_CharacterID);
        StartCoroutine(characterComponent.SpawnCharacter());
    }

    public void HandleEvent(object eventData, CharacterEvent characterEvent)
    {
        if (characterEvent == CharacterEvent.HIT)
        {
            eventData = WeaponDefinition.Get(Debug_WeaponID).CalculateDamage();
        }

        characterComponent.HandleEvent(eventData, characterEvent);
    }
}

[CustomEditor(typeof(DebugCharacterAnim))]
[CanEditMultipleObjects]
public class DebugCharacterAnimEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(Application.isPlaying)
        {
            DebugCharacterAnim debugCharacter = (DebugCharacterAnim)target;

            foreach(CharacterEvent characterEvent in Enum.GetValues(typeof(CharacterEvent)))
            {
                if (characterEvent == CharacterEvent.ABILITY)
                {
                    foreach (AbilityID abilityID in Enum.GetValues(typeof(AbilityID)))
                    {
                        if(abilityID != AbilityID.NONE)
                        {
                            if (GUILayout.Button(GetDisplayString(abilityID)))
                            {
                                debugCharacter.HandleEvent(abilityID, characterEvent);
                            }
                        }
                    }
                }
                else if (characterEvent == CharacterEvent.HIT)
                {
                    if (GUILayout.Button(GetDisplayString(characterEvent)))
                    {
                        debugCharacter.HandleEvent(null, characterEvent);
                    }
                }
                else
                {
                    if (GUILayout.Button(GetDisplayString(characterEvent)))
                    {
                        debugCharacter.HandleEvent(null, characterEvent);
                    }
                }

            }

            if (GUILayout.Button("Repawn"))
            {
                debugCharacter.Spawn();
            }
        }
    }
}