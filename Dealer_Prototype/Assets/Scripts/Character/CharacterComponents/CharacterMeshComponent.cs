using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class CharacterMeshComponent : MonoBehaviour
{
    [SerializeField] private CharacterConstants.CharacterMeshType _type;

    private SkinnedMeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SkinnedMeshRenderer>();
    }

    public void ToggleVisibility(bool flag)
    {
        _renderer.enabled = flag;
    }
}
