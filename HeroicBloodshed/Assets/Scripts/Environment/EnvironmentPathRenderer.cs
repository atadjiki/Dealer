using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentPathRenderer : MonoBehaviour, IEncounterEventHandler, IEnvironmentInputHandler
{
    private LineRenderer _pathRenderer;

    private void Awake()
    {
        _pathRenderer = GetComponent<LineRenderer>();   
    }

    public IEnumerator Coroutine_EncounterStateUpdate(Constants.EncounterState stateID, EncounterModel model)
    {
        switch (stateID)
        {
            case EncounterState.CHOOSE_ACTION:
                break;
            default:
                Clear();
                break;
        }

        yield return null;
    }

    public IEnumerator PerformInputUpdate(EnvironmentInputData InputData)
    {
        if (InputData.OnValidTile)
        {
            if (InputData.RangeType != MovementRangeType.None)
            {
                Clear();

                int length = InputData.VectorPath.Count;

                _pathRenderer.positionCount = length;

                _pathRenderer.SetPositions(InputData.VectorPath.ToArray());

                Color pathColor = GetColor(InputData.RangeType);
                pathColor.a = 0.25f;

                _pathRenderer.material.color = pathColor;

                _pathRenderer.forceRenderingOff = false;
            }
        }
        else
        {
            Clear();
        }

        yield return null;
    }

    private void Clear()
    {
        _pathRenderer.positionCount = 0;
        _pathRenderer.SetPositions(new Vector3[] { });
        _pathRenderer.forceRenderingOff = true;
    }
}
