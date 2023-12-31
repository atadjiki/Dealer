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

    public void HandleEvent(CharacterEvent characterEvent, object eventData = null)
    {
        if (characterEvent == CharacterEvent.DAMAGE)
        {
            eventData = WeaponDefinition.Get(WeaponID).CalculateDamage(null, characterComponent);
        }
        else if(characterEvent == CharacterEvent.HIT_LIGHT || characterEvent == CharacterEvent.HIT_HARD)
        {
            DamageInfo damageInfo = new DamageInfo();
            eventData = damageInfo;
        }

        characterComponent.HandleEvent(characterEvent, eventData);
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
                switch (characterEvent)
                {
                    case CharacterEvent.SELECTED:
                    case CharacterEvent.DESELECTED:
                    case CharacterEvent.TARGETED:
                    case CharacterEvent.UNTARGETED:
                    case CharacterEvent.DAMAGE:
                        break;
                    default:
                    if (GUILayout.Button(GetDisplayString(characterEvent)))
                    {
                        debugCharacter.HandleEvent(characterEvent);
                    }
                    break;
                }

            }

            if (GUILayout.Button("Respawn"))
            {
                debugCharacter.Spawn();
            }
        }
    }
}