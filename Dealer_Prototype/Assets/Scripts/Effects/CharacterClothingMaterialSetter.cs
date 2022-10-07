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

        switch(ClothingType)
        {
            case Enumerations.CharacterClothingType.Top:
                _meshRenderer.material.color = characterInfo.TopColor;
                break;
            case Enumerations.CharacterClothingType.Bottom:
                _meshRenderer.material.color = characterInfo.BottomColor;
                break;
            case Enumerations.CharacterClothingType.Shoes:
                _meshRenderer.material.color = characterInfo.ShoeColor;
                break;
            
        }
    }
}
