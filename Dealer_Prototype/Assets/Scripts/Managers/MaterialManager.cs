using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class MaterialManager : Singleton<MaterialManager>
{
    [Header("Debug")]
    [SerializeField] private Material Mat_Debug;

    [Header("Team")]
    [SerializeField] private Material Mat_Team_Player;
    [SerializeField] private Material Mat_Team_Enemy;
    [SerializeField] private Material Mat_Team_Neutral;

    [Header("Skin Color")]
    [SerializeField] private Material Mat_Skin_Dark;
    [SerializeField] private Material Mat_Skin_Fair;
    [SerializeField] private Material Mat_Skin_Pale;
    [SerializeField] private Material Mat_Skin_Pink;
    [SerializeField] private Material Mat_Skin_Tan;

    [Header("Hair Color")]
    [SerializeField] private Material Mat_Hair_Black;
    [SerializeField] private Material Mat_Hair_Blonde;
    [SerializeField] private Material Mat_Hair_Brunette;
    [SerializeField] private Material Mat_Hair_Ginger;

    public Material GetMaterialByTeam(Enumerations.Team team)
    {
        switch(team)
        {
            case Enumerations.Team.Player:
                return Mat_Team_Player;
            case Enumerations.Team.Enemy:
                return Mat_Team_Enemy;
            case Enumerations.Team.Neutral:
                return Mat_Team_Neutral;
        }

        return Mat_Debug;
    }

    public Color GetColorByArenaSide(Enumerations.ArenaSide side)
    {
        switch(side)
        {
            case Enumerations.ArenaSide.Defending:
                return Mat_Team_Player.color;
            case Enumerations.ArenaSide.Opposing:
                return Mat_Team_Enemy.color;
            default:
                return Mat_Debug.color;
        }
    }

    public Material GetMaterialBySkinColor(Enumerations.CharacterSkinColorType skinColor)
    {
        switch(skinColor)
        {
            case Enumerations.CharacterSkinColorType.Dark:
                return Mat_Skin_Dark;
            case Enumerations.CharacterSkinColorType.Fair:
                return Mat_Skin_Fair;
            case Enumerations.CharacterSkinColorType.Pale:
                return Mat_Skin_Pale;
            case Enumerations.CharacterSkinColorType.Pink:
                return Mat_Skin_Pink;
            case Enumerations.CharacterSkinColorType.Tan:
                return Mat_Skin_Tan;
        }

        return Mat_Debug;
    }

    public Material GetMaterialByHairColor(Enumerations.HairColor hairColor)
    {
        switch(hairColor)
        {
            case Enumerations.HairColor.Black:
                return Mat_Hair_Black;
            case Enumerations.HairColor.Blonde:
                return Mat_Hair_Blonde;
            case Enumerations.HairColor.Brunette:
                return Mat_Hair_Brunette;
            case Enumerations.HairColor.Ginger:
                return Mat_Hair_Ginger;
        }

        return Mat_Debug;
    }
}
