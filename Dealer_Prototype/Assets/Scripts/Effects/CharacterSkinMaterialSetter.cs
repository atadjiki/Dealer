using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterSkinMaterialSetter : CharacterMaterialSetter
{
    [SerializeField] private Enumerations.CharacterSkinColorType SkinColor;

    public override void ApplyCharacterInfo(CharacterInfo characterInfo)
    {
        base.ApplyCharacterInfo(characterInfo);
    }
}
