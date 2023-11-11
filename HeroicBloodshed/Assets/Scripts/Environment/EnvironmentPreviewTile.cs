using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentPreviewTile : MonoBehaviour
{
    [SerializeField] private Color Color_Half;
    [SerializeField] private Color Color_Full;

    private Outlinable _outliner;

    private void Awake()
    {
        _outliner = GetComponent<Outlinable>();
    }

    public void Setup(MovementRangeType rangeType)
    {
        switch(rangeType)
        {
            case MovementRangeType.Full:
                SetColor(Color_Full);
                break;
            case MovementRangeType.Half:
                SetColor(Color_Half);
                break;
        }
    }

    private void SetColor(Color color)
    {
        if(_outliner != null)
        {
            _outliner.OutlineParameters.Color = color;
        }
    }
}
