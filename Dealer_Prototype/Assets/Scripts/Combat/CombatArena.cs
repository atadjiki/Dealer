using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

[System.Serializable]
public struct SideInfo
{
    public Enumerations.ArenaSide side;
    public AnchorPoint anchorPoint;
    private Roster roster;

    public void SetRoster(Roster _roster) { roster = _roster; }
    public Roster GetRoster() { return roster; }
}

public class CombatArena : MonoBehaviour
{
    [SerializeField] private SideInfo sideA;
    [SerializeField] private SideInfo sideB;

    public void SpawnTeams(Roster team_A, Roster team_B)
    {
        sideA.SetRoster(team_A);
        sideB.SetRoster(team_B);

        PopulateMarkersFromRoster(sideA);
        PopulateMarkersFromRoster(sideB);
    }

    private void PopulateMarkersFromRoster(SideInfo sideInfo)
    {
        Roster roster = sideInfo.GetRoster();
        AnchorPoint anchorPoint = sideInfo.anchorPoint;

        Debug.Log("processing team " + roster.Team);

        GameObject markerGroupPrefab = Instantiate(PrefabManager.Instance.GetMarkerGroupBySize(roster.GetRosterSize()));
        markerGroupPrefab.transform.position = anchorPoint.transform.position;

        MarkerGroup markerGroup = markerGroupPrefab.GetComponent<MarkerGroup>();

        for(int i = 0; i < roster.GetRosterSize(); i++)
        {
            SpawnCharacterOnMarker(markerGroup, roster, i);
        }
    }

    private void SpawnCharacterOnMarker(MarkerGroup markerGroup, Roster roster, int index)
    {
        if(index > (markerGroup.Markers.Length -1) || index > (roster.Characters.Length -1)) { return; } 

        CharacterMarker marker = markerGroup.Markers[index];
        marker.SetTeam(roster.Team);
       

        CharacterInfo info = roster.Characters[index];

        GameObject characterModelPrefab = PrefabManager.Instance.GetCharacterModel(info.CharacterModelID);

        GameObject spawnedCharacter = Instantiate(characterModelPrefab, marker.transform);

        CharacterModel model = spawnedCharacter.GetComponent<CharacterModel>();
        model.SetAnimationSet(info);
    }
}

