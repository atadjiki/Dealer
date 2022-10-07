using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterSkinMaterialSetter : CharacterMaterialSetter
{
    [SerializeField] private Enumerations.SkinColor SkinColor;

    public override void ApplyCharacterInfo(CharacterInfo characterInfo)
    {
        base.ApplyCharacterInfo(characterInfo);

        if(characterInfo.SkinColor != Enumerations.SkinColor.None)
        {
            _meshRenderer.material.color = ColorManager.Instance.GetColor(characterInfo.SkinColor);
        }
        
    }
}
