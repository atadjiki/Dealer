using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[System.Serializable]
public class CharacterInfo
{
    public string CharacterName;
    public Enumerations.WeaponID WeaponID;
    public Enumerations.CharacterModelID CharacterModelID;

    [Header("Material Settings")]
    public Enumerations.HairColor HairColor;
    public Enumerations.SkinColor SkinColor;

    [Header("Clothing")]
    public Color Clothing_TopColor;
    public Color Clothing_BottomColor;
    public Color Clothing_ShoeColor;

    [HideInInspector]
    public Enumerations.ArenaSide ArenaSide;
}

