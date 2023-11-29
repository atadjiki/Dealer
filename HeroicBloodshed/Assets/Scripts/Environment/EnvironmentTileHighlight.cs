using EPOOutline;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentTileHighlight : MonoBehaviour, IEncounterEventHandler
{
    [SerializeField] private GameObject HighlightDecal;

    private MeshRenderer _renderer;
    private Outlinable _outliner;

    private void Awake()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();

        _outliner = GetComponentInChildren<Outlinable>();
    }

    public IEnumerator Coroutine_EncounterStateUpdate(Constants.EncounterState stateID, EncounterModel model)
    {
        yield return null;
    }

    private void CheckMouseHighlight()
    {
        if (EnvironmentUtil.CheckIsMouseBlocked()) { return; }

        CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("EnvironmentGround")))
        {
            Vector3 nodePosition;

            if (EnvironmentManager.Instance.GetClosestNodeToPosition(hit.point, out nodePosition))
            {
                if(!EnvironmentManager.Instance.IsPositionOccupied(nodePosition))
                {
                    int distance = (int)Vector3.Distance(nodePosition, currentCharacter.GetWorldLocation());

                    MovementRangeType rangeType;

                    if (EnvironmentUtil.IsWithinCharacterRange(distance, currentCharacter, out rangeType))
                    {
                        SetColor(GetColor(rangeType));
                        HighlightDecal.transform.position = nodePosition + new Vector3(0, 0.5f, 0);
                    }
                    else
                    {
                        SetColor(Color.clear);
                    }
                }
                else
                {
                    SetColor(Color.clear);
                }
            }
            else
            {
                SetColor(Color.clear);
            }
        }
    }

    private void CheckMouseClick()
    {
        if (EnvironmentUtil.CheckIsMouseBlocked()) { return; }

        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask(LAYER_ENV_GROUND)))
        {
            if(!EnvironmentManager.Instance.IsPositionOccupied(hit.point))
            {
                StartCoroutine(Coroutine_OnDestinationSelected(hit.point));
            }
        }
    }

    private IEnumerator Coroutine_OnDestinationSelected(Vector3 destination)
    {
        CharacterComponent currentCharacter = EncounterManager.Instance.GetCurrentCharacter();

        int movementRange = currentCharacter.GetMovementRange();

        Vector3 origin = currentCharacter.GetWorldLocation();

        ABPath path = ABPath.Construct(origin, destination);

        AstarPath.StartPath(path, true);

        yield return new WaitUntil(() => path.CompleteState == PathCompleteState.Complete);

        int cost = 0;

        foreach (GraphNode pathNode in path.path)
        {
            cost += (int)path.GetTraversalCost(pathNode);
        }

        Debug.Log("Path cost is " + cost);

        MovementRangeType rangeType;
        if (EnvironmentUtil.IsWithinCharacterRange(cost, currentCharacter, out rangeType))
        {
            EncounterManager.Instance.OnEnvironmentDestinationSelected(destination, rangeType);
        }
        else
        {
            Debug.Log(destination.ToString() + " is out of range for " + currentCharacter.GetID());
        }
    }

    //private void SetActiveState(EnvironmentTileActiveState state)
    //{


    //    HighlightDecal.SetActive(_activeState == EnvironmentTileActiveState.Active);
    //}

    private void SetColor(Color color)
    {
        if (_renderer.material != null)
        {
            _renderer.material.color = color;
        }

        if(_outliner != null)
        {
            _outliner.OutlineParameters.Color = color;
        }
    }
}
