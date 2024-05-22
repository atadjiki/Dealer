using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentClickHandler : EncounterEventHandler
{
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            EnvironmentTileRaycastInfo info;

            if (EnvironmentUtil.GetTileBeneathMouse(out info))
            {
                if (IsLayerTraversible(info.layer))
                {
                    EncounterStateMachine.OnAbilityChosen.Invoke(AbilityID.MOVE_FULL, info.position);
                }
            }
        }
    }

    protected override void OnStateChangedCallback(Constants.EncounterState state)
    {
        if(state == Constants.EncounterState.PERFORM_ACTION)
        {
            Destroy(this.gameObject);
        }
    }
}
