using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Constants;

[Serializable]
public struct CharacterStateData
{
    public AbilityID ActiveAbility;
    public CharacterComponent ActiveTarget;
    public Vector3 ActiveDestination;

    public int Health;
    public int ActionPoints;

    public bool IsAlive()
    {
        return Health > 0;
    }

    public bool IsDead()
    {
        return Health <= 0;
    }

    public bool HasActionPoints()
    {
        return ActionPoints > 0;
    }

    public bool CanAffordAbility(AbilityID ability)
    {
        int cost = GetAbilityCost(ability);

        return (cost <= ActionPoints);
    }

    public void DerementActionPoints(AbilityID ability)
    {
        int cost = GetAbilityCost(ability);

        ActionPoints -= cost;
    }

    public void AdjustHealth(int amount)
    {
        Health += amount;
    }

    public void ResetForTurn(CharacterID ID)
    {
        CharacterDefinition def = ResourceUtil.GetCharacterData(ID);

        ActiveAbility = AbilityID.NONE;
        ActiveTarget = null;
        ActiveDestination = Vector3.zero;

        ActionPoints = def.BaseActionPoints;
    }

    public string GetInfoString()
    {
        return
            "HP: " + Health + "\n" +
            "AP: " + ActionPoints + "\n"; 

    }

    public static CharacterStateData Build(CharacterID ID)
    {
        CharacterDefinition def = ResourceUtil.GetCharacterData(ID);

        return new CharacterStateData()
        {
            ActiveAbility = AbilityID.NONE,
            ActiveTarget = null,
            ActiveDestination = Vector3.zero,
            Health = def.BaseHealth,
            ActionPoints = def.BaseActionPoints
        };
    }
}

public class CharacterComponent : MonoBehaviour
{
    private CharacterID _ID;

    private CharacterNavigator _navigator;
    private CapsuleCollider _graphCollider;

    private CharacterAnimator _animator;
    private CharacterModel _model;

    private CharacterStateData _state;

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
        _navigator.SetSpeed(def.MovementSpeed);

        //place a capsule on the character for pathfinding purposes
        _graphCollider = navigatorObject.AddComponent<CapsuleCollider>();
        _graphCollider.isTrigger = true;
        _graphCollider.radius = ENV_TILE_SIZE / 2;
        _graphCollider.height = ENV_TILE_SIZE;

        //load the model and animator for this character
        GameObject modelPrefab = ResourceUtil.GetModelPrefab(def.GetRandomModel());

        GameObject modelObject = Instantiate<GameObject>(modelPrefab, navigatorObject.transform);
        _model = modelObject.GetComponent<CharacterModel>();
        _model.CreateHighlight(GetTeamByID(_ID));
        yield return new WaitUntil(() => _model != null);
        _animator = modelObject.GetComponent<CharacterAnimator>();
        yield return new WaitUntil(() => _animator != null);

        yield return null;
    }

    private void OnDestroy()
    {
        _navigator.DestinationReachedCallback -= OnDestinationReached;
    }

    public void OnDestinationReached(CharacterNavigator navigator)
    {
        _animator.SetAnim(CharacterAnim.IDLE);
        EnvironmentUtil.Scan();
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
        CharacterDefinition def = ResourceUtil.GetCharacterData(_ID);

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

    public void OnSelected()
    {
        _model.ToggleHighlighted(true);
        _graphCollider.enabled = false;
    }

    public void OnDeselected()
    {
        _model.ToggleHighlighted(false);
        _graphCollider.enabled = true;
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

        //TODO HandleEvent(CharacterEvent.MOVING);
        _animator.SetAnim(CharacterAnim.MOVING);

        yield return _navigator.Coroutine_MoveTo(_state.ActiveDestination);

        //TODO HandleEvent(CharacterEvent.STOPPED);
    }

    //Gizmos
    private void OnDrawGizmos()
    {
        Handles.Label(GetWorldLocation(), _state.GetInfoString());
    }
}
