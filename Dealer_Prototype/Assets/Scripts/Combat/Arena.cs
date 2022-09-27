using Constants;
using UnityEngine;

[System.Serializable]
public class ArenaLayout
{
    public AnchorPoint anchorPoint;
    private MarkerGroup markerGroup;
    public void SetMarkerGroup(MarkerGroup group) { markerGroup = group; }
    public MarkerGroup GetMarkerGroup() { return markerGroup; }
}

public class Arena : MonoBehaviour
{
    [SerializeField] private ArenaLayout Layout_Defending;
    [SerializeField] private ArenaLayout Layout_Opposing;

    public void Setup(Roster defending, Roster opposing)
    {
        GenerateMarkers(Layout_Defending, defending.GetSize());
        GenerateMarkers(Layout_Opposing, opposing.GetSize());

        SpawnRoster(defending, GetDefendingMarkers());
        SpawnRoster(opposing, GetOpposingMarkers());
    }

    private void SpawnRoster(Roster roster, MarkerGroup markerGroup)
    {
        for (int i = 0; i < markerGroup.GetSize(); i++)
        {
            CharacterMarker marker = markerGroup.Markers[i];
            CharacterInfo info = roster.Characters[i];

            GameObject spawnedCharacter = Instantiate(PrefabManager.Instance.GetCharacterModel(info.CharacterModelID), marker.transform);
            spawnedCharacter.transform.eulerAngles = marker.transform.eulerAngles;

            CharacterModel model = spawnedCharacter.GetComponent<CharacterModel>();
            model.Setup(info);
        }
    }

    private void GenerateMarkers(ArenaLayout layout, int size)
    {
        GameObject markerGroupPrefab = Instantiate(PrefabManager.Instance.GetMarkerGroupBySize(size), layout.anchorPoint.transform);
        markerGroupPrefab.transform.position = layout.anchorPoint.transform.position;
        markerGroupPrefab.transform.rotation = layout.anchorPoint.transform.rotation;
        MarkerGroup markerGroup = markerGroupPrefab.GetComponent<MarkerGroup>();

        layout.SetMarkerGroup(markerGroup);
    }

    //helpers
    public ArenaLayout GetDefendingLayout() { return Layout_Defending; }
    public ArenaLayout GetOpposingLayout() { return Layout_Opposing; }

    public MarkerGroup GetDefendingMarkers() { return Layout_Defending.GetMarkerGroup(); }
    public MarkerGroup GetOpposingMarkers() { return Layout_Opposing.GetMarkerGroup(); }

}

