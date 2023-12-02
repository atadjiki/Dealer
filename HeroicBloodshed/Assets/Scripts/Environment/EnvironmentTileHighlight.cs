using EPOOutline;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentTileHighlight : MonoBehaviour, IEncounterEventHandler, IEnvironmentInputHandler
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
        switch(stateID)
        {
            case EncounterState.SELECT_CURRENT_CHARACTER:
                break;
            default:
                SetColor(Color.clear);
                StopAllCoroutines();
                break;
        }

        yield return null;
    }

    public IEnumerator PerformInputUpdate(EnvironmentInputData InputData)
    {
        transform.position = InputData.TilePosition;

        if (InputData.OnValidTile)
        {
            SetColor(GetColor(InputData.RangeType));
        }
        else
        {
            SetColor(Color.clear);
        }

        yield return null;
    }

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
