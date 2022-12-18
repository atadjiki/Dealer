using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : NPCComponent
{
    public delegate void OnPlayerSpawned(Transform transform);
    public static OnPlayerSpawned OnPlayerSpawnedDelegate;

    public override void ProcessSpawnData(object _data)
    {
        PlayerSpawnData playerData = (PlayerSpawnData)_data;

        _modelID = playerData.ModelID;
    }

    public override IEnumerator PerformInitialize()
    {
        yield return base.PerformInitialize();

        OnPlayerSpawnedDelegate.Invoke(model.transform);
    }

    protected override void Highlight()
    {
        MaterialHelper.SetPlayerOutline(model);
    }

    protected override void Unhighlight()
    {
        base.Unhighlight();
    }
}
