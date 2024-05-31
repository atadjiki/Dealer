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

        _quad = EnvironmentUtil.CreateTileQuad(MaterialLibrary.Get(MaterialID.DEST_HIGHLIGHT), this.transform, Vector3.one);
    }

    protected override void OnStateChangedCallback(EncounterStateData state)
    {
        if (state.GetCurrentState() == EncounterState.PERFORM_ACTION)
        {
            Destroy(this.gameObject);
        }
    }
}
