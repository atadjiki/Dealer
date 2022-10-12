using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterHairMaterialSetter : CharacterMaterialSetter
{
    public override void ApplyCharacterInfo(CharacterInfo characterInfo)
    {
        base.ApplyCharacterInfo(characterInfo);

        _meshRenderer.material.color = ColorManager.Instance.GetColor(characterInfo.ArenaSide);
    }
}
