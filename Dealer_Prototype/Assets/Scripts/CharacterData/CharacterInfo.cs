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

    private Enumerations.ArenaSide ArenaSide;
    public Enumerations.ArenaSide GetSide() { return ArenaSide; }
    public void SetSide(Enumerations.ArenaSide side) { ArenaSide = side; }
}

