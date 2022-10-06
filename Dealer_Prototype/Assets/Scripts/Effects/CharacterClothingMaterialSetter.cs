using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterClothingMaterialSetter : CharacterMaterialSetter
{ 
    [SerializeField] private Enumerations.CharacterClothingType ClothingType;

    public override void ApplyCharacterInfo(CharacterInfo characterInfo)
    {
        base.ApplyCharacterInfo(characterInfo);
    }
}
