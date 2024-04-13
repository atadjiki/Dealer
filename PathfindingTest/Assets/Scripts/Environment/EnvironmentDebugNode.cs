using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public struct EnvironmentDebugNodeSettings
{
    public Color Color_Walkable;
    public Color Color_Unwalkable;
    public Color Color_Obstacle_Half;
    public Color Color_Obstacle_Full;
}

public class EnvironmentDebugNode : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private EnvironmentDebugNodeSettings Settings;

    [Header("Mesh")]
    [SerializeField] private GameObject Mesh;

    private MeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
    }

    public void SetState(EnvironmentNodeState state)
    {
        Color color = GetColorByState(state);

        SetColor(color);

        Vector3 scale = GetScaleByState(state);

        Mesh.transform.localScale = scale;
        
    }

    private void SetColor(Color color)
    {
        if (_renderer != null)
        {
            _renderer.material.color = color;
            return;
        }

        Debug.Log("Mesh renderer was null!");
    }

    private void SetVisibility(bool flag)
    {
        Mesh.SetActive(flag);
    }

    private Vector3 GetScaleByState(EnvironmentNodeState state)
    {
        switch(state)
        {
            case EnvironmentNodeState.Walkable:
                return new Vector3(0.25f, 0.1f, 0.25f);
            default:
                return new Vector3(1.5f, 0.1f, 1.5f);
        }
    }

    private Color GetColorByState(EnvironmentNodeState state)
    {
        switch(state)
        {
            case EnvironmentNodeState.Walkable:
                return Settings.Color_Walkable;
            case EnvironmentNodeState.Unwalkable:
                return Settings.Color_Unwalkable;
            case EnvironmentNodeState.Obstacle_Half:
                return Settings.Color_Obstacle_Half;
            case EnvironmentNodeState.Obstacle_Full:
                return Settings.Color_Obstacle_Full;
            default:
                return Color.clear;
        }
    }
}
