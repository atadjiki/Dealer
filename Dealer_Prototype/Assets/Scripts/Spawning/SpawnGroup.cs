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
    private AIConstants.Mode Mode;
    private CharacterScheduledTask[] ScheduledTasks;

    public void SetTeam(CharacterConstants.Team InTeam) { Team = InTeam; }
    public CharacterConstants.Team GetTeam() { return Team; }

    public void SetMode(AIConstants.Mode InMode) { Mode = InMode; }
    public AIConstants.Mode GetMode() { return Mode; }

    public void SetScheduledTasks(CharacterScheduledTask[] InTasks) { ScheduledTasks = InTasks; }
    public CharacterScheduledTask[] GetScheduledTasks() { return ScheduledTasks; }

}

[System.Serializable]
public struct SpawnGroup
{
    public SpawnData Data;
    public int Size;
    public float Initial_Delay;
    public float Delay_Between;
}
