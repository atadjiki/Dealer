using Constants;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private AnchorPoint Anchor_Defending;
    [SerializeField] private AnchorPoint Anchor_Opposing;

    public void Setup(Roster defending, Roster opposing)
    {
        defending.SetSide(Enumerations.ArenaSide.Defending);
        opposing.SetSide(Enumerations.ArenaSide.Opposing);

        SpawnRoster(defending, Anchor_Defending);
        SpawnRoster(opposing, Anchor_Opposing);
    }

    private void SpawnRoster(Roster roster, AnchorPoint anchorPoint)
    {
        if(roster.GetSize() == 0) { return; }

        MarkerGroup markerGroup = GenerateMarkers(anchorPoint, roster.GetSize());

        for (int i = 0; i < markerGroup.GetSize(); i++)
        {
            CharacterMarker marker = markerGroup.Markers[i];
            CharacterInfo info = roster.Characters[i];
            info.SetSide(roster.GetSide());

            marker.Setup(info);
        }
    }

    private MarkerGroup GenerateMarkers(AnchorPoint anchorPoint, int size)
    {
        GameObject markerGroupPrefab = Instantiate(PrefabManager.Instance.GetMarkerGroupBySize(size), anchorPoint.transform);
        markerGroupPrefab.transform.position = anchorPoint.transform.position;
        markerGroupPrefab.transform.rotation = anchorPoint.transform.rotation;

        return markerGroupPrefab.GetComponent<MarkerGroup>();
    }
}
