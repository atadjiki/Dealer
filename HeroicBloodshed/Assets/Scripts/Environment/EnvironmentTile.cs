using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Constants;

[RequireComponent(typeof(BoxCollider))]
public class EnvironmentTile : MonoBehaviour
{
    [Header("Meshes")]
    [SerializeField] private GameObject Mesh_Highlight;
    [SerializeField] private GameObject Mesh_Preview;

    private List<EnvironmentTile> _neighbors;
    private EnvironmentObstacle _obstacle;
    private EnvironmentSpawnPoint _spawnPoint;

    private Outlinable _highlightOutline;
    private Outlinable _previewOutline;

    private EnvironmentTileHighlightState _highlightState;
    private EnvironmentTilePreviewState _previewState;

    private void Awake()
    {
        _highlightOutline = Mesh_Highlight.GetComponent<Outlinable>();
        _previewOutline = Mesh_Preview.GetComponent<Outlinable>();
    }

    public void PerformScan()
    {
        GatherNeighbors();
        GatherObstacles();
        GatherSpawnMarkers();
    }

    private void GatherNeighbors()
    {
        _neighbors = new List<EnvironmentTile>();

        foreach (Collider collider in Physics.OverlapSphere(this.transform.position, 1f, LayerMask.GetMask("EnvironmentTile")))
        {
            EnvironmentTile neighbor = collider.gameObject.GetComponent<EnvironmentTile>();

            if (neighbor != this)
            {
                _neighbors.Add(neighbor);
            }
        }
    }

    private void GatherObstacles()
    {
        _obstacle = null;

        foreach (Collider collider in Physics.OverlapSphere(this.transform.position, 0.1f, LayerMask.GetMask("EnvironmentObstacle")))
        {
            _obstacle = collider.gameObject.GetComponent<EnvironmentObstacle>();
            break;
        }
    }

    private void GatherSpawnMarkers()
    {
        _spawnPoint = null;

        foreach (Collider collider in Physics.OverlapSphere(this.transform.position, 0.1f, LayerMask.GetMask("EnvironmentSpawnMarker")))
        {
            _spawnPoint = collider.gameObject.GetComponent<EnvironmentSpawnPoint>();
            break;
        }
    }

    public bool HasNeighbors()
    {
        if (_neighbors != null)
        {
            return _neighbors.Count > 0;
        }
        else
        {
            return false;
        }
    }

    public bool IsFree()
    {
        if (ContainsObstacle())
        {
            return false;
        }

        return true;
    }

    public EnvironmentObstacleType GetCoverType()
    {
        EnvironmentObstacleType obstacleType = EnvironmentObstacleType.NoCover;

        if (_neighbors != null)
        {
            foreach (EnvironmentTile neighbor in _neighbors)
            {
                float distance = Vector3.Distance(this.transform.position, neighbor.transform.position);
                if (Mathf.Approximately(distance, 1))
                {
                    if (neighbor.ContainsObstacle())
                    {
                        EnvironmentObstacleType neighborType = neighbor.GetObstacle().GetObstacleType();

                        if (neighborType > obstacleType)
                        {
                            obstacleType = neighborType;
                        }
                    }
                }
            }
        }

        return obstacleType;
    }

    public bool IsCoverTile()
    {
        if(_neighbors != null)
        {
            foreach (EnvironmentTile neighbor in _neighbors)
            {
                float distance = Vector3.Distance(this.transform.position, neighbor.transform.position);
                if (Mathf.Approximately(distance, 1))
                {
                    if (neighbor.ContainsObstacle())
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void SetHighlightState(EnvironmentTileHighlightState highlightState)
    {
        _highlightState = highlightState;

        Color outlineColor = Color.white;
        Color fillColor = Color.clear;

        if (_highlightState == EnvironmentTileHighlightState.On)
        {
            if (ContainsObstacle())
            {
                fillColor = Color.red;
            }
            else if(IsCoverTile())
            {
                EnvironmentObstacleType obstacleType = GetCoverType();

                switch(obstacleType)
                {
                    case EnvironmentObstacleType.FullCover:
                        fillColor = Color.green;
                        break;
                    case EnvironmentObstacleType.HalfCover:
                        fillColor = Color.yellow;
                        break;
                }
            }

            outlineColor.a = 0.5f;
            fillColor.a = 0.2f;

            _highlightOutline.OutlineParameters.Color = outlineColor;
            _highlightOutline.OutlineParameters.FillPass.SetColor("_PublicColor", fillColor);

        }
        else
        {
            outlineColor = Color.grey;
            outlineColor.a = 0.5f;

            _highlightOutline.OutlineParameters.Color = outlineColor;
            _highlightOutline.OutlineParameters.FillPass.SetColor("_PublicColor", fillColor);
        }
    }

    public void SetPreviewState(EnvironmentTilePreviewState previewState)
    {
        _previewState = previewState;

        switch(_previewState)
        {
            case EnvironmentTilePreviewState.Full:
                Mesh_Preview.SetActive(true);
                _previewOutline.OutlineParameters.Color = GetColor(MovementRangeType.Full);
                _previewOutline.OutlineLayer = 2;
                break;
            case EnvironmentTilePreviewState.Half:
                Mesh_Preview.SetActive(true);
                _previewOutline.OutlineParameters.Color = GetColor(MovementRangeType.Half);
                _previewOutline.OutlineLayer = 1;
                break;
            case EnvironmentTilePreviewState.None:
                Mesh_Preview.SetActive(false);
                break;
        }
    }

    public List<EnvironmentTile> GetNeighbors()
    {
        return _neighbors;
    }

    public EnvironmentObstacle GetObstacle()
    {
        return _obstacle;
    }

    public bool ContainsObstacle()
    {
        return _obstacle != null;
    }

    public EnvironmentSpawnPoint GetSpawnPoint()
    {
        return _spawnPoint;
    }

    public bool ContainsSpawnPoint()
    {
        return _spawnPoint != null;
    }
}
