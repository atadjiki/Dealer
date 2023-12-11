using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Constants;

public class EnvironmentRadiusTile : MonoBehaviour
{
    [Header("Meshes")]
    [SerializeField] private GameObject Mesh;

    private Outlinable _outline;

    private EnvironmentTileState _state;

    private void Awake()
    {
        _outline = Mesh.GetComponent<Outlinable>();
    }

    public void SetState(EnvironmentTileState previewState)
    {
        _state = previewState;

        Color color;

        switch (_state)
        {
            case EnvironmentTileState.Full:
                Mesh.SetActive(true);
                color = GetColor(MovementRangeType.Full);
                _outline.OutlineParameters.Color = color;
                color.a = 0.25f;
                _outline.OutlineParameters.FillPass.SetColor("_PublicColor", color);
                break;
            case EnvironmentTileState.Half:
                Mesh.SetActive(true);
                color = GetColor(MovementRangeType.Half);
                _outline.OutlineParameters.Color = color;
                color.a = 0.25f;
                _outline.OutlineParameters.FillPass.SetColor("_PublicColor", color);
                break;
            case EnvironmentTileState.None:
                Mesh.SetActive(false);
                color = GetColor(MovementRangeType.None);
                _outline.OutlineParameters.Color = color;
                color.a = 0.25f;
                _outline.OutlineParameters.FillPass.SetColor("_PublicColor", color);
                break;
        }
    }
}
