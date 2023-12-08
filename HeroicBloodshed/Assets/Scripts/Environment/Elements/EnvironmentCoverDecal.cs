using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentCoverDecal : MonoBehaviour
{
    [SerializeField] private GameObject Quad_Full;
    [SerializeField] private GameObject Quad_Half;

    private EnvironmentObstacleType _obstacleType;

    public void Setup(EnvironmentObstacleType obstacleType)
    {
        _obstacleType = obstacleType;

        ToggleVisibility(false);

        UpdateColor(Color.white);
    }

    private void UpdateColor(Color color)
    {
        if (_obstacleType == EnvironmentObstacleType.HalfCover)
        {
            SetChildMaterialColors(Quad_Half, color);
        }
        else if (_obstacleType == EnvironmentObstacleType.FullCover)
        {
            SetChildMaterialColors(Quad_Full, color);
        }
    }

    public void ToggleVisibility(bool flag)
    {
        if (_obstacleType == EnvironmentObstacleType.HalfCover)
        {
            Quad_Full.SetActive(false);
            Quad_Half.SetActive(flag);
        }
        else if (_obstacleType == EnvironmentObstacleType.FullCover)
        {
            Quad_Full.SetActive(flag);
            Quad_Half.SetActive(false);
        }
        else
        {
            Quad_Full.SetActive(false);
            Quad_Half.SetActive(false);
        }
    }

    private void SetChildMaterialColors(GameObject parentObject, Color color)
    {
        foreach (MeshRenderer renderer in parentObject.GetComponentsInChildren<MeshRenderer>())
        {
            if (renderer.material != null)
            {
                renderer.material.color = color;
            }
        }
    }

    public List<Transform> GetChildTransforms()
    {
        List<Transform> quadPositions = new List<Transform>();

        Transform parentTransform;

        if(_obstacleType == EnvironmentObstacleType.FullCover)
        {
            parentTransform = Quad_Full.transform;
        }
        else
        {
            parentTransform = Quad_Half.transform;
        }

        for (int i = 0; i < parentTransform.childCount; i++)
        {
            quadPositions.Add(parentTransform.GetChild(i));
        }

        return quadPositions;
    }
}
