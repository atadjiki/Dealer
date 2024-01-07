using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EnvironmentCoverDecal : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material Mat_Half;
    [SerializeField] private Material Mat_Full;

    [SerializeField] private GameObject Quad;

    private MeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
    }

    public void Setup(EnvironmentObstacleType obstacleType)
    {
        if (obstacleType == EnvironmentObstacleType.FullCover)
        {
            _renderer.material = Mat_Full;
            Quad.transform.localPosition = new Vector3(0, 1.5f, 0.75f);
        }
        else
        {
            _renderer.material = Mat_Half;
            Quad.transform.localPosition = new Vector3(0, 0.75f, 0.75f);
        }
    }
}
