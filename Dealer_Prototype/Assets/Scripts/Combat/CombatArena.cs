using UnityEngine;

[System.Serializable]
public struct RosterContext
{
    public AnchorPoint anchorPoint;

    private Roster roster;
    private MarkerGroup markerGroup;

    public void SetRoster(Roster _roster) { roster = _roster; }
    public Roster GetRoster() { return roster; }

    public void SetMarkerGroup(MarkerGroup _group) { markerGroup = _group; }
    public MarkerGroup GetMarkerGroup() { return markerGroup; }
}

public class CombatArena : MonoBehaviour
{
    [SerializeField] private RosterContext context_defending;
    [SerializeField] private RosterContext context_opposing;

    public void SpawnDefendingTeam(Roster roster) { SpawnRoster(context_defending, roster); }
    public void SpawnOpposingTeam(Roster roster) { SpawnRoster(context_opposing, roster); }

    //spawn rosters
    private void SpawnRoster(RosterContext rosterContext, Roster roster)
    {
        rosterContext.SetRoster(roster);
        PopulateMarkersFromRoster(rosterContext);
    }

    private void PopulateMarkersFromRoster(RosterContext rosterContext)
    {
        Roster roster = rosterContext.GetRoster();
        AnchorPoint anchorPoint = rosterContext.anchorPoint;

        //spawn the marker group that we will use to place our characters from the roster
        GameObject markerGroupPrefab = Instantiate(PrefabManager.Instance.GetMarkerGroupBySize(roster.GetRosterSize()), rosterContext.anchorPoint.transform);
        markerGroupPrefab.transform.position = anchorPoint.transform.position;
        markerGroupPrefab.transform.eulerAngles = anchorPoint.transform.eulerAngles;

        markerGroupPrefab.name = "MarkerGroup_Team_" + rosterContext.GetRoster().Team;
    
        rosterContext.SetMarkerGroup(markerGroupPrefab.GetComponent<MarkerGroup>());

        //spawn individual characters
        for(int i = 0; i < roster.GetRosterSize(); i++)
        {
            SpawnCharacterOnMarker(rosterContext, i);
        }
    }

    private void SpawnCharacterOnMarker(RosterContext rosterContext, int index)
    {
        Roster roster = rosterContext.GetRoster();
        MarkerGroup markerGroup = rosterContext.GetMarkerGroup();

        if(index > (markerGroup.Markers.Length -1) || index > (roster.Characters.Length -1)) { return; } 

        CharacterMarker marker = markerGroup.Markers[index];

        CharacterInfo info = roster.Characters[index];

        GameObject characterModelPrefab = PrefabManager.Instance.GetCharacterModel(info.CharacterModelID);

        GameObject spawnedCharacter = Instantiate(characterModelPrefab, marker.transform);
        spawnedCharacter.transform.eulerAngles = marker.transform.eulerAngles;

        CharacterModel model = spawnedCharacter.GetComponent<CharacterModel>();
        model.SetAnimationSet(info);
    }
}

