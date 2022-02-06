using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[System.Serializable]
public struct SpawnData
{
    public CharacterConstants.CharacterID ID;
    public AnimationConstants.Anim InitialAnim;

    private CharacterConstants.Team Team;
    private CharacterConstants.Mode Mode;

    public void SetTeam(CharacterConstants.Team InTeam) { Team = InTeam; }
    public CharacterConstants.Team GetTeam() { return Team; }

    private void SetMode(CharacterConstants.Mode InMode) { Mode = InMode; }
    private CharacterConstants.Mode GetMode() { return Mode; }
}

[System.Serializable]
public struct SpawnGroup
{
    public SpawnData Data;
    public int Size;
    public float Initial_Delay;
    public float Delay_Between;
}
