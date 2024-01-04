using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentPathRenderer : EnvironmentInputHandler
{
    [Header("Colors")]
    [SerializeField] private Color Color_HalfRange;
    [SerializeField] private Color Color_FullRange;

    private LineRenderer _pathRenderer;

    private void Awake()
    {
        _pathRenderer = GetComponent<LineRenderer>();   
    }

    public override void Deactivate()
    {
        base.Deactivate();

        Clear();
    }

    public override IEnumerator PerformInputUpdate(EnvironmentInputData InputData)
    {
        base.PerformInputUpdate(InputData);

        if (InputData.OnValidTile)
        {
            Clear();

            if (InputData.RangeType != MovementRangeType.None)
            {
                List<Vector3> positions = InputData.PathToHighlightedNode;

                int length = positions.Count;

                _pathRenderer.positionCount = length;

                _pathRenderer.SetPositions(positions.ToArray());

                Color pathColor = GetColor(InputData.RangeType);
                
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

    private Color GetColor(MovementRangeType rangeType)
    {
        if (rangeType == MovementRangeType.Half)
        {
            return Color_HalfRange;
        }
        else if (rangeType == MovementRangeType.Full)
        {
            return Color_FullRange;
        }

        return Color.clear;
    }
}
