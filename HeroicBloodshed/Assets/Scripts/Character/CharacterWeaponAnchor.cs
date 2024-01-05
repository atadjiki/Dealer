using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public struct WeaponOffsetData
{
    public WeaponID ID;
    public Transform Offset;
}

public class CharacterWeaponAnchor : MonoBehaviour
{
    [SerializeField] private List<WeaponOffsetData> OffsetData;

    public Transform GetOffset(WeaponID weaponID)
    {
        foreach(WeaponOffsetData data in OffsetData)
        {
            if(data.ID == weaponID)
            {
                return data.Offset;
            }
        }

        return this.transform;
    }
}
