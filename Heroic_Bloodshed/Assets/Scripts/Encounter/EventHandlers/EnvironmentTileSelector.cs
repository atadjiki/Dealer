using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentTileSelector : EncounterEventHandler
{
    private GameObject _quad;

    protected override void OnAwake()
    {
        base.OnAwake();

        MaterialLibrary matLib = MaterialLibrary.Get();

        _quad = EnvironmentUtil.CreateTileQuad(matLib.TileSelector, this.transform);
        _quad.SetActive(false);
    }

    protected override void OnStateChangedCallback(EncounterStateData stateData)
    {
        if (stateData.GetCurrentState() == EncounterState.PERFORM_ACTION)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (_quad == null) return;

        EnvironmentTileRaycastInfo info;

        if(EnvironmentUtil.GetTileBeneathMouse(out info))
        {
            if(info.layer == EnvironmentLayer.CHARACTER || info.layer == EnvironmentLayer.GROUND)
            {
                _quad.transform.position = info.position;
                _quad.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    EncounterStateMachine.OnAbilityChosen.Invoke(AbilityID.MOVE_FULL, info.position);
                }

                return;
            }    
        }

        _quad.SetActive(false);
    }
}
