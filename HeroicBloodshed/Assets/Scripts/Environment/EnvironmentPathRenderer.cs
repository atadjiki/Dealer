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
            case EncounterState.SELECT_CURRENT_CHARACTER:
                break;
            default:
                Clear();
                StopAllCoroutines();
                break;
        }

        yield return null;
    }

    public IEnumerator PerformInputUpdate(EnvironmentInputData InputData)
    {
        if (InputData.OnValidTile)
        {
            Clear();

            if (InputData.RangeType != MovementRangeType.None)
            {
                int length = InputData.VectorPath.Count;

                _pathRenderer.positionCount = length;

                Vector3[] positions = InputData.VectorPath.ToArray();

                for(int i = 0; i < positions.Length; i++)
                {
                    positions[i].y = 0.5f;
                }

                _pathRenderer.SetPositions(positions);

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
