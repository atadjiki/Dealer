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

    public static HighlightProfile Get(TeamID ID)
    {
        Refresh();

        foreach(TeamHighlightInfo info in library.TeamProfiles)
        {
            if(info.ID == ID)
            {
                return info.Profile;
            }
        }

        return null;
    }

    public static HighlightProfile Get(MovementRangeType ID)
    {
        Refresh();

        foreach (MovementRadiusInfo info in library.MovementRadiusProfiles)
        {
            if (info.ID == ID)
            {
                return info.Profile;
            }
        }

        return null;
    }


    private static HighlightProfileLibrary library;

    private static void Refresh()
    {
        if (library == null)
        {
            library = Load();
        }
    }

    public static HighlightProfileLibrary Load()
    {
        Debug.Log("Loading HighlightProfileLibrary");
        return Resources.Load<HighlightProfileLibrary>("Data/Libraries/HighlightProfileLibrary");
    }
}
