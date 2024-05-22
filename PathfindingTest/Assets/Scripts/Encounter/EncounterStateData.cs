using System;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public struct EncounterStateData
{
    public Dictionary<TeamID, List<CharacterComponent>> CharacterMap;

    public Dictionary<TeamID, Queue<CharacterComponent>> TeamQueues;

    public EncounterState CurrentState;
    public TeamID CurrentTeam;

    public int TurnCount;

    public bool Busy;


    public void PopCurrentCharacter()
    {
        if (TeamQueues != null)
        {
            if (TeamQueues[CurrentTeam] != null)
            {
                if (TeamQueues[CurrentTeam].Count > 0)
                {
                    TeamQueues[CurrentTeam].Dequeue();
                }
            }
        }
    }

    public bool IsCurrentCharacterAlive()
    {
        return GetCurrentCharacter().IsAlive();
    }

    public CharacterComponent GetCurrentCharacter()
    {
        if (TeamQueues != null)
        {
            if (TeamQueues[CurrentTeam] != null)
            {
                if (TeamQueues[CurrentTeam].Count > 0)
                {
                    return TeamQueues[CurrentTeam].Peek();
                }
            }
        }

        return null;
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

    public void BuildAllTeamQueues()
    {
        foreach (TeamID team in CharacterMap.Keys)
        {
            BuildTeamQueue(team);
        }
    }

    public void BuildTeamQueue(TeamID team)
    {
        TeamQueues[team].Clear();

        string debugString = team.ToString() + " Queue: ";

        foreach (CharacterComponent character in CharacterMap[team])
        {
            if (character.IsAlive())
            {
                TeamQueues[team].Enqueue(character);
                debugString += character.GetID().ToString() + " ";
            }
        }

        Debug.Log(debugString);
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
        foreach (Queue<CharacterComponent> queue in TeamQueues.Values)
        {
            if (queue.Count > 0)
            {
                return false;
            }
        }

        return true;
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
                { TeamID.ENEMY,  new List<CharacterComponent>() }
            },

            TeamQueues = new Dictionary<TeamID, Queue<CharacterComponent>>()
            {
                { TeamID.PLAYER, new Queue<CharacterComponent>() },
                { TeamID.ENEMY,  new Queue<CharacterComponent>() },

            },

            CurrentState = EncounterState.NONE,
            CurrentTeam = TeamID.PLAYER,

            TurnCount = 0,

            Busy = false
        };
    }
}