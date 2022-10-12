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

    [HideInInspector]
    public Enumerations.ArenaSide ArenaSide;
}
