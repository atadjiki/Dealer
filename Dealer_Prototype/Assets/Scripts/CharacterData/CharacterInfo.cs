using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[System.Serializable]
public class CharacterInfo 
{
    [Header("General Data")]
    public string CharacterName;
    public int Health;
    public int Level;

    public Enumerations.WeaponID WeaponID;
    public Enumerations.CharacterModelID CharacterModelID;

    [Header("Material Settings")]
    public Color HairColor;
    public Color SkinColor;

    [Header("Clothing")]
    public Color TopColor;
    public Color BottomColor;
    public Color ShoeColor;

    [HideInInspector]
    public Enumerations.ArenaSide ArenaSide;

    void Reset()
    {
        CharacterName = "John Doe";
        WeaponID = Enumerations.WeaponID.Glock;
        CharacterModelID = Enumerations.CharacterModelID.Male_1;


        HairColor = new Color(0.1568628f, 0.1568628f, 0.1568628f, 1);
        SkinColor = new Color(0.8470588f, 0.5647059f, 0.372549f, 1);

        TopColor = new Color(0.5f, 0.5f, 0.5f, 1);
        BottomColor = new Color(0.5f, 0.5f, 0.5f, 1);
        ShoeColor = new Color(0, 0, 0);
    }
}
