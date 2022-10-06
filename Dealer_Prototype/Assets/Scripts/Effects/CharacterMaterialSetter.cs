using Constants;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class CharacterMaterialSetter : MaterialSetter
{
   protected SkinnedMeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<SkinnedMeshRenderer>();    
    }

    public virtual void ApplyCharacterInfo(CharacterInfo characterInfo) 
    {
    }
}
