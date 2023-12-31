using EPOOutline;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(BoxCollider))]

public class EnvironmentObstacle : MonoBehaviour
{
    [SerializeField] private EnvironmentObstacleType ObstacleType;

    public EnvironmentObstacleType GetObstacleType() { return ObstacleType; }

    private BoxCollider _collider;

    private void Awake()
    {
        Outlinable outline = this.gameObject.AddComponent<Outlinable>();

        foreach(Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            OutlineTarget target = new OutlineTarget(renderer);
            outline.TryAddTarget(target);
        }

        Color outlineColor = Color.black;
        outlineColor.a = 0.25f;
        outline.OutlineParameters.Color = outlineColor;
        outline.OutlineParameters.DilateShift = 0.25f;
        outline.OutlineParameters.BlurShift = 0.25f;
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
