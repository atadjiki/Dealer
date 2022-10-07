using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMaterialSetter : ObjectMaterialSetter
{
    public override void ApplyCharacterInfo(CharacterInfo characterInfo)
    {
        base.ApplyCharacterInfo(characterInfo);

        _meshRenderer.material.SetColor("OutlineColor", ColorManager.Instance.GetColor(characterInfo.ArenaSide));
    }
}
