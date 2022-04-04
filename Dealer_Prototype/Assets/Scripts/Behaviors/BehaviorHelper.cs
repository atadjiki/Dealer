using System.Collections;
using Constants;
using UnityEngine;

using static CharacterBehaviorScript;

public class BehaviorHelper : MonoBehaviour
{
    public static CharacterBehaviorScript IdleBehavior(CharacterComponent character, out bool success)
    {
        CharacterBehaviorScript behaviorScript
            = CreateBehaviorObject(character.GetID() + " idle ", character).AddComponent<Behavior_Idle>();

        BehaviorData data = new CharacterBehaviorScript.BehaviorData
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

        BehaviorData data = new BehaviorData
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

    public static CharacterBehaviorScript ApproachCharacterBehavior(CharacterComponent characterA, CharacterComponent characterB, out bool success)
    {
        CharacterBehaviorScript behaviorScript
             = CreateBehaviorObject(characterA.GetID() + " talk " + characterB.GetID(), characterA).AddComponent<Behavior_ApproachCharacter>();

        BehaviorData data = new BehaviorData
        {
            Character = characterA,
            Interactee = characterB,
            Behavior = behaviorScript,
            Destination = characterB.GetNavigatorComponent().transform.position
        };

        behaviorScript.Setup(data);

        success = true;
        return behaviorScript;
    }

    public static CharacterBehaviorScript ApproachInteractableBehavior(CharacterComponent character, IInteraction interactable, out bool success)
    {
        CharacterBehaviorScript behaviorScript
             = CreateBehaviorObject(character.GetID() + " approach " + interactable.GetID(), character).AddComponent<Behavior_ApproachInteractable>();

        BehaviorData data = new BehaviorData
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

        BehaviorData data = new BehaviorData
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

    public static IEnumerator ResolvePerformNPCInteraction(BehaviorData _data)
    {
        return PerformConversation(_data);
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
        else if (_data.Interactable.GetID() == InteractableConstants.InteractableID.Television.ToString())
        {
            return PerformInteractWith(_data);
        }

        return null;
    }

    public static IEnumerator PerformApproachCharacter(BehaviorData _data)
    {
        float time_before = Time.time;

        _data.Character.SetAIState(AIConstants.AIState.Moving);

        Transform interacteeTransform = _data.Interactee.GetNavigatorComponent().transform;

        Vector3 interacteeOffset = interacteeTransform.forward * 1.5f;

        if (_data.Character.GetNavigatorComponent().MoveToLocation(interacteeTransform.position + interacteeOffset))
        {
            yield return new WaitWhile(() => _data.Character.GetNavigatorComponent().State == NavigatorComponent.MovementState.Moving);

            DebugManager.Instance.Print(DebugManager.Log.LogBehavior, "Task took " + Mathf.Abs(Time.time - time_before) + " seconds");

            if (Vector3.Distance(_data.Character.GetNavigatorComponent().transform.position, _data.Interactee.GetNavigatorComponent().transform.position) < 0.1f)
            {
                _data.Character.GetNavigatorComponent().TeleportToLocation(_data.Interactee.GetNavigatorComponent().transform);
                DebugManager.Instance.Print(DebugManager.Log.LogBehavior, _data.Character.GetID() + " teleporting to  " + _data.Interactee.GetID());
            }

            Vector3 relativePos = _data.Interactee.GetNavigatorComponent().transform.position - _data.Character.GetNavigatorComponent().transform.position;

            _data.Character.GetNavigatorComponent().transform.rotation
                = Quaternion.LookRotation(relativePos);

        }

        yield return null;
    }

    public static IEnumerator PerformApproachInteractable(BehaviorData _data)
    {
        float time_before = Time.time;

        _data.Character.SetAIState(AIConstants.AIState.Moving);

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
        _data.Character.SetUpdateState(AIConstants.UpdateState.Busy);

        Chair chair = (Chair)_data.Interactable;

        if (chair != null)
        {
            Transform sittingTransform = chair.GetSittingPoseTransform();

            yield return LerpToTransform(_data.Character.GetNavigatorComponent().transform, sittingTransform, 1.5f);

            DebugManager.Instance.Print(DebugManager.Log.LogBehavior, _data.Character.GetID() + " teleporting to  " + _data.Interactable.GetID());

            _data.Character.FadeToAnimation(AnimationConstants.Anim.Sitting_Default, 0.05f, false);

            DebugManager.Instance.Print(DebugManager.Log.LogBehavior, _data.Character.GetID() + " sitting on " + _data.Interactable.GetID());

            _data.Character.SetUpdateState(AIConstants.UpdateState.Ready);

            _data.Character.SetAIState(AIConstants.AIState.Sitting);
        }

        yield return null;
    }

    public static IEnumerator AttemptStand(BehaviorData _data)
    {
        if(_data.Character.AIState == AIConstants.AIState.Sitting)
        {
            _data.Character.SetUpdateState(AIConstants.UpdateState.Busy);

            DebugManager.Instance.Print(DebugManager.Log.LogBehavior, _data.Character.GetID() + " standing up");

            _data.Character.FadeToAnimation(AnimationConstants.Anim.Idle, 0.1f, true);

            yield return new WaitForSeconds(1.0f);

            _data.Character.SetUpdateState(AIConstants.UpdateState.Ready);

            _data.Character.SetAIState(AIConstants.AIState.Idle);
        }

        yield return null;
    }

    //basic tasks
    public static IEnumerator PerformIdle(BehaviorData _data)
    {
        _data.Character.SetAIState(AIConstants.AIState.Idle);

        _data.Character.FadeToAnimation(AnimationConstants.Anim.Idle, 0.1f, false);

        yield return null;
    }

    public static IEnumerator PerformConversation(BehaviorData _data)
    {
        ConversationManager.Instance.StartConversation();

        //set initiator state
        _data.Character.SetUpdateState(AIConstants.UpdateState.Busy);
        _data.Character.SetAIState(AIConstants.AIState.Interacting);
       
        //cache some data first
        Transform interacteeTransform = _data.Interactee.GetNavigatorComponent().transform;
        AIConstants.AIState interacteeAIState = _data.Interactee.AIState;
        AnimationConstants.Anim interacteeAnim = _data.Interactee.GetCurrentAnimation();

        //set interactee state
        _data.Interactee.SetAIState(AIConstants.AIState.Interacting);
        _data.Interactee.SetUpdateState(AIConstants.UpdateState.Busy);

        //perform the actual convo here
        //in the future we'll need anims per character instead of the default
        _data.Character.FadeToAnimation(AnimationConstants.Anim.Talking_Default, 0.5f, false);
        _data.Interactee.FadeToAnimation(AnimationConstants.Anim.Talking_Default, 0.5f, false);

        //Dialogue dialogue
        //    = new Dialogue(_data.Character, "Hello my dear friend", AnimationConstants.Anim.Talking_Default, 5.0f);

        //UIManager.Instance.HandleEvent(UI.Events.CharacterLine_Begin, dialogue);

        yield return new WaitForSeconds(5.0f);

        //dialogue
        //    = new Dialogue(_data.Interactee, "Good to see you colleague", AnimationConstants.Anim.Idle_Drunk, 5.0f);

        //UIManager.Instance.HandleEvent(UI.Events.CharacterLine_Begin, dialogue);

        yield return new WaitForSeconds(5.0f);

        //reset character
        _data.Character.FadeToAnimation(AnimationConstants.Anim.Idle, 0, false);
        _data.Character.SetAIState(interacteeAIState);
        _data.Character.SetUpdateState(AIConstants.UpdateState.Ready);

        //reset interactee
        _data.Interactee.FadeToAnimation(interacteeAnim, 0, false);
        _data.Interactee.GetNavigatorComponent().transform.rotation = interacteeTransform.rotation;
        _data.Interactee.SetUpdateState(AIConstants.UpdateState.Ready);

        ConversationManager.Instance.EndConversation();
    }

    public static IEnumerator PerformInteractWith(BehaviorData _data)
    {
        _data.Character.SetAIState(AIConstants.AIState.Interacting);

        _data.Character.SetUpdateState(AIConstants.UpdateState.Busy);

        //move NPC to interaction location
        yield return LerpToTransform(_data.Character.GetNavigatorComponent().transform, _data.Interactable.GetInteractionTransform(), 2f);

        _data.Character.GetNavigatorComponent().TeleportToLocation(_data.Interactable.GetInteractionTransform());
        DebugManager.Instance.Print(DebugManager.Log.LogBehavior, _data.Character.GetID() + " teleporting to  " + _data.Interactable.GetID());

        //later we will fetch the appropriate anim
        _data.Character.FadeToAnimation(AnimationConstants.Anim.Interaction_ButtonPush, 0.5f, false);

        yield return new WaitForSeconds(0.5f);
        _data.Interactable.OnInteraction();

        DebugManager.Instance.Print(DebugManager.Log.LogBehavior, _data.Character.GetID() + " interacting with " + _data.Interactable.GetID());
        yield return new WaitForSeconds(3.0f);

        _data.Character.FadeToAnimation(AnimationConstants.Anim.Idle, 0.0f, false);

        DebugManager.Instance.Print(DebugManager.Log.LogBehavior, _data.Character.GetID() + " finished interacting with " + _data.Interactable.GetID());
        _data.Character.SetUpdateState(AIConstants.UpdateState.Ready);
    }

    public static IEnumerator PerformMoveToDestination(BehaviorData _data)
    {
        float time_before = Time.time;

        _data.Character.SetAIState(AIConstants.AIState.Moving);
        if (_data.Character.GetNavigatorComponent().MoveToLocation(_data.Destination))
        {
            yield return new WaitWhile(() => _data.Character.GetNavigatorComponent().State == NavigatorComponent.MovementState.Moving);

            DebugManager.Instance.Print(DebugManager.Log.LogBehavior, "Task took " + Mathf.Abs(Time.time - time_before) + " seconds");
        }
    }

    public static IEnumerator PerformMoveToRandomLocation(BehaviorData _data)
    {
        _data.Character.SetAIState(AIConstants.AIState.Moving);
        _data.Character.GetNavigatorComponent().MoveToRandomLocation();

        yield return new WaitWhile(() => _data.Character.GetNavigatorComponent().State == NavigatorComponent.MovementState.Moving);
    }
}
