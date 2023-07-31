using System;
using System.Collections;
using System.Collections.Generic;

using Cinemachine;
using UnityEngine;
using static Constants;

public class Encounter : MonoBehaviour
{
    [Header("Teams")]
    [SerializeField] private List<EncounterTeamData> Teams;
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera VirtualCamera;

    private EncounterState _state;

    private void Awake()
    {
        _state = new EncounterState(Teams);
    }

    //Public
    public void SpawnCharacters()
    {
        foreach (CharacterComponent characterComponent in _state.GetAllCharacters())
        {
            characterComponent.SetupModel();
            characterComponent.SetupWeapon();
        }
    }

    public void DespawnCharacters()
    {
        foreach (CharacterComponent characterComponent in _state.GetAllCharacters())
        {
            characterComponent.DestroyModel();
            characterComponent.DestroyWeapon();
        }
    }

    //focus on the character who is currently performing an ability
    public void PrepareTurn()
    {
        CharacterComponent currentCharacter = _state.GetCurrentCharacter();

        //create decal
        currentCharacter.CreateDecal();

        //focus camera on character
        VirtualCamera.Follow = currentCharacter.transform;

        //show appropriate UI

        ProcessTurn();
    }

    //allow every team to perform an ability, then build the queues again 
    public void ProcessTurn()
    {
        //foreach team
        //foreach character in team

        TeamID CurrentTeam = _state.GetCurrentTeam();

        if(CurrentTeam == TeamID.Player)
        {
            ChooseCharacterAbility();
            return;
        }
        else if(CurrentTeam == TeamID.Enemy)
        {
            ChooseCharacterAbility();
            return;
        }
    }

    //non player AI will use this to determine their action for the turn 
    public void ChooseCharacterAbility()
    {

        CharacterComponent currentCharacter = _state.GetCurrentCharacter();

        AbilityID chosenAbility = GetAllowedAbilities(currentCharacter.GetID())[0];

        PerformCharacterAbility(chosenAbility);
    }

    //perform ability for the currently selected character
    public void PerformCharacterAbility(AbilityID abilityID)
    {
        //block input, play animation, do effects, update character data to reflect, update UI
        switch(abilityID)
        {
            default:
                Debug.Log("Performing ability " + abilityID);
                StartCoroutine(Coroutine_PerformAbility_SkipTurn());
                break;
        }
    }

    private IEnumerator Coroutine_PerformAbility_SkipTurn()
    {
        yield return new WaitForSeconds(2.5f);

        CleanupTurn();
    }

    //deselect current character before starting a new turn 
    public void CleanupTurn()
    {
        //deselect the current character 
        CharacterComponent currentCharacter = _state.GetCurrentCharacter();

        //destroy any decals
        currentCharacter.DestroyDecal();

        //recenter camera
        VirtualCamera.Follow = null;

        //pop the current character from the queue 
        _state.PopCurrentCharacter();

        //update the encounter state 
        UpdateState();
    }

    public void UpdateState()
    {
        _state.IncrementTurnCount();

        //check if we are done with this team 
        if (_state.IsCurrentTeamQueueEmpty())
        {
            Debug.Log(_state.GetCurrentTeam() + " team done");

            if(_state.IncrementTeam() == TeamID.None)
            {
                //if all teams have gone, then time to rebuild queues
                _state.BuildTeamQueues();
                return;
            }
        }

        PrepareTurn();
    }
}
