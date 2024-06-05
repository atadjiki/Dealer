using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentTileSelector : EncounterEventHandler
{
    [SerializeField] private GameObject Quad;

    protected override void OnAwake()
    {
        base.OnAwake();

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
            if(node.Layer == EnvironmentLayer.CHARACTER || node.Layer == EnvironmentLayer.GROUND)
            {
                Quad.transform.position = (Vector3) node.position;
                Quad.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    EncounterStateMachine.OnAbilityChosen.Invoke(AbilityID.MOVE_FULL, (Vector3)node.position);
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
