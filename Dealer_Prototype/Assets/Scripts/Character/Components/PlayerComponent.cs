using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : NPCComponent
{
    public override void ProcessSpawnData(object _data)
    {
        PlayerSpawnData playerData = (PlayerSpawnData)_data;

        _modelID = playerData.ModelID;
    }

    public override IEnumerator PerformInitialize()
    {
        return base.PerformInitialize();
    }
}
