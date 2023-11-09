using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnvironmentWall : MonoBehaviour
{
    private List<MeshRenderer> _renderers;

    private void Awake()
    {
        _renderers = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
    }
}
