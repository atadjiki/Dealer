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

    public bool CanPerformAbility(AbilityID ability)
    {
        int cost = GetAbilityCost(ability);

        return (cost <= ActionPoints);
    }

    public void DerementActionPoints(AbilityID ability)
    {
        int cost = GetAbilityCost(ability);

        ActionPoints -= cost;

        ActionPoints = Mathf.Clamp(ActionPoints, 0, ActionPoints);
    }

    public void AdjustHealth(int amount)
    {
        Health += amount;

        Health = Mathf.Clamp(Health, 0, Health);
    }

    public void ResetForTurn(CharacterID ID)
    {
        CharacterData data = ResourceUtil.GetCharacterData(ID);

        ActiveAbility = AbilityID.NONE;
        ActiveTarget = null;
        ActiveDestination = Vector3.zero;

        ActionPoints = data.ActionPoints;
    }

    public string GetInfoString()
    {
        return
            "HP: " + Health + "\n" +
            "AP: " + ActionPoints + "\n"; 

    }

    public static CharacterStateData Build(CharacterID ID)
    {
        CharacterData data = ResourceUtil.GetCharacterData(ID);

        return new CharacterStateData()
        {
            ActiveAbility = AbilityID.NONE,
            ActiveTarget = null,
            ActiveDestination = Vector3.zero,
            Health = data.Health,
            ActionPoints = data.ActionPoints,
        };
    }
}

public class CharacterComponent : MonoBehaviour
{
    private CharacterID _ID;

    private CharacterNavigator _navigator;

    private CharacterAnimator _animator;
    private CharacterModel _model;

    private CharacterCameraFollow _cameraFollow;

    private CharacterStateData _state;

    //construct the character using their defined data
    public IEnumerator Coroutine_Setup(CharacterData data)
    {
        _ID = data.ID;

        _state = CharacterStateData.Build(_ID);

        //create a navigator for the character
        GameObject navigatorObject = new GameObject("Navigator");
        navigatorObject.transform.parent = this.transform;
        _navigator = navigatorObject.AddComponent<CharacterNavigator>();
        yield return new WaitUntil( () => _navigator != null );
        _navigator.DestinationReachedCallback += OnDestinationReached;
        _navigator.SetSpeed(data.MovementSpeed);

        //load the model and animator for this character
        GameObject modelObject = Instantiate<GameObject>(data.Model, navigatorObject.transform);
        _model = modelObject.GetComponent<CharacterModel>();
        yield return new WaitUntil(() => _model != null);
        _animator = modelObject.GetComponent<CharacterAnimator>();
        yield return new WaitUntil(() => _animator != null);

        //place a camera follow target for the character
        GameObject cameraFollowObject = new GameObject("Camera Follow");
        cameraFollowObject.transform.parent = navigatorObject.transform;
        cameraFollowObject.transform.localPosition = data.CameraFollowOffset;
        _cameraFollow = cameraFollowObject.AddComponent<CharacterCameraFollow>();
        yield return new WaitUntil(() => _cameraFollow != null);

        yield return null;
    }

    private void OnDestroy()
    {
        _navigator.DestinationReachedCallback -= OnDestinationReached;
    }

    //Movement interface
    public void MoveTo(Vector3 destination)
    {
        _navigator.MoveTo(destination);
        _animator.SetAnim(CharacterAnim.MOVING);
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

    public bool HasActionPoints()
    {
        return _state.HasActionPoints();
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

    public void SetActiveAbility(AbilityID ID)
    {
        _state.ActiveAbility = ID;
    }

    public CharacterID GetID()
    {
        return _ID;
    }

    //Gizmos
    private void OnDrawGizmos()
    {
        Handles.Label(GetWorldLocation(), _state.GetInfoString());
    }
}
