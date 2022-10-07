using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class ColorManager : Singleton<ColorManager>
{
    [Header("Debug")]
    [SerializeField] private Color Debug;

    [Header("Team")]
    [SerializeField] private Color Team_Player;
    [SerializeField] private Color Team_Enemy;
    [SerializeField] private Color Team_Neutral;

    [Header("Skin Color")]
    [SerializeField] private Color Skin_Dark;
    [SerializeField] private Color Skin_Fair;
    [SerializeField] private Color Skin_Pale;
    [SerializeField] private Color Skin_Pink;
    [SerializeField] private Color Skin_Tan;

    [Header("Hair Color")]
    [SerializeField] private Color Hair_Black;
    [SerializeField] private Color Hair_Blonde;
    [SerializeField] private Color Hair_Brunette;
    [SerializeField] private Color Hair_Ginger;

    public Color GetColor(Enumerations.Team team)
    {
        switch (team)
        {
            case Enumerations.Team.Player:
                return Team_Player;
            case Enumerations.Team.Enemy:
                return Team_Enemy;
            case Enumerations.Team.Neutral:
                return Team_Neutral;
        }

        return Debug;
    }

    public Color GetColor(Enumerations.ArenaSide side)
    {
        switch (side)
        {
            case Enumerations.ArenaSide.Defending:
                return Team_Player;
            case Enumerations.ArenaSide.Opposing:
                return Team_Enemy;
            default:
                return Debug;
        }
    }

    public Color GetColor(Enumerations.SkinColor color)
    {
        switch (color)
        {
            case Enumerations.SkinColor.Dark:
                return Skin_Dark;
            case Enumerations.SkinColor.Fair:
                return Skin_Fair;
            case Enumerations.SkinColor.Pale:
                return Skin_Pale;
            case Enumerations.SkinColor.Pink:
                return Skin_Pink;
            case Enumerations.SkinColor.Tan:
                return Skin_Tan;
        }

        return Debug;
    }

    public Color GetColor(Enumerations.HairColor color)
    {
        switch (color)
        {
            case Enumerations.HairColor.Black:
                return Hair_Black;
            case Enumerations.HairColor.Blonde:
                return Hair_Blonde;
            case Enumerations.HairColor.Brunette:
                return Hair_Brunette;
            case Enumerations.HairColor.Ginger:
                return Hair_Ginger;
        }

        return Debug;
    }
}
