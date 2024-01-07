using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Constants;

public class EncounterModel : MonoBehaviour
{
    //event handling
    public delegate void EncounterStateDelegate();
    public EncounterStateDelegate OnStateChanged;

    //setup data
    private EncounterSetupData _setupData;

    //collections
    //when setup is performed, store spawned characters
    private Dictionary<TeamID, List<CharacterComponent>> _characterMap;

    //create a queue for each team each combat loop
    private Queue<CharacterComponent> _timeline;

    private EncounterState _pendingState = EncounterState.INIT;

    private bool _busy = false;

    private int _turnCount;

    private TeamID _currentTeam;

    private AbilityID _activeAbility = AbilityID.NONE;
    private CharacterComponent _activeTarget = null;
    private Vector3 _activeDestination = Vector3.zero;

    //
    //state machine

    public IEnumerator Setup(EncounterSetupData setupData)
    {
        Debug.Log("Encounter init " + this.name);

        _setupData = setupData;

        yield return new WaitForFixedUpdate();

        _turnCount = 1;
        _currentTeam = TeamID.Player; //the player always goes first

        _characterMap = new Dictionary<TeamID, List<CharacterComponent>>();

        foreach (EncounterTeamData teamData in _setupData.Teams)
        {
            _characterMap.Add(teamData.Team, new List<CharacterComponent>());

            //spawn characters for each team 
            foreach (CharacterID characterID in teamData.Characters)
            {
                GameObject characterObject = new GameObject(teamData.Team + "_" + characterID);

                characterObject.transform.parent = this.transform;
                characterObject.transform.localPosition = Vector3.zero;
                characterObject.transform.localEulerAngles = Vector3.zero;

                CharacterComponent characterComponent = characterObject.AddComponent<CharacterComponent>();
                characterComponent.SetID(characterID);

                if (characterComponent != null)
                {
                    _characterMap[teamData.Team].Add(characterComponent);
                }
            }
        }
    }

    public void TransitionState()
    {
        if(!_busy)
        {
            StartCoroutine(Coroutine_TransitionState());
        }
    }

    private IEnumerator Coroutine_TransitionState()
    {
        _busy = true;

        Debug.Log("Handling state " + _pendingState);

        switch(_pendingState)
        {
            case EncounterState.INIT:
                yield return Coroutine_Init();
                break;
            case EncounterState.SETUP_COMPLETE:
                yield return Coroutine_SetupComplete();
                break;
            case EncounterState.BUILD_QUEUES:
                yield return Coroutine_BuildTimeline();
                break;
            case EncounterState.CHECK_CONDITIONS:
                yield return Coroutine_CheckConditions();
                break;
            case EncounterState.TEAM_UPDATED:
                yield return Coroutine_TeamUpdated();
                break;
            case EncounterState.SELECT_CURRENT_CHARACTER:
                yield return Coroutine_SelectCurrentCharacter();
                break;
            case EncounterState.CHOOSE_ACTION:
                yield return Coroutine_ChooseAction();
                break;
            case EncounterState.CHOOSE_TARGET:
                yield return Coroutine_ChooseTarget();
                break;
            case EncounterState.CANCEL_ACTION:
                yield return Coroutine_CancelAction();
                break;
            case EncounterState.PERFORM_ACTION:
                yield return Coroutine_PerformAction();
                break;
            case EncounterState.DESELECT_CURRENT_CHARACTER:
                yield return Coroutine_DeselectCurrentCharacter();
                break;
            case EncounterState.UPDATE:
                yield return Coroutine_UpdateEncounter();
                break;
            case EncounterState.DONE:
                yield return Coroutine_Done();
                break;
            default:
                Debug.Log("No state transition available!");
                break;
        }

        _busy = false;

        //broadcast state change
        if (OnStateChanged != null)
        {
            OnStateChanged.Invoke();
        }

        yield return null;
    }

    private IEnumerator Coroutine_Init()
    {
        SetPendingState(EncounterState.SETUP_COMPLETE);
        yield return null;
    }

    private IEnumerator Coroutine_SetupComplete()
    {
        SetPendingState(EncounterState.BUILD_QUEUES);
        yield return null;
    }

    private IEnumerator Coroutine_BuildTimeline()
    {
        _timeline = new Queue<CharacterComponent>();

        Dictionary<TeamID, List<CharacterComponent>> teamLists = new Dictionary<TeamID, List<CharacterComponent>>();

        foreach (TeamID team in _characterMap.Keys)
        {
            teamLists.Add(team, new List<CharacterComponent>());

            foreach (CharacterComponent characterComponent in _characterMap[team])
            {
                if (characterComponent.IsAlive())
                {
                    characterComponent.ReplenishActionPoints();

                    teamLists[team].Add(characterComponent);
                }
            }
        }

        int maxCount = 0;

        foreach(List<CharacterComponent> teamList in teamLists.Values)
        {
            if(teamList.Count > maxCount)
            {
                maxCount = teamList.Count;
            }
        }

        for(int i = 0; i < maxCount; i++)
        {
            foreach(List<CharacterComponent> teamList in teamLists.Values)
            {
                if(i < teamList.Count)
                {
                    _timeline.Enqueue(teamList[i]);
                }
                else if(teamList.Count > 0)
                {
                    _timeline.Enqueue(teamList[i % teamList.Count]);
                }
            }
        }

        string debugstring = "Timeline: \n";

        foreach(CharacterComponent character in _timeline)
        {
            debugstring += character.GetID() + " (" + teamLists[character.GetTeam()].IndexOf(character) + ")\n";
        }

        Debug.Log(debugstring);

        SetPendingState(EncounterState.CHECK_CONDITIONS);
        yield return null;
    }

    private IEnumerator Coroutine_CheckConditions()
    {
        //if any queues are empty, then we are done with the encounter
        foreach (TeamID team in _characterMap.Keys)
        {
            if(IsTeamDead(team))
            {
                //call delegate
                SetPendingState(EncounterState.DONE);
                yield break;
            }
        }

        yield return null;

        SetPendingState(EncounterState.SELECT_CURRENT_CHARACTER);
    }

    private IEnumerator Coroutine_Done()
    {
        SetPendingState(EncounterState.DONE);
        yield return null;
    }

    private IEnumerator Coroutine_TeamUpdated()
    {
        SetPendingState(EncounterState.SELECT_CURRENT_CHARACTER);
        yield return null;
    }

    private IEnumerator Coroutine_SelectCurrentCharacter()
    {
        if (GetCurrentCharacter().IsAlive())
        {
            SetPendingState(EncounterState.CHOOSE_ACTION);
        }
        else
        {
            SetPendingState(EncounterState.DESELECT_CURRENT_CHARACTER);
        }

        yield return null;
    }

    private IEnumerator Coroutine_ChooseAction()
    {
        //check if we need to choose a target for this action
        AbilityID abilityID = GetActiveAbility();

        switch(GetTargetType(abilityID))
        {
            case TargetType.None:
                SetPendingState(EncounterState.PERFORM_ACTION);
                break;
            case TargetType.Enemy:
                SetPendingState(EncounterState.CHOOSE_TARGET);
                break;
            case TargetType.Ally:
                SetPendingState(EncounterState.CHOOSE_TARGET);
                break;
        }

        yield return null;
    }

    private IEnumerator Coroutine_ChooseTarget()
    {
        SetPendingState(EncounterState.PERFORM_ACTION);
        yield return null;
    }

    private IEnumerator Coroutine_CancelAction()
    {
        SetPendingState(EncounterState.CHOOSE_ACTION);
        yield return null;
    }

    private IEnumerator Coroutine_PerformAction()
    {
        CharacterComponent currentCharacter = GetCurrentCharacter();

        currentCharacter.DecrementActionPoints(GetAbilityCost(GetActiveAbility()));

        if (currentCharacter.HasActionPoints())
        {
            SetPendingState(EncounterState.SELECT_CURRENT_CHARACTER);
        }
        else
        {
            SetPendingState(EncounterState.DESELECT_CURRENT_CHARACTER);
        }

        yield return null;
    }

    private IEnumerator Coroutine_DeselectCurrentCharacter()
    {
        PopCurrentCharacter();

        SetPendingState(EncounterState.UPDATE);
        yield return null;
    }

    private IEnumerator Coroutine_UpdateEncounter()
    {
        //if the timeline is empty, time for a new round
        if (IsTimelineEmpty() || IsOpposingTeamDead())
        {
            IncrementTurnCount();
            ResetTeam();
            SetPendingState(EncounterState.BUILD_QUEUES);
        }
        //if there are still characters in the timeline, keep processing the round
        else
        {
            IncrementTeam();
            SetPendingState(EncounterState.TEAM_UPDATED);
            SetPendingState(EncounterState.SELECT_CURRENT_CHARACTER);
        }

        yield return null;
    }

    public bool IsTimelineEmpty()
    {
        return _timeline.Count == 0;
    }

    public bool IsAbilitySelected()
    {
        return GetActiveAbility() != AbilityID.NONE;
    }

    //
    //helpers

    public List<CharacterComponent> GetAllCharacters()
    {
        List<CharacterComponent> result = new List<CharacterComponent>();

        foreach (List<CharacterComponent> teamList in _characterMap.Values)
        {
            result.AddRange(teamList);
        }

        return result;
    }

    public bool IsOpposingTeamDead()
    {
        return IsTeamDead(GetOpposingTeam(GetCurrentTeam()));
    }

    public bool IsTeamDead(TeamID team)
    {
        foreach(CharacterComponent character in GetAllCharactersInTeam(team))
        {
            if(character.IsAlive())
            {
                return false;
            }
        }

        return true;
    }

    public void ResetTeam()
    {
        _currentTeam = TeamID.Player;

        Debug.Log("Current Team: " + _currentTeam);
    }

    public TeamID IncrementTeam()
    {
        switch (_currentTeam)
        {
            case TeamID.Player:
                _currentTeam = TeamID.Enemy;
                break;
            case TeamID.Enemy:
                _currentTeam = TeamID.Player;
                break;
        }

        Debug.Log("Current Team: " + _currentTeam);

        return _currentTeam;
    }

    //getters/setters

    //ability
    public void SetActiveAbility(AbilityID abilityID)
    {
        Debug.Log("Selected ability: " + abilityID);
        _activeAbility = abilityID;
    }

    public AbilityID GetActiveAbility()
    {
        return _activeAbility;
    }

    public void SetTarget(CharacterComponent target)
    {
        if(target != null)
        {
            Debug.Log("Selected target: " + target.gameObject.name);
            target.ToggleHighlight(true);

        }
        _activeTarget = target;
    }

    public CharacterComponent GetActiveTarget()
    {
        return _activeTarget;
    }

    public void SetActiveDestination(Vector3 position)
    {
        _activeDestination = position;
    }

    public Vector3 GetActiveDestination()
    {
        return _activeDestination;
    }

    public void CancelActiveAbility()
    {
        _activeAbility = AbilityID.NONE;
        _activeTarget = null;

        SetPendingState(EncounterState.CANCEL_ACTION);
    }

    //control
    public bool IsCurrentTeamCPU()
    {
        return (GetCurrentTeam() == TeamID.Enemy || _setupData.IsPlayerCPU);
 
    }

    public bool IsPlayerTurn()
    {
        return !IsCurrentTeamCPU();
    }

    //state
    public EncounterState GetState() { return _pendingState; }

    private void SetPendingState(EncounterState state)
    {
        _pendingState = state;
    }
    //characters
    public CharacterComponent GetCurrentCharacter()
    {
        return _timeline.Peek();
    }

    public bool AreTargetsAvailable()
    {
        return GetTargetCandidates().Count > 0;
    }

    public bool AreAlliesAvailable()
    {
        List<CharacterComponent> allies = GetAllCharactersInTeam(GetCurrentTeam());

        if(allies != null)
        {
            return allies.Count > 0;
        }

        return false;
    }

    public CharacterComponent GetClosestEnemy()
    {
        List<CharacterComponent> enemies = GetOpposingCharacters();

        CharacterComponent closest = null;

        if(enemies.Count > 0)
        {
            Vector3 origin = GetCurrentCharacter().GetWorldLocation();

            closest = enemies[0];

            foreach(CharacterComponent enemy in enemies)
            {
                float currentDistance = Vector3.Distance(origin, closest.GetWorldLocation());

                float targetDistance = Vector3.Distance(origin, enemy.GetWorldLocation());

                if(targetDistance < currentDistance)
                {
                    closest = enemy;
                }
            }
        }

        return closest;
    }

    public List<CharacterComponent> GetTargetCandidates()
    {
        CharacterComponent currentCharacter = GetCurrentCharacter();

        TeamID opposingTeam = GetOpposingTeam(GetCurrentTeam());

        List<CharacterComponent> candidates = new List<CharacterComponent>();

        List<CharacterComponent> OpposingCharacters = GetAllCharactersInTeam(opposingTeam);

        if(OpposingCharacters != null)
        {
            foreach (CharacterComponent enemy in OpposingCharacters)
            {
                if (enemy.IsAlive())
                {
                    if (EnvironmentUtil.IsInWeaponRange(currentCharacter, enemy))
                    {
                        candidates.Add(enemy);
                    }
                }
            }
        }

        return candidates;
    }

    public List<CharacterComponent> GetOpposingCharacters()
    {
        return GetAllCharactersInTeam(GetOpposingTeam(GetCurrentCharacter()));
    }

    public List<CharacterComponent> GetAllCharactersInTeam(TeamID teamID) 
    {
        if(_characterMap.ContainsKey(teamID))
        {
            return _characterMap[teamID];
        }
        else
        {
            return null;
        }
    }

    public void PopCurrentCharacter()
    {
        _timeline.Dequeue();
        CancelActiveAbility();
    }

    public TeamID GetCurrentTeam() { return _currentTeam; }

    //turns
    public int GetTurnCount() { return _turnCount; }

    public int IncrementTurnCount()
    {
        Debug.Log("Turn Count: " + (_turnCount + 1));
        return _turnCount++;
    }

    public int GetDeadCount(TeamID team)
    {
        int count = 0;

        foreach(CharacterComponent character in GetAllCharactersInTeam(team))
        {
            if(character.IsDead())
            {
                count++;
            }
        }

        return count;
    }

    public bool DidPlayerWin()
    {
        if(IsTeamDead(TeamID.Player))
        {
            return false;
        }

        return true;
    }
}
