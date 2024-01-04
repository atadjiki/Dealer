using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentTileHighlight : EnvironmentInputHandler
{
    [Header("Colors")]
    [SerializeField] private Color Color_HalfRange;
    [SerializeField] private Color Color_FullRange;

    [SerializeField] private GameObject HighlightDecal;

    private MeshRenderer _renderer;
    private Outlinable _outliner;

    private void Awake()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();

        _outliner = GetComponentInChildren<Outlinable>();
    }

    public override void Deactivate()
    {
        base.Deactivate();

        SetColor(Color.clear);
    }

    public override IEnumerator PerformInputUpdate(EnvironmentInputData InputData)
    {
        transform.position = InputData.NodePosition;

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
