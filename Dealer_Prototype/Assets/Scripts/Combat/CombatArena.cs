using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[System.Serializable]
public class MarkerGroup
{
    public Enumerations.ArenaSide Side;
    public CharacterMarker[] Markers = new CharacterMarker[CombatConstants.RosterSize];
}

public class CombatArena : MonoBehaviour
{
    public MarkerGroup Markers_A;
    public MarkerGroup Markers_B;

    public void SpawnTeams(Roster Team_A, Roster Team_B)
    {
        PopulateMarkersFromRoster(Markers_A, Team_A);
        PopulateMarkersFromRoster(Markers_B, Team_B);
    }

    private void PopulateMarkersFromRoster(MarkerGroup MarkerGroup, Roster Roster)
    {
        Debug.Log("processing team " + Roster.Team);

        for(int i = 0; i < CombatConstants.RosterSize; i++)
        {
            SpawnCharacterOnMarker(MarkerGroup, Roster, i);
        }
    }

    private void SpawnCharacterOnMarker(MarkerGroup MarkerGroup, Roster Roster, int index)
    {
        if(index > (MarkerGroup.Markers.Length -1) || index > (Roster.Characters.Length -1)) { return; } 

        CharacterMarker marker = MarkerGroup.Markers[index];
        marker.SetTeam(Roster.Team);
        marker.SetSide(MarkerGroup.Side);

        CharacterInfo info = Roster.Characters[index];

        GameObject spawnedCharacter = Instantiate(info.Model.gameObject, marker.transform);
        CharacterModel model = spawnedCharacter.GetComponent<CharacterModel>();
        model.SetAnimationSet(info);
    }
}

