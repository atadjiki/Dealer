using EPOOutline;
using Pathfinding.RVO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(BoxCollider))]

public class EnvironmentObstacle : MonoBehaviour
{
    [SerializeField] private EnvironmentObstacleType ObstacleType;

    [SerializeField] private Shader BackShader;

    public EnvironmentObstacleType GetObstacleType() { return ObstacleType; }

    private BoxCollider _collider;

    private Outlinable _outliner;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();

        _outliner = this.gameObject.AddComponent<Outlinable>();

        _outliner.OutlineLayer = 0;

        _outliner.RenderStyle = RenderStyle.FrontBack;

        _outliner.FrontParameters.Color = Color.white;

        Color backColor = Color.grey;

        _outliner.BackParameters.Color = backColor;
        if (BackShader != null)
        {
            _outliner.BackParameters.FillPass.Shader = BackShader;
            backColor.a = 0.1f;
        }

        _outliner.BackParameters.FillPass.SetColor("_PublicColor", backColor);

        foreach (MeshRenderer renderer in this.gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            OutlineTarget target = new OutlineTarget(renderer);
            _outliner.TryAddTarget(target);
        }
    }

}
