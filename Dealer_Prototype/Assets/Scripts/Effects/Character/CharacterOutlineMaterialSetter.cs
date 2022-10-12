using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterOutlineMaterialSetter : CharacterMaterialSetter
{
    public override void ApplyCharacterInfo(CharacterInfo characterInfo)
    {
        base.ApplyCharacterInfo(characterInfo);

     //   _meshRenderer.material.SetColor("OutlineColor", ColorManager.Instance.GetColor(characterInfo.ArenaSide));
    }
}
