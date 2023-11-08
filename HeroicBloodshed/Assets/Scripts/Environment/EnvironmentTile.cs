using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Constants;

[RequireComponent(typeof(BoxCollider))]
public class EnvironmentTile : MonoBehaviour
{
    [SerializeField] private List<EnvironmentTile> Neighbors;

    [SerializeField] private EnvironmentObstacle Obstacle;

    [SerializeField] private EnvironmentSpawnPoint SpawnPoint;

    private Vector2 _coordinates;
    private BoxCollider _collider;
    private MeshRenderer _renderer;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _renderer = GetComponentInChildren<MeshRenderer>();

        ToggleVisibility(false);
    }

    public void Setup(int Row, int Column)
    {
        _coordinates = new Vector2(Row, Column);
        SetColor(Color.clear);
    }

    public void MarkAsCoverTile(EnvironmentObstacleType obstacleType)
    {
        if(Obstacle != null) { return; }
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
        if(Obstacle != null)
        {
            foreach(EnvironmentTile neighbor in Neighbors)
            {
                float distance = Vector3.Distance(this.transform.position, neighbor.transform.position);

                if (Mathf.Approximately(distance, 1))
                {
                    neighbor.MarkAsCoverTile(Obstacle.GetObstacleType());
                }
            }
        }
    }

    public void PerformScan()
    {
        GatherNeighbors();
        GatherObstacles();
        GatherSpawnMarkers();

        if(Obstacle != null)
        {
            SetColor(Color.red);
        }
        else
        {
            SetColor(Color.blue);
        }

        if(SpawnPoint != null)
        {
            SetColor(Color.green);
        }
    }

    private void GatherNeighbors()
    {
        Neighbors = new List<EnvironmentTile>();

        foreach (Collider collider in Physics.OverlapSphere(this.transform.position, 1f, LayerMask.GetMask("EnvironmentTile")))
        {
            EnvironmentTile neighbor = collider.gameObject.GetComponent<EnvironmentTile>();

            if (neighbor != this)
            {
                Neighbors.Add(neighbor);
            }
        }
    }

    private void GatherObstacles()
    {
        Obstacle = null;

        foreach (Collider collider in Physics.OverlapSphere(this.transform.position, 0.1f, LayerMask.GetMask("EnvironmentObstacle")))
        {
            Obstacle = collider.gameObject.GetComponent<EnvironmentObstacle>();
            break;
        }
    }

    private void GatherSpawnMarkers()
    {
        SpawnPoint = null;

        foreach (Collider collider in Physics.OverlapSphere(this.transform.position, 0.1f, LayerMask.GetMask("EnvironmentSpawnMarker")))
        {
            SpawnPoint = collider.gameObject.GetComponent<EnvironmentSpawnPoint>();
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
            color.a = 0.1f;

            _renderer.material.color = color;
        }
    }

    public void ToggleVisibility(bool flag)
    {
        //_renderer.gameObject.SetActive(flag);
    }

    private void OnMouseOver()
    {
        ToggleVisibility(true);
    }

    private void OnMouseExit()
    {
        ToggleVisibility(false);
    }
}
