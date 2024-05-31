using Pathfinding;
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

        _quad = EnvironmentUtil.CreateTileQuad(MaterialLibrary.Get(MaterialID.TILE_SELECTOR), this.transform);
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

        TileNode node;
        if(EnvironmentUtil.GetNodeBeneathMouse(out node))
        {
            if(node.Layer == EnvironmentLayer.CHARACTER || node.Layer == EnvironmentLayer.GROUND)
            {
                _quad.transform.position = (Vector3) node.position;
                _quad.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    EncounterStateMachine.OnAbilityChosen.Invoke(AbilityID.MOVE_FULL, (Vector3)node.position);
                }

                return;
            }    
        }

        _quad.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if (_quad == null) return;


    }
}
