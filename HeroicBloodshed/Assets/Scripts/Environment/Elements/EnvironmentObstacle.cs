using EPOOutline;
using Pathfinding;
using Pathfinding.RVO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Constants;

[RequireComponent(typeof(BoxCollider))]

public class EnvironmentObstacle : MonoBehaviour
{
    [SerializeField] private EnvironmentObstacleType ObstacleType;

    public EnvironmentObstacleType GetObstacleType() { return ObstacleType; }

    private BoxCollider _collider;

    private Dictionary<Vector3, EnvironmentCoverDecal> _coverDecals;

    private List<Vector3> _neighbors;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();

        _coverDecals = new Dictionary<Vector3, EnvironmentCoverDecal>();
    }

    public void Setup()
    {
        _neighbors = EnvironmentUtil.GetNeighboringNodes(_collider.bounds);
    }

    public Bounds GetBounds()
    {
        return _collider.bounds;
    }

    public bool ContainsPoint(Vector3 point)
    {
        return _collider.bounds.Contains(point);
    }

    //public void AddCoverDecal(Vector3 position)
    //{
    //    StartCoroutine(Coroutine_CreateCoverDecal(position));
    //}

    //public void ToggleDecal(bool flag)
    //{
    //    foreach(EnvironmentCoverDecal decal in _coverDecals.Values)
    //    {
    //        decal.ToggleVisibility(flag);
    //    }
    //}

    //private IEnumerator Coroutine_CreateCoverDecal(Vector3 position)
    //{
    //    ResourceRequest resourceRequest = GetEnvironmentVFX(PrefabID.EnvironmentCoverDecal);

    //    yield return new WaitUntil(() => resourceRequest.isDone);

    //    GameObject prefabObject = Instantiate((GameObject)resourceRequest.asset, this.transform);

    //    prefabObject.transform.position = position;

    //    yield return new WaitUntil(() => prefabObject.GetComponent<EnvironmentCoverDecal>() != null);

    //    EnvironmentCoverDecal coverDecal = prefabObject.GetComponent<EnvironmentCoverDecal>();

    //    coverDecal.Setup(ObstacleType);

    //    _coverDecals.Add(position, coverDecal);

    //    PerformCoverDecalOverlapCheck();

    //    yield return null;
    //}

    public void SetNeighbors(List<Vector3> nodeList)
    {
        _neighbors = nodeList;
    }

    public List<Vector3> GetNeighbors()
    {
        return _neighbors;
    }

    public bool IsNeighborOf(Vector3 position)
    {
        return _neighbors.Contains(position);
    }
}
