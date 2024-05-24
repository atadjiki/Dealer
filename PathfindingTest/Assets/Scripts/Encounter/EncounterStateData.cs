using System;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[Serializable]
public struct EncounterStateData 
{
    public delegate void EncounterStateDelegate(EncounterStateData State);
    public static EncounterStateDelegate OnStateChanged;

    public Dictionary<TeamID, List<CharacterComponent>> CharacterMap;

    public Queue<CharacterComponent> Timeline;

    public EncounterState CurrentState;

    public int TurnCount;

    public void AddCharacter(TeamID team, CharacterComponent character)
    {
        CharacterMap[team].Add(character);
    }

    public void PopCurrentCharacter()
    {
        Debug.Log("Popping current character");

        CancelActiveAbility();

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

    public bool IsOpposingTeamDead()
    {
        return IsTeamDead(GetOpposingTeam(GetCurrentTeam()));
    }

    public TeamID GetCurrentTeam()
    {
        CharacterComponent currentCharacter = GetCurrentCharacter();

        return GetTeam(currentCharacter);
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

            TurnCount = 0,
        };
    }

    public EncounterState GetCurrentState()
    {
        return CurrentState;
    }

    public void SetPendingState(EncounterState state)
    {
        if (state != CurrentState)
        {
            CurrentState = state;
        }
    }

    public void BroadcastState()
    {
        if (OnStateChanged != null)
        {
            OnStateChanged.Invoke(this);
        }
    }

    public void CancelActiveAbility()
    {
        CharacterComponent currentCharacter = GetCurrentCharacter();
        currentCharacter.ResetForTurn();
        SetPendingState(EncounterState.CANCEL_ACTION);
    }

    public int IncrementTurnCount()
    {
        Debug.Log("Turn Count: " + (TurnCount + 1));
        return TurnCount++;
    }

    public AbilityID GetActiveAbility()
    {
        CharacterComponent currentCharacter = GetCurrentCharacter();
        return currentCharacter.GetActiveAbility();
    }

    public void SetActiveAbility(AbilityID ability)
    {
        CharacterComponent currentCharacter = GetCurrentCharacter();
        currentCharacter.SetActiveAbility(ability);
    }

    public void SetActiveDestination(Vector3 destination)
    {
        CharacterComponent currentCharacter = GetCurrentCharacter();
        currentCharacter.SetActiveDestination(destination);
    }

    public bool AreActionPointsAvailable()
    {
        CharacterComponent currentCharacter = GetCurrentCharacter();

        return currentCharacter.HasActionPoints();
    }
}