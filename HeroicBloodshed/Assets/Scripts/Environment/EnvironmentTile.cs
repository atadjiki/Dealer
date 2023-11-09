using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Constants;

[RequireComponent(typeof(BoxCollider))]
public class EnvironmentTile : MonoBehaviour
{
    public delegate void TileSelectedDelegate(EnvironmentTile environmentTile);
    public TileSelectedDelegate OnTileSelected;

    [SerializeField] private List<EnvironmentTile> _neighbors;

    [SerializeField] private EnvironmentObstacle _obstacle;

    [SerializeField] private EnvironmentSpawnPoint _spawnPoint;

    private Vector2 _coordinates;
    private BoxCollider _collider;
    private MeshRenderer _renderer;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _renderer = GetComponentInChildren<MeshRenderer>();

        ToggleVisibility(false);
    }

    private void Update()
    {
        CheckMouseHighlight();
        CheckMouseClick();
    }

    public List<EnvironmentTile> GetNeighbors()
    {
        return _neighbors;
    }

    public bool HasNeighbors()
    {
        if(_neighbors != null)
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
        //need to also check for other characters on these tiles

        if(ContainsObstacle())
        {
            return false;
        }

        return true;
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

    private void CheckMouseHighlight()
    {
        if (Camera.main == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("EnvironmentTile")))
        {
            EnvironmentTile tile = hit.collider.GetComponent<EnvironmentTile>();

            if (tile != null && tile == this)
            {
                ToggleVisibility(true);
                return;
            }
        }

        ToggleVisibility(false);

    }

    private void CheckMouseClick()
    {
        if (Camera.main == null) return;
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("EnvironmentTile")))
        {
            EnvironmentTile tile = hit.collider.GetComponent<EnvironmentTile>();

            if (tile != null && tile == this)
            {
                Select();
            }
        }
    }

    public void Setup(int Row, int Column)
    {
        _coordinates = new Vector2(Row, Column);
        SetColor(Color.clear);
    }

    public void MarkAsCoverTile(EnvironmentObstacleType obstacleType)
    {
        if(_obstacle != null) { return; }
        switch(obstacleType)
        {
            case EnvironmentObstacleType.FullCover:
                SetColor(Color.cyan);
                break;
            case EnvironmentObstacleType.HalfCover:
                SetColor(Color.yellow);
                break;
            case EnvironmentObstacleType.NoCover:
                break;
        }
    }

    public void PerformCoverCheck()
    {
        if(_obstacle != null)
        {
            foreach(EnvironmentTile neighbor in _neighbors)
            {
                float distance = Vector3.Distance(this.transform.position, neighbor.transform.position);

                if (Mathf.Approximately(distance, 1))
                {
                    neighbor.MarkAsCoverTile(_obstacle.GetObstacleType());
                }
            }
        }
    }

    public void PerformScan()
    {
        GatherNeighbors();
        GatherObstacles();
        GatherSpawnMarkers();

        if (_spawnPoint != null)
        {
            SetColor(Color.green);
        }
        else if (_obstacle != null)
        {
            SetColor(Color.red);
        }
        else
        {
            SetColor(Color.blue);
        }
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

    public Vector2 GetCoordinates()
    {
        return _coordinates;
    }

    public void SetColor(Color color)
    {
        if(_renderer != null)
        {
            _renderer.material.color = color;
        }
    }

    public void ToggleVisibility(bool flag)
    {
        _renderer.gameObject.SetActive(flag);
    }

    public void Select()
    {
        if (OnTileSelected != null)
        {
            OnTileSelected.Invoke(this);
        }
    }
}
