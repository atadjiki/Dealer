using EPOOutline;
using Pathfinding;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(GraphUpdateScene))]
public class EnvironmentObstacle : MonoBehaviour
{
    [SerializeField] private EnvironmentObstacleType ObstacleType;

    public EnvironmentObstacleType GetObstacleType() { return ObstacleType; }

    private BoxCollider _collider;

    private void Awake()
    {
        EnvironmentUtil.AddOutline(this.gameObject, Color.grey, 0.5f);
    }
    
    public Bounds GetBounds()
    {
        if(_collider == null)
        {
            _collider = GetComponent<BoxCollider>();
        }

        return _collider.bounds;
    }
}
