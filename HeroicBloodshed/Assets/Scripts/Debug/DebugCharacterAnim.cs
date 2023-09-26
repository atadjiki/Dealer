//C# Example (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;
using static Constants;
using System;

public class DebugCharacterAnim : MonoBehaviour
{
    private CharacterComponent characterComponent;

    [Header("To Spawn")]
    [SerializeField] private CharacterDefinition DebugCharacter;
    [Header("Attacking Weapon")]
    [SerializeField] private WeaponID WeaponID;

    private void Awake()
    {
        Spawn();
    }

    public void Spawn()
    {
        //delete the existing character component
        if(characterComponent != null)
        {
            Destroy(characterComponent.gameObject);
            characterComponent = null;
        }

        GameObject characterObject = new GameObject("Debug " + DebugCharacter.ID);
        characterObject.transform.parent = this.transform;
        characterObject.transform.localRotation = Quaternion.identity;


        characterComponent = characterObject.AddComponent<CharacterComponent>();
        StartCoroutine(characterComponent.Coroutine_Spawn(DebugCharacter));
    }

    public void HandleEvent(object eventData, CharacterEvent characterEvent)
    {
        if (characterEvent == CharacterEvent.DAMAGE)
        {
            eventData = WeaponDefinition.Get(WeaponID).CalculateDamage();
        }
        else if(characterEvent == CharacterEvent.HIT)
        {
            DamageInfo damageInfo = new DamageInfo();
            eventData = damageInfo;
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

            GUILayout.Space(20);
            GUILayout.Label("Commands");

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
                else
                {
                    if (GUILayout.Button(GetDisplayString(characterEvent)))
                    {
                        debugCharacter.HandleEvent(null, characterEvent);
                    }
                }

            }

            if (GUILayout.Button("Respawn"))
            {
                debugCharacter.Spawn();
            }
        }
    }
}