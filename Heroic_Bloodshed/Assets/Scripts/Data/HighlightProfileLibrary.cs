using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightPlus;
using System;
using static Constants;

[Serializable]
public struct MovementRadiusInfo
{
    public MovementRangeType ID;
    public HighlightProfile Profile;
}

[Serializable]
public struct TeamHighlightInfo
{
    public TeamID ID;
    public HighlightProfile Profile;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Library/HighlightProfileLibrary", order = 1)]
public class HighlightProfileLibrary : ScriptableObject
{
    [Header("Teams")]
    [SerializeField] private List<TeamHighlightInfo> TeamProfiles;

    [Header("Components")]
    [SerializeField] private List<MovementRadiusInfo> MovementRadiusProfiles;

    private static HighlightProfileLibrary library;

    public HighlightProfile GetTeamProfile(TeamID ID)
    {
        foreach(TeamHighlightInfo info in TeamProfiles)
        {
            if(info.ID == ID)
            {
                return info.Profile;
            }
        }

        return null;
    }

    public HighlightProfile GetMovementRadiusProfile(MovementRangeType ID)
    {
        foreach (MovementRadiusInfo info in MovementRadiusProfiles)
        {
            if (info.ID == ID)
            {
                return info.Profile;
            }
        }

        return null;
    }

    public static HighlightProfileLibrary Get()
    {
        if (library != null)
        {
            return library;
        }
        else
        {
            library = Load();
            return library;
        }
    }

    public static HighlightProfileLibrary Load()
    {
        Debug.Log("Loading HighlightProfileLibrary");
        return Resources.Load<HighlightProfileLibrary>("Data/Libraries/HighlightProfileLibrary");
    }
}
