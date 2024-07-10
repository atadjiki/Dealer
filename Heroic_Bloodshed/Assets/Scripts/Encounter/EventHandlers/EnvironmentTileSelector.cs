using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentTileSelector : EncounterEventHandler
{
    [SerializeField] private GameObject Quad;

    protected override void OnStart()
    {
        base.OnStart();

        Quad.SetActive(false);
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
        if (Quad == null) return;

        TileNode node;
        if(EnvironmentUtil.GetNodeBeneathMouse(out node))
        {
            if(node.layer == EnvironmentLayer.CHARACTER || node.layer == EnvironmentLayer.GROUND)
            {
                Quad.transform.position = node.GetGridPosition();
                Quad.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    EncounterStateMachine.OnAbilityChosen.Invoke(AbilityID.MOVE_FULL, node.GetGridPosition());
                }

                return;
            }    
        }

        Quad.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if (Quad == null) return;


    }
}
