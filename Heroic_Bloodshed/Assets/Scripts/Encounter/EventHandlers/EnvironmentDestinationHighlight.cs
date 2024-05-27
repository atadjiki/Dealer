using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentDestinationHighlight : EncounterEventHandler
{
    private GameObject _quad;

    protected override void OnAwake()
    {
        base.OnAwake();

        MaterialLibrary matLib = MaterialLibrary.Get();

        _quad = EnvironmentUtil.CreateTileQuad(matLib.DestinationHighlight, this.transform, Vector3.one);
    }

    protected override void OnStateChangedCallback(EncounterStateData state)
    {
        if (state.GetCurrentState() == EncounterState.DESELECT_CURRENT_CHARACTER)
        {
            Destroy(this.gameObject);
        }
    }
}
