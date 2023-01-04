using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEditor;
using GameDelegates;
using UnityEngine;

public class NPCComponent : CharacterComponent
{
    protected Coroutine currentCoroutine;

    public NewDestination OnNewDestination;
    public MovementStateChanged OnMovementStateChanged;
    public NewCommand OnNewCommand;

    public override void ProcessSpawnData(CharacterSpawnData _data)
    {
        base.ProcessSpawnData(_data);

        spawnData.Team = Enumerations.Team.Neutral;
    }

    public override IEnumerator PerformInitialize()
    {
        yield return base.PerformInitialize();

        GameObject navigatorObject = Instantiate<GameObject>(PrefabLibrary.GetNavigatorComponent(), this.transform);
        NavigatorComponent navigator = navigatorObject.GetComponent<NavigatorComponent>();
        navigator.OnDestinationReachedDelegate += Stop;
        OnMovementStateChanged += navigator.HandleMovementState;
        OnNewDestination += navigator.SetDestination;
        navigator.Initialize(this);

        model.transform.parent = navigatorObject.transform;
        OnNewCommand += model.HandleCharacterAction;

        model.tag = "NPC";

        yield return null;
    }

    public void BeginMovement()
    {
        OnNewCommand.Invoke(Enumerations.CommandType.Move);
        OnMovementStateChanged.Invoke(Enumerations.MovementState.Moving);
    }

    public void Stop()
    {
        OnNewCommand.Invoke(Enumerations.CommandType.None);
        OnMovementStateChanged.Invoke(Enumerations.MovementState.Stopped);
    }

    public void GoTo(Vector3 location)
    {
        StopAllCoroutines();

        OnNewDestination.Invoke(location);

        BeginMovement();
    }

    public bool DoesShowNavDecals()
    {
        return spawnData.ShowNavDecals;
    }
}
