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

        Color color = ColorManager.Instance.GetColor(characterInfo.ArenaSide);

        switch (ClothingType)
        {
            case Enumerations.CharacterClothingType.Top:
                _meshRenderer.material.color = color;
                break;
            case Enumerations.CharacterClothingType.Bottom:
                _meshRenderer.material.color = color;
                break;
            case Enumerations.CharacterClothingType.Shoes:
                _meshRenderer.material.color = color;
                break;
            
        }
    }
}
