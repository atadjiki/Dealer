using EPOOutline;
using Pathfinding;
using Pathfinding.RVO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[RequireComponent(typeof(BoxCollider))]

public class EnvironmentObstacle : MonoBehaviour
{
    [SerializeField] private EnvironmentObstacleType ObstacleType;

    //[SerializeField] private Shader FillShader;

    public EnvironmentObstacleType GetObstacleType() { return ObstacleType; }

    private BoxCollider _collider;

    //private Outlinable _outliner;

    //private void Awake()
    //{
    //    _collider = GetComponent<BoxCollider>();
    //    _outliner = this.gameObject.AddComponent<Outlinable>();

    //    _outliner.RenderStyle = RenderStyle.FrontBack;

    //    _outliner.BackParameters.Color = Color.clear;
    //    //_outliner.BackParameters.FillPass.Shader = FillShader;
    //    //_outliner.BackParameters.FillPass.SetColor("_PublicColor", fillColor);

    //    _outliner.FrontParameters.Color = Color.grey;
    //    //_outliner.FrontParameters.FillPass.Shader = FillShader;
    //    //_outliner.FrontParameters.FillPass.SetColor("_PublicColor", fillColor);

    //    foreach (MeshRenderer renderer in this.gameObject.GetComponentsInChildren<MeshRenderer>())
    //    {
    //        OutlineTarget target = new OutlineTarget(renderer);
    //        _outliner.TryAddTarget(target);
    //    }
    //}

}
