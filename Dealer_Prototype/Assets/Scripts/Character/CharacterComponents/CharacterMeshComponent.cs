using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class CharacterMeshComponent : MonoBehaviour
{
    private SkinnedMeshRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<SkinnedMeshRenderer>();
    }

    public void ToggleVisibility(bool flag)
    {
        if(_renderer != null)
        {
            _renderer.enabled = flag;
        }
    }
}
