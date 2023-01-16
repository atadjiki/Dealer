using System.Collections;
using System.Collections.Generic;
using Constants;
using GameDelegates;
using UnityEngine;

public class CutsceneCharacterComponent : CharacterComponent
{
    protected Coroutine currentCoroutine;

    public DestinationReached OnDestinationReached;
    public NewDestination OnNewDestination;
    public MovementStateChanged OnMovementStateChanged;
    public NewCommand OnNewCommand;

    private NavigatorComponent _navigator;

    public Enumerations.CharacterID CharacterID;

    public void ToggleVisiblity(bool flag)
    {
        model.gameObject.SetActive(flag);
    }

    public override IEnumerator PerformInitialize()
    {
        yield return base.PerformInitialize();

        GameObject navigatorObject = Instantiate<GameObject>(PrefabLibrary.GetNavigatorComponent(), this.transform);
        _navigator = navigatorObject.GetComponent<NavigatorComponent>();

        OnMovementStateChanged += _navigator.HandleMovementState;
        OnNewDestination += _navigator.SetDestination;
        _navigator.Initialize(this);

        model.transform.parent = navigatorObject.transform;
        OnNewCommand += model.HandleCharacterAction;

        yield return null;
    }

    public void BeginMovement()
    {
        _navigator.OnDestinationReachedDelegate += Stop;
        OnNewCommand.Invoke(Enumerations.CommandType.Move);
        OnMovementStateChanged.Invoke(Enumerations.MovementState.Moving);
    }

    public void Stop()
    {
        if (OnDestinationReached != null)
        {
            OnDestinationReached.Invoke();
            _navigator.OnDestinationReachedDelegate -= Stop;
        }

        OnNewCommand.Invoke(Enumerations.CommandType.None);
        OnMovementStateChanged.Invoke(Enumerations.MovementState.Stopped);
    }

    public void GoTo(Vector3 location)
    {
        StopAllCoroutines();

        OnNewDestination.Invoke(location);

        BeginMovement();
    }
}
