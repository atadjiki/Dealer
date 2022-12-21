using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEditor;
using GameDelegates;
using UnityEngine;

public class NPCComponent : CharacterComponent
{
    protected Coroutine currentCoroutine;

    public NewDestination OnNewDestinationDelegate;
    public NewAction OnNewActionDelegate;

    public override void ProcessSpawnData(object _data)
    {
        NPCSpawnData npcData = (NPCSpawnData) _data;

        _modelID = npcData.ModelID;
    }

    public override IEnumerator PerformInitialize()
    {
        yield return base.PerformInitialize();

        GameObject navigatorObject = Instantiate<GameObject>(PrefabLibrary.GetNavigatorComponent(), this.transform);
        NavigatorComponent navigator = navigatorObject.GetComponent<NavigatorComponent>();
        navigator.OnDestinationReachedDelegate += Stop;
        OnNewActionDelegate += navigator.HandleCharacterAction;
        navigator.Initialize(this);

        model.transform.parent = navigatorObject.transform;
        OnNewActionDelegate += model.HandleCharacterAction;

        yield return null;
    }

    public void BeginMovement()
    {
        OnNewActionDelegate.Invoke(Enumerations.CharacterAction.Move);
    }

    public void Stop()
    {
        OnNewActionDelegate.Invoke(Enumerations.CharacterAction.None);
    }

    public void GoTo(Vector3 location)
    {
        StopAllCoroutines();

        OnNewDestinationDelegate.Invoke(location);

        BeginMovement();
    }
}
