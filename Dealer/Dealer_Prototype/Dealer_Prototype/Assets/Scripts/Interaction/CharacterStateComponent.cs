using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterStateComponent : ObjectStateComponent
{
    [Header("Character ID")]
    [SerializeField] private CharacterConstants.CharacterID CharacterID;

    public void SetCharacterID(CharacterConstants.CharacterID _ID) { CharacterID = _ID; }

    public override string GetID()
    {
        return CharacterID.ToString();
    }
}
