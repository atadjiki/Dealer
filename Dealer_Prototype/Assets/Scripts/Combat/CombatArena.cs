using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//describes the characters, arena, markers, involved for a combat scenario
public class CombatArena : MonoBehaviour
{
    [Header("Teams")]
    [SerializeField] private Roster Team_A;
    [SerializeField] private Roster Team_B;

    [Header("Markers")]
    [SerializeField] private CharacterMarker[] Markers_Team_A;
    [SerializeField] private CharacterMarker[] Markers_Team_B;

    private void Start()
    {
       foreach(CharacterMarker marker in Markers_Team_A)
        {
            marker.SetTeam(Team_A.Team);
        }

        foreach (CharacterMarker marker in Markers_Team_B)
        {
            marker.SetTeam(Team_B.Team);
        }
    }

}
