using System;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public struct EncounterStateData
{
    public Dictionary<TeamID, List<CharacterComponent>> CharacterMap;

    public Queue<CharacterComponent> Timeline;

    public EncounterState CurrentState;
    public TeamID CurrentTeam;

    public int TurnCount;

    public bool Busy;


    public void PopCurrentCharacter()
    {
        Debug.Log("Popping current character");

        Timeline.Dequeue();

        Debug.Log(MakeTimelineString());
    }

    public bool IsCurrentCharacterAlive()
    {
        return GetCurrentCharacter().IsAlive();
    }

    public CharacterComponent GetCurrentCharacter()
    {
        return Timeline.Peek();
    }

    public List<CharacterComponent> GetAllCharacters()
    {
        List<CharacterComponent> result = new List<CharacterComponent>();

        foreach (List<CharacterComponent> teamList in CharacterMap.Values)
        {
            result.AddRange(teamList);
        }

        return result;
    }

    public List<CharacterComponent> GetCharactersInTeam(TeamID team)
    {
        return CharacterMap[team];
    }

    public void BuildTimeline()
    {
        foreach (TeamID team in CharacterMap.Keys)
        {
            foreach (CharacterComponent character in CharacterMap[team])
            {
                if (character.IsAlive())
                {
                    Timeline.Enqueue(character);
                }
            }
        }

        Debug.Log(MakeTimelineString());
    }

    public string MakeTimelineString()
    {
        string debugString = "Timeline: ";

        foreach (CharacterComponent character in Timeline.ToArray())
        {
            if (character.IsAlive())
            {
                debugString += character.GetID().ToString() + " ";
            }
        }

        return debugString;
    }

    public bool AreAnyTeamsDead()
    {
        foreach (TeamID team in CharacterMap.Keys)
        {
            if (CharacterMap[team].Count > 0)
            {
                if (IsTeamDead(team))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsTeamDead(TeamID team)
    {
        if(CharacterMap[team].Count == 0)
        {
            return false;
        }

        foreach (CharacterComponent character in CharacterMap[team])
        {
            if (character.IsAlive())
            {
                return false;
            }
        }

        Debug.Log("Team " + team.ToString() + " is dead");
        return true;
    }

    public bool IsTimelineEmpty()
    {
        return (Timeline.Count == 0);
    }

    public TeamID IncrementTeam()
    {
        switch (CurrentTeam)
        {
            case TeamID.PLAYER:
                CurrentTeam = TeamID.ENEMY;
                break;
            case TeamID.ENEMY:
                CurrentTeam = TeamID.PLAYER;
                break;
        }

        Debug.Log("Current Team: " + CurrentTeam);

        return CurrentTeam;
    }

    public bool IsOpposingTeamDead()
    {
        return IsTeamDead(GetOpposingTeam(CurrentTeam));
    }

    public void ResetTeam()
    {
        CurrentTeam = TeamID.PLAYER;

        Debug.Log("Current Team: " + CurrentTeam);
    }

    public static EncounterStateData Build()
    {
        return new EncounterStateData()
        {
            CharacterMap = new Dictionary<TeamID, List<CharacterComponent>>()
            {
                { TeamID.PLAYER, new List<CharacterComponent>() },
                { TeamID.ENEMY, new List<CharacterComponent>() }
            },

            Timeline = new Queue<CharacterComponent>(),

            CurrentState = EncounterState.NONE,
            CurrentTeam = TeamID.PLAYER,

            TurnCount = 0,

            Busy = false
        };
    }
}