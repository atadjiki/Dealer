using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentTileHighlight : MonoBehaviour, IEncounterEventHandler
{
    [SerializeField] private GameObject HighlightDecal;

    private EnvironmentTileActiveState _activeState = EnvironmentTileActiveState.Inactive;

    public IEnumerator Coroutine_EncounterStateUpdate(Constants.EncounterState stateID, EncounterModel model)
    {
        switch (stateID)
        {
            case EncounterState.CHOOSE_ACTION:
                if (!model.IsCurrentTeamCPU())
                {
                    SetActiveState(EnvironmentTileActiveState.Active);
                }
                break;
            default:
                SetActiveState(EnvironmentTileActiveState.Inactive);
                break;
        }

        yield return null;
    }

    private void Update()
    {
        if (_activeState == EnvironmentTileActiveState.Active)
        {
            CheckMouseHighlight();
        }
    }

    private void CheckMouseHighlight()
    {
        if (EnvironmentUtil.CheckIsMouseBlocked()) { return; }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("EnvironmentGround")))
        {
            Vector3 nodePosition;
            if (EnvironmentManager.Instance.GetClosestNodeToPosition(hit.point, out nodePosition))
            {
                HighlightDecal.transform.position = nodePosition + new Vector3(0,0.5f,0);
            }
        }
    }

    private void SetActiveState(EnvironmentTileActiveState state)
    {
        _activeState = state;

        HighlightDecal.SetActive(_activeState == EnvironmentTileActiveState.Active);
    }
}
