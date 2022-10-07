using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ObjectMaterialSetter : MaterialSetter
{
    protected MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public virtual void ApplyCharacterInfo(CharacterInfo characterInfo)
    {
    }
}
