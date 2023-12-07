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

    private List<EnvironmentCoverDecal> _coverDecals;

    private List<Vector3> _neighbors;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();

        _coverDecals = new List<EnvironmentCoverDecal>();
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

    public void AddCoverDecal(Vector3 position)
    {
        StartCoroutine(Coroutine_CreateCoverDecal(position));
    }

    public void UpdateMovementRangeType(MovementRangeType movementRangeType)
    {
        foreach (EnvironmentCoverDecal decal in _coverDecals)
        {
            decal.UpdateMovementRangeType(movementRangeType);
        }
    }

    public void ToggleDecal(bool flag)
    {
        foreach(EnvironmentCoverDecal decal in _coverDecals)
        {
            decal.ToggleVisibility(flag);
        }
    }

    private IEnumerator Coroutine_CreateCoverDecal(Vector3 position)
    {
        ResourceRequest resourceRequest = GetEnvironmentVFX(PrefabID.EnvironmentCoverDecal);

        yield return new WaitUntil(() => resourceRequest.isDone);

        GameObject prefabObject = Instantiate((GameObject)resourceRequest.asset, this.transform);

        prefabObject.transform.position = position;

        yield return new WaitUntil(() => prefabObject.GetComponent<EnvironmentCoverDecal>() != null);

        EnvironmentCoverDecal coverDecal = prefabObject.GetComponent<EnvironmentCoverDecal>();

        coverDecal.Setup(ObstacleType);

        _coverDecals.Add(coverDecal);

        PerformCoverDecalOverlapCheck();

        yield return null;
    }

    private void PerformCoverDecalOverlapCheck()
    {
        if(_coverDecals.Count > 1)
        {
            Debug.Log("Decals on obstacle: " + this.gameObject.name + " " + _coverDecals.Count);

            foreach (EnvironmentCoverDecal coverDecal in _coverDecals)
            {
                foreach(Transform childTransform in coverDecal.GetChildTransforms())
                {
                    if(_collider.bounds.Contains(childTransform.position))
                    {
                        Debug.Log("overlap found");
                        childTransform.gameObject.SetActive(false);
                    }
                    else if(!EnvironmentManager.Instance.IsPositionFree(childTransform.position))
                    {
                        Debug.Log("tile is not walkable");
                        childTransform.gameObject.SetActive(false);
                    }
                }
            }
        }

    }

    public void SetNeighbors(List<Vector3> nodeList)
    {
        _neighbors = nodeList;
    }

    public List<Vector3> GetNeighbors()
    {
        return _neighbors;
    }
}
