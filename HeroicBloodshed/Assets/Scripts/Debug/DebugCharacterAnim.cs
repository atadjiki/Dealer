//C# Example (LookAtPointEditor.cs)
using UnityEngine;
using UnityEditor;
using static Constants;
using System;
using System.Collections;

public class DebugCharacterAnim : MonoBehaviour
{
    private CharacterComponent casterComponent;
    private CharacterComponent targetComponent;

    [Header("Caster")]
    [SerializeField] private CharacterDefinition casterDef;
    [SerializeField] private Transform casterPlate;
    [Header("Target")]
    [SerializeField] private CharacterDefinition targetDef;
    [SerializeField] private Transform targetPlate;

    private void Awake()
    {
        Spawn();
    }

    public void Spawn()
    {
        StartCoroutine(Coroutine_Spawn());
    }

    public IEnumerator Coroutine_Spawn()
    {
        //delete the existing character component
        DestroyCharacter(ref casterComponent);
        DestroyCharacter(ref targetComponent);

        casterComponent = SpawnCharacter(casterDef, casterPlate);
        targetComponent = SpawnCharacter(targetDef, targetPlate);

        yield return new WaitWhile(() => casterComponent == null);
        yield return new WaitWhile(() => targetComponent == null);

        casterComponent.RotateTowards(targetComponent);
        targetComponent.RotateTowards(casterComponent);
    }

    private void DestroyCharacter(ref CharacterComponent characterComponent)
    {
        if (characterComponent != null)
        {
            Destroy(characterComponent.gameObject);
            characterComponent = null;
        }
    }

    private CharacterComponent SpawnCharacter(CharacterDefinition characterDefinition, Transform plate)
    {
        GameObject characterObject = new GameObject("Debug " + characterDefinition.ID);
        characterObject.transform.parent = plate;
        characterObject.transform.localPosition = Vector3.zero;
        characterObject.transform.localRotation = Quaternion.identity;

        CharacterComponent characterComponent = characterObject.AddComponent<CharacterComponent>();
        StartCoroutine(characterComponent.Coroutine_Spawn(characterDefinition));

        return characterComponent;
    }

    public void HandleEvent(CharacterEvent characterEvent, object eventData = null)
    {
        if (characterEvent == CharacterEvent.DAMAGE)
        {
            eventData = WeaponDefinition.Get(targetDef.AllowedWeapons[0]).CalculateDamage(targetComponent, casterComponent);
        }
        else if(characterEvent == CharacterEvent.HIT_LIGHT || characterEvent == CharacterEvent.HIT_HARD)
        {
            DamageInfo damageInfo = new DamageInfo();
            eventData = damageInfo;
        }

        casterComponent.HandleEvent(characterEvent, eventData);
    }

    public void HandleAbility(AbilityID abilityID)
    {
        casterComponent.PerformAbility(abilityID, targetComponent, casterComponent.GetWorldLocation());
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
            GUILayout.Label("Character Events");

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

            GUILayout.Space(20);
            GUILayout.Label("Abilities");
            foreach (AbilityID abilityID in Enum.GetValues(typeof(AbilityID)))
            {
                switch (abilityID)
                {
                    case AbilityID.MoveFull:
                    case AbilityID.MoveHalf:
                    case AbilityID.NONE:
                        break;
                    default:
                    {
                        if (GUILayout.Button(GetDisplayString(abilityID)))
                        {
                            debugCharacter.HandleAbility(abilityID);
                        }
                        break;
                    }
                }

  
            }

            GUILayout.Space(20);
            if (GUILayout.Button("Respawn"))
            {
                debugCharacter.Spawn();
            }
        }
    }
}