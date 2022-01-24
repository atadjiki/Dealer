using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterStateComponent : ObjectStateComponent
{
    [Header("Character ID")]
    [SerializeField] private CharacterConstants.CharacterID CharacterID;

    [Header("Gender")]
    [SerializeField] private CharacterConstants.GenderType Gender;

    [Header("Team")]
    [SerializeField] private CharacterConstants.Team Team;

    public void SetCharacterID(CharacterConstants.CharacterID _ID) { CharacterID = _ID; }

    public void SetTeam(CharacterConstants.Team _Team) { Team = _Team; }

    public CharacterConstants.Team GetTeam()
    {
        return Team;
    }

    public override string GetID()
    {
        return CharacterID.ToString();
    }

    public CharacterConstants.GenderType GetGender()
    {
        return Gender;
    }

    public void SetGender(CharacterConstants.GenderType _gender)
    {
        Gender = _gender;
    }
}
