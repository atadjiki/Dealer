using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MaterialColorPicker : MonoBehaviour
{
    public Color _color = Color.magenta;

    public void UpdateColor()
    {
        MeshRenderer _meshRenderer = GetComponent<MeshRenderer>();
        Material _material = _meshRenderer.sharedMaterial;

        _material.color = _color;
    }
}
