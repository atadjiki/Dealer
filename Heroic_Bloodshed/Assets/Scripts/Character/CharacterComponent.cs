using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Constants;

public class CharacterComponent : MonoBehaviour, ICharacterEventHandler
{
    private CharacterID _ID;

    private CharacterNavigator _navigator;
    private CapsuleCollider _graphCollider;

    private CharacterAnimator _animator;
    private CharacterModel _model;

    private CharacterStateData _state;

    private List<ICharacterEventHandler> _eventHandlers;

    //construct the character using their defined data
    public IEnumerator Coroutine_Setup(CharacterDefinition def)
    {
        _ID = def.ID;

        _state = CharacterStateData.Build(_ID);

        this.gameObject.layer = LAYER_CHARACTER;

        //create a navigator for the character
        GameObject navigatorObject = new GameObject("Navigator");
        navigatorObject.transform.parent = this.transform;
        navigatorObject.layer = LAYER_CHARACTER;
        _navigator = navigatorObject.AddComponent<CharacterNavigator>();
        yield return new WaitUntil( () => _navigator != null );
        _navigator.DestinationReachedCallback += OnDestinationReached;
        _navigator.OnBeginMovementPathCallback += OnBeginMovementPath;
        _navigator.SetSpeed(def.MovementSpeed);

        //place a capsule on the character for pathfinding purposes
        _graphCollider = navigatorObject.AddComponent<CapsuleCollider>();
        _graphCollider.isTrigger = true;
        _graphCollider.radius = ENV_TILE_SIZE / 2;
        _graphCollider.height = ENV_TILE_SIZE;

        //load the model and animator for this character
        GameObject modelObject = Instantiate<GameObject>(def.GetRandomModelPrefab(), navigatorObject.transform);
        _model = modelObject.GetComponent<CharacterModel>();
        _model.CreateHighlight(_state.CurrentTeam);
        yield return new WaitUntil(() => _model != null);
        _animator = modelObject.GetComponent<CharacterAnimator>();
        yield return new WaitUntil(() => _animator != null);

        _eventHandlers = new List<ICharacterEventHandler>();

        foreach(ICharacterEventHandler eventHandler in GetComponentsInChildren<ICharacterEventHandler>())
        {
            _eventHandlers.Add(eventHandler);
        }

        _eventHandlers.Remove(this);

        yield return null;
    }

    private void OnDestroy()
    {
        _navigator.DestinationReachedCallback -= OnDestinationReached;
        _navigator.OnBeginMovementPathCallback -= OnBeginMovementPath;
    }

    public void HandleEvent(CharacterEvent characterEvent, object eventData = null)
    {
        switch(characterEvent)
        {
            case CharacterEvent.SELECTED:
                _graphCollider.enabled = false;
                break;
            case CharacterEvent.DESELECTED:
                _graphCollider.enabled = true;
                break;
            case CharacterEvent.DESTINATION_REACHED:
                EnvironmentUtil.Scan();
                break;
            default:
                break;
        }

        //broadcast the event to children
        foreach(ICharacterEventHandler eventHandler in _eventHandlers)
        {
            eventHandler.HandleEvent(characterEvent, eventData);
        }
    }

    public void OnDestinationReached(CharacterNavigator navigator)
    {
        HandleEvent(CharacterEvent.DESTINATION_REACHED, navigator);
    }

    public void OnBeginMovementPath(MovementInfo info)
    {
        HandleEvent(CharacterEvent.MOVEMENT_BEGIN, info);
    }

    public void Teleport(Vector3 destination)
    {
        _navigator.Teleport(destination);
    }

    public Vector3 GetWorldLocation()
    {
        return _navigator.GetWorldLocation();
    }

    public CharacterNavigator GetNavigator()
    {
        return _navigator;
    }

    public CharacterModel GetModel()
    {
        return _model;
    }

    public void ToggleVisibility(bool flag)
    {
        _model.ToggleVisibility(flag);
    }

    //State Interface
    public bool IsAlive()
    {
        return _state.IsAlive();
    }

    public bool IsDead()
    {
        return _state.IsDead();
    }

    public int GetRange(MovementRangeType rangeType)
    {
        CharacterDefinition def = ResourceUtil.GetCharacterDefinition(_ID);

        switch (rangeType)
        {
            case MovementRangeType.HALF:
                return def.MovementRange;
            case MovementRangeType.FULL:
                return def.MovementRange * 2;
            default:
                return 0;
        }
    }

    public int GetMaxRange()
    {
        if (CanAffordAbility(AbilityID.MOVE_FULL))
        {
            return GetRange(MovementRangeType.FULL);
        }
        else if (CanAffordAbility(AbilityID.MOVE_HALF))
        {
            return GetRange(MovementRangeType.HALF);
        }
        else
        {
            return 0;
        }
    }

    public bool IsWithinRange(int distance)
    {
        return (distance >= 0 && distance <= GetMaxRange());
    }

    public bool HasActionPoints()
    {
        return _state.HasActionPoints();
    }

    public bool CanAffordAbility(AbilityID ID)
    {
        return _state.CanAffordAbility(ID);
    }    

    public void DecrementActionPoints(AbilityID ID)
    {
        _state.DerementActionPoints(ID);
    }

    public void ResetForTurn()
    {
        _state.ResetForTurn(_ID);
    }

    public void SetActiveDestination(Vector3 destination)
    {
        _state.ActiveDestination = destination;
    }

    public AbilityID GetActiveAbility()
    {
        return _state.ActiveAbility;
    }

    public void SetActiveAbility(AbilityID ID)
    {
        _state.ActiveAbility = ID;
    }

    public CharacterID GetID()
    {
        return _ID;
    }

    public TeamID GetTeam()
    {
        return _state.CurrentTeam;
    }

    public void ResetTeam()
    {
        _state.ResetTeam();
    }

    public void SetTeam(TeamID team)
    {
        _state.CurrentTeam = team;
    }

    public IEnumerator Coroutine_PerformAbility()
    {
        yield return Coroutine_PerformAbility(GetActiveAbility());
    }

    public IEnumerator Coroutine_PerformAbility(AbilityID abilityID)
    {
        switch(abilityID)
        {
            case AbilityID.MOVE_FULL:
            case AbilityID.MOVE_HALF:
                yield return Coroutine_HandleAbility_Move();
                break;
            default:
                break;
        }

        DecrementActionPoints(abilityID);

        yield return null;
    }

    private IEnumerator Coroutine_HandleAbility_Move()
    {
        Debug.Log("Handling Ability: Move");

        CameraRig.Instance.Follow(this);

        yield return new WaitForSeconds(0.2f);

        ////TODO HandleEvent(CharacterEvent.MOVING);
        //_animator.SetAnim(CharacterAnim.MOVING);

        yield return _navigator.Coroutine_MoveTo(_state.ActiveDestination);

        //TODO HandleEvent(CharacterEvent.STOPPED);
    }

    //Gizmos
    private void OnDrawGizmos()
    {
        Handles.Label(GetWorldLocation(), _state.GetInfoString());
    }
}
