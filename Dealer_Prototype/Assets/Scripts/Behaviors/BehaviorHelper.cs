using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorHelper : MonoBehaviour
{
    public static CharacterBehaviorScript IdleBehavior(CharacterComponent character, out bool success)
    {
        CharacterBehaviorScript behaviorScript
            = CreateBehaviorObject(character.GetID() + " idle ", character).AddComponent<Behavior_Idle>();

        CharacterBehaviorScript.BehaviorData data = new CharacterBehaviorScript.BehaviorData
        {
            Character = character,
            Interactable = null,
            Behavior = behaviorScript,
            Destination = Vector3.zero
        };

        behaviorScript.BeginBehavior(data);

        success = true;
        return behaviorScript;
    }

    public static CharacterBehaviorScript MoveToBehavior(CharacterComponent character, Vector3 destination, out bool success)
    {
       CharacterBehaviorScript behaviorScript
            = CreateBehaviorObject(character.GetID() + " move to location " + destination.ToString(), character).AddComponent<Behavior_MoveToLocation>();

        CharacterBehaviorScript.BehaviorData data = new CharacterBehaviorScript.BehaviorData
        {
            Character = character,
            Interactable = null,
            Behavior = behaviorScript,
            Destination = destination
        };

        behaviorScript.BeginBehavior(data);

        success = true;
        return behaviorScript;
    }

    public static CharacterBehaviorScript ApproachBehavior(CharacterComponent character, Interactable interactable, out bool success)
    {
        CharacterBehaviorScript behaviorScript
             = CreateBehaviorObject(character.GetID() + " approach " + interactable.GetID(), character).AddComponent<Behavior_Approach>();

        CharacterBehaviorScript.BehaviorData data = new CharacterBehaviorScript.BehaviorData
        {
            Character = character,
            Interactable = interactable,
            Behavior = behaviorScript,
        };

        behaviorScript.BeginBehavior(data);

        success = true;
        return behaviorScript;
    }

    public static CharacterBehaviorScript MoveToRandomLocation(CharacterComponent character, out bool success)
    {
        CharacterBehaviorScript behaviorScript
                = CreateBehaviorObject(character.GetID() + " move to random location behavior ", character).AddComponent<Behavior_MoveToRandomLocation>();

        CharacterBehaviorScript.BehaviorData data = new CharacterBehaviorScript.BehaviorData
        {
            Character = character,
            Behavior = behaviorScript
        };

        behaviorScript.BeginBehavior(data);

        success = true;
        return behaviorScript;
    }

    public static CharacterBehaviorScript InteractWithBehavior(CharacterComponent character, Interactable interactable, out bool success)
    {
        if (interactable != null && interactable.HasBeenInteractedWith(character) == false)
        {
            CharacterBehaviorScript behaviorScript
                =  CreateBehaviorObject(character.GetID() + " - " + interactable.GetID() + " interaction behavior ", character).AddComponent<Behavior_InteractWith>();

            CharacterBehaviorScript.BehaviorData data = new CharacterBehaviorScript.BehaviorData
            {
                Character = character,
                Interactable = interactable,
                Behavior = behaviorScript,
                Destination = interactable.transform.position
            };

            behaviorScript.BeginBehavior(data);

            success = true;
            return behaviorScript;
        }

        success = false;
        return null;
    }

    public static GameObject CreateBehaviorObject(string name, CharacterComponent character)
    {
        GameObject behaviorObject = new GameObject(name);
        behaviorObject.transform.parent = character.transform;

        return behaviorObject;
    }

    public static bool IsInteractionAllowed(CharacterComponent character, Interactable interactable)
    {
        Vector3 characterPos = character.GetNavigatorComponent().transform.position;
        Vector3 interactionPos = interactable.GetInteractionTransform().position;

        return (Vector3.Distance(characterPos, interactionPos) < 1f);
    }
}
