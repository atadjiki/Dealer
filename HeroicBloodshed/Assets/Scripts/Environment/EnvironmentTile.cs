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

    [SerializeField] private List<EnvironmentObstacle> Obstacles;

    private Vector2 _coordinates;
    private BoxCollider _collider;
    private MeshRenderer _renderer;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _renderer = GetComponentInChildren<MeshRenderer>();

        SetColor(Color.clear);
    }

    public void Setup(int Row, int Column)
    {
        _coordinates = new Vector2(Row, Column);
        SetColor(Color.clear);
    }

    public void MarkAsCoverTile(EnvironmentObstacleType obstacleType)
    {
        if(Obstacles.Count > 0) { return; }
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
        if(Obstacles.Count > 0)
        {
            EnvironmentObstacle obstacle = Obstacles[0];

            foreach(EnvironmentTile neighbor in Neighbors)
            {
                float distance = Vector3.Distance(this.transform.position, neighbor.transform.position);

                if (Mathf.Approximately(distance, 1))
                {
                    neighbor.MarkAsCoverTile(obstacle.GetObstacleType());
                }
            }
        }
    }

    public void PerformScan()
    {
        GatherNeighbors();
        GatherObstacles();

        if(Obstacles.Count > 0)
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
        Obstacles = new List<EnvironmentObstacle>();

        foreach (Collider collider in Physics.OverlapSphere(this.transform.position, 0.1f, LayerMask.GetMask("EnvironmentObstacle")))
        {
            EnvironmentObstacle obstacle = collider.gameObject.GetComponent<EnvironmentObstacle>();

            if (obstacle != this)
            {
                Obstacles.Add(obstacle);
            }
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
            color.a = 0.25f;

            _renderer.material.color = color;
        }
    }

    private void OnMouseOver()
    {
        //  Debug.Log("Pointer over tile " + _coordinates.ToString());
      // SetColor(Color.green);
    }

    private void OnMouseExit()
    {
     //   SetColor(Color.clear);
    }
}
