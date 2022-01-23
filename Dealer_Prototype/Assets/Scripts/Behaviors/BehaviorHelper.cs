using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;
using static CharacterBehaviorScript;

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
            Destination = character.GetNavigatorComponent().transform.position
        };

        behaviorScript.Setup(data);

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

        behaviorScript.Setup(data);

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
            Destination = interactable.GetInteractionTransform().position
        };

        behaviorScript.Setup(data);

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
            Behavior = behaviorScript,
            Destination = Vector3.zero
        };

        behaviorScript.Setup(data);

        success = true;
        return behaviorScript;
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

    public static IEnumerator ResolvePerformInteraction(BehaviorData _data)
    {
        if (_data.Interactable.GetID() == InteractableConstants.InteractableID.Generic.ToString())
        {
            return PerformInteractWith(_data);
        }
        else if (_data.Interactable.GetID() == InteractableConstants.InteractableID.Chair.ToString())
        {
            return PerformSit(_data);
        }
        else if(_data.Interactable.GetID() == InteractableConstants.InteractableID.Television.ToString())
        {
            return PerformInteractWith(_data);
        }

        return null;
    }

    public static IEnumerator PerformApproachInteractable(BehaviorData _data)
    {
        float time_before = Time.time;

        if (_data.Character.GetNavigatorComponent().MoveToLocation(_data.Interactable.GetInteractionTransform().position))
        {
            yield return new WaitWhile(() => _data.Character.GetNavigatorComponent().State == NavigatorComponent.MovementState.Moving);

            DebugManager.Instance.Print(DebugManager.Log.LogBehavior, "Task took " + Mathf.Abs(Time.time - time_before) + " seconds");

            if (Vector3.Distance(_data.Character.GetNavigatorComponent().transform.position, _data.Interactable.GetInteractionTransform().position) < 0.1f)
            {
                _data.Character.GetNavigatorComponent().TeleportToLocation(_data.Interactable.GetInteractionTransform());
                DebugManager.Instance.Print(DebugManager.Log.LogBehavior, _data.Character.GetID() + " teleporting to  " + _data.Interactable.GetID());
            }

        }

        yield return null;
    }

    public static IEnumerator LerpToTransform(Transform A, Transform B, float speed)
    {
        //lerp NPC to interaction location
        Vector3 initialPosition = A.transform.position;
        Vector3 targetPosition = B.position;

        Quaternion initialRotation = A.rotation;
        Quaternion targetRotation = B.rotation;

        float lerp = 0;

        while (lerp < 1)
        {
            Vector3 lerpPosition = Vector3.Lerp(initialPosition, targetPosition, lerp);
            Quaternion lerpRotation = Quaternion.Lerp(initialRotation, targetRotation, lerp);

            lerp += Time.fixedDeltaTime * speed;

            A.position = lerpPosition;
            A.rotation = lerpRotation;

            yield return new WaitForFixedUpdate();
        }

        A.position = B.position;
        A.rotation = B.rotation;

        yield return null;
    }

    public static IEnumerator PerformSit(BehaviorData _data)
    {
        _data.Character.SetUpdateState(CharacterConstants.UpdateState.Busy);

        Chair chair = (Chair)_data.Interactable;

        if (chair != null)
        {
            Transform sittingTransform = chair.GetSittingPoseTransform();

            yield return LerpToTransform(_data.Character.GetNavigatorComponent().transform, sittingTransform, 1.5f);
            
            DebugManager.Instance.Print(DebugManager.Log.LogBehavior, _data.Character.GetID() + " teleporting to  " + _data.Interactable.GetID());

            _data.Character.FadeToAnimation(AnimationConstants.Animations.Sitting_Idle, 0.05f, false);

            DebugManager.Instance.Print(DebugManager.Log.LogBehavior, _data.Character.GetID() + " sitting on " + _data.Interactable.GetID());

            _data.Character.SetUpdateState(CharacterConstants.UpdateState.Ready);
        }

        yield return null;
    }

    //basic tasks
    public static IEnumerator PerformIdle(BehaviorData _data)
    {
        _data.Character.FadeToAnimation(AnimationConstants.Animations.Idle, 0.1f, false);

        yield return null;
    }

    public static IEnumerator PerformInteractWith(BehaviorData _data)
    {
        _data.Character.SetUpdateState(CharacterConstants.UpdateState.Busy);

        //move NPC to interaction location
        yield return LerpToTransform(_data.Character.GetNavigatorComponent().transform, _data.Interactable.GetInteractionTransform(), 2f);

        _data.Character.GetNavigatorComponent().TeleportToLocation(_data.Interactable.GetInteractionTransform());
        DebugManager.Instance.Print(DebugManager.Log.LogBehavior, _data.Character.GetID() + " teleporting to  " + _data.Interactable.GetID());

        //later we will fetch the appropriate anim
        _data.Character.FadeToAnimation(AnimationConstants.Animations.ButtonPush, 0.5f, false);

        yield return new WaitForSeconds(0.5f);
        _data.Interactable.OnInteraction();

        DebugManager.Instance.Print(DebugManager.Log.LogBehavior, _data.Character.GetID() + " interacting with " + _data.Interactable.GetID());
        yield return new WaitForSeconds(3.0f);

        _data.Character.FadeToAnimation(AnimationConstants.Animations.Idle, 0.25f, false);

        DebugManager.Instance.Print(DebugManager.Log.LogBehavior, _data.Character.GetID() + " finished interacting with " + _data.Interactable.GetID());
        _data.Character.SetUpdateState(CharacterConstants.UpdateState.Ready);
    }

    public static IEnumerator PerformMoveToDestination(BehaviorData _data)
    {
        float time_before = Time.time;

        if (_data.Character.GetNavigatorComponent().MoveToLocation(_data.Destination))
        {
            yield return new WaitWhile(() => _data.Character.GetNavigatorComponent().State == NavigatorComponent.MovementState.Moving);

            DebugManager.Instance.Print(DebugManager.Log.LogBehavior, "Task took " + Mathf.Abs(Time.time - time_before) + " seconds");
        }
    }

    public static IEnumerator PerformMoveToRandomLocation(BehaviorData _data)
    {
        _data.Character.GetNavigatorComponent().MoveToRandomLocation();

        yield return new WaitWhile(() => _data.Character.GetNavigatorComponent().State == NavigatorComponent.MovementState.Moving);
    }
}
