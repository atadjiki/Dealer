using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[System.Serializable]
public class CharacterInfo 
{
    public string CharacterName = "John Doe";
    public Enumerations.WeaponID WeaponID = Enumerations.WeaponID.Glock;
    public Enumerations.CharacterModelID CharacterModelID = Enumerations.CharacterModelID.Male_1;

    [Header("Material Settings")]
    public Color32 HairColor = new Color(0.1568628f, 0.1568628f, 0.1568628f, 1);
    public Color32 SkinColor = new Color(0.8470588f, 0.5647059f, 0.372549f, 1);

    [Header("Clothing")]
    public Color32 TopColor = new Color(0.5f,0.5f,0.5f,1);
    public Color32 BottomColor = new Color(0.5f, 0.5f, 0.5f, 1);
    public Color ShoeColor = new Color(0, 0, 0);

    [HideInInspector]
    public Enumerations.ArenaSide ArenaSide;
}
