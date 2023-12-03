using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Constants;

public class EnvironmentTile : MonoBehaviour
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

        switch(_state)
        {
            case EnvironmentTileState.Full:
                Mesh.SetActive(true);
                _outline.OutlineParameters.Color = GetColor(MovementRangeType.Full);
                _outline.OutlineLayer = 2;
                break;
            case EnvironmentTileState.Half:
                Mesh.SetActive(true);
                _outline.OutlineParameters.Color = GetColor(MovementRangeType.Half);
                _outline.OutlineLayer = 1;
                break;
            case EnvironmentTileState.None:
                Mesh.SetActive(false);
                break;
        }
    }
}
