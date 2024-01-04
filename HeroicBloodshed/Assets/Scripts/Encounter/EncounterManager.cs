using System;
using System.Collections;
using System.Collections.Generic;

using Cinemachine;
using UnityEngine;
using static Constants;

public class EncounterManager : MonoBehaviour
{
    [Header("Setup Data")]
    [SerializeField] private EncounterSetupData _setupData; //setup data

    [Header("Prefabs")]
    [SerializeField] private GameObject Prefab_EncounterCanvas;
    [SerializeField] private GameObject Prefab_EncounterAudioManager;

    private static EncounterManager _instance;

    public static EncounterManager Instance { get { return _instance; } }

    private EncounterModel _model;

    private List<EncounterEventReceiver> _eventReceivers;

    private EncounterCanvasManager _canvas;
    private EncounterAudioManager _audioManager;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    private void Build()
    {
        LoadEncounter();
    }

    public void LoadEncounter()
    {
        StartCoroutine(Coroutine_LoadEncounter());
    }

    private IEnumerator Coroutine_LoadEncounter()
    {
        Debug.Log("Waiting on environment");
        yield return EnvironmentManager.Instance.Coroutine_Build();

        _eventReceivers = new List<EncounterEventReceiver>();

        //create a canvas 
        GameObject canvasObject = Instantiate(Prefab_EncounterCanvas, this.transform);
        yield return new WaitWhile(() => canvasObject.GetComponent<EncounterCanvasManager>() == null);
        _canvas = canvasObject.GetComponent<EncounterCanvasManager>();
        _eventReceivers.Add(_canvas);

        //create audio manager
        GameObject audioManagerObject = Instantiate(Prefab_EncounterAudioManager, this.transform);
        yield return new WaitWhile(() => audioManagerObject.GetComponent<EncounterAudioManager>() == null);
        _audioManager= audioManagerObject.GetComponent<EncounterAudioManager>();
        _eventReceivers.Add(_audioManager);

        //generate encounter and attach to handler
        _model = _setupData.gameObject.AddComponent<EncounterModel>();
        yield return new WaitWhile(() => _setupData.gameObject.GetComponent<EncounterModel>() == null);

        _model.OnStateChanged += this.EncounterStateCallback;

        yield return _model.Setup(_setupData);

        yield return SpawnCharacters();

        foreach (EncounterEventReceiver receiver in _eventReceivers)
        {
            yield return receiver.Coroutine_Init(_model);
        }

        _model.TransitionState();

        yield return null;
    }

    private void EncounterStateCallback()
    {
        StartCoroutine(Coroutine_EncounterStateCallback());
    }

    private IEnumerator Coroutine_EncounterStateCallback()
    {
        EncounterState state = _model.GetState();

        foreach (EncounterEventReceiver receiver in _eventReceivers)
        {
            yield return receiver.Coroutine_StateUpdate(state, _model);
        }

        //broadcast to other managers as well
        yield return EnvironmentManager.Instance.Coroutine_EncounterStateUpdate(state, _model);

        switch (state)
        {
            case EncounterState.SELECT_CURRENT_CHARACTER:
            {
                CharacterComponent character = _model.GetCurrentCharacter();
                CameraRig.Instance.Follow(character);

                if (character.IsAlive())
                {
                    character.HandleEvent(CharacterEvent.SELECTED);
                }
                break;
            }
            case EncounterState.TEAM_UPDATED:
            {
                yield return new WaitForSeconds(1.0f);
                break;
            }
            case EncounterState.CHOOSE_ACTION:
            {
                yield return Coroutine_ChooseAction();
                yield break;
            }
            case EncounterState.CHOOSE_TARGET:
            {
                yield return Coroutine_ChooseTarget();
                yield break;
            }
            case EncounterState.DESELECT_CURRENT_CHARACTER:
            {
                CharacterComponent character = _model.GetCurrentCharacter();
                CameraRig.Instance.Unfollow();
                character.HandleEvent(CharacterEvent.DESELECTED);
                break;
            }
            case EncounterState.PERFORM_ACTION:
            {
                CharacterComponent currentCharacter = _model.GetCurrentCharacter();
                yield return PerformAbility(currentCharacter);
                break;
            }
            case EncounterState.DONE:
            {
                foreach(CharacterComponent character in _model.GetAllCharacters())
                {
                    character.DestroyEncounterOverhead();
                }
                _model.OnStateChanged -= EncounterStateCallback;
                break;
            }
            default:
            {
                break;
            }
        }

        if (_model != null)
        {
            _model.TransitionState();
        }

        yield return null;
    }

    private IEnumerator Coroutine_ChooseAction()
    {
        if (_model.IsCurrentTeamCPU())
        {
            CharacterComponent currentCharacter = _model.GetCurrentCharacter();

            if (_model.AreTargetsAvailable())
            {
                //TODO
                //if(currentCharacter.GetRemainingAmmo() == 0)
                //{
                //    _model.SetActiveAbility(AbilityID.Reload);
                //}
                //else
                //{
                //    _model.SetActiveAbility(AbilityID.FireWeapon);
                //}

                _model.SetActiveAbility(AbilityID.FireWeapon);
            }
            else
            {
                _model.SetActiveAbility(AbilityID.SkipTurn);
            }

            //pretend like the CPU is thinking :)
            yield return new WaitForSeconds(0.5f);

            _model.TransitionState();
        }

        yield return null;
    }

    private IEnumerator Coroutine_ChooseTarget()
    {
        if (_model.IsCurrentTeamCPU())
        {
            CharacterComponent characterComponent = _model.GetCurrentCharacter();

            List<CharacterComponent> enemies = _model.GetTargetCandidates();

            if (enemies.Count > 0)
            {
                CharacterComponent targetCharacter = enemies[UnityEngine.Random.Range(0, enemies.Count)];
                _model.SetTarget(targetCharacter);
            }

            yield return new WaitForSeconds(0.5f);

            _model.TransitionState();
        }

        yield return null;
    }

    //Abilities
    public IEnumerator PerformAbility(CharacterComponent caster)
    {
        AbilityID abilityID = _model.GetActiveAbility();

        string casterName = GetDisplayString(caster.GetID());
        string abilityString = GetEventString(abilityID);

        Debug.Log(casterName + " " + abilityString + "...");

        caster.PlayAudio(CharacterAudioType.Confirm);

        //UnfollowCharacter();

        _canvas.ShowEventBanner(casterName + " " + abilityString + "...");
        yield return new WaitForSeconds(0.5f);

        yield return AbilityHandler.PerformAbility(abilityID, _model.GetCurrentCharacter(), _model.GetActiveTarget(), _model.GetActiveDestination());
    }

    public void OnAbilityCancelled()
    {
        CameraRig.Instance.Follow(_model.GetCurrentCharacter());

        _model.CancelActiveAbility();

        _model.TransitionState();
    }

    public void OnAbilitySelected(AbilityID abilityID)
    {
        _model.SetActiveAbility(abilityID);

        _model.TransitionState();
    }
    
    public void OnEnvironmentDestinationSelected(Vector3 destination, MovementRangeType rangeType)
    {
        Debug.Log("Destination " + destination.ToString() + "selected");

        _model.SetActiveDestination(destination);

        if (rangeType == MovementRangeType.Half)
        {
            _model.SetActiveAbility(AbilityID.MoveHalf);
        }
        else
        {
            _model.SetActiveAbility(AbilityID.MoveFull);
        }

        _model.TransitionState();
    }

    public bool AreTargetsAvailable()
    {
        return _model.AreTargetsAvailable();
    }

    public bool AreAlliesAvailable()
    {
        return _model.AreAlliesAvailable();
    }

    public void SelectTarget(CharacterComponent character)
    {
        if(_model.GetState() == EncounterState.CHOOSE_TARGET)
        {
            foreach (CharacterComponent target in _model.GetAllCharactersInTeam(character.GetTeam()))
            {
                character.HandleEvent(CharacterEvent.UNTARGETED);
            }

            _model.SetTarget(character);

            _model.TransitionState();
        }
    }

    public void OnEnemyHighlighted(CharacterComponent target)
    {
        //untarget everyone first before assigning a new one
        foreach(CharacterComponent character in _model.GetAllCharactersInTeam(target.GetTeam()))
        {
            character.HandleEvent(CharacterEvent.UNTARGETED);
        }

        CharacterComponent caster = _model.GetCurrentCharacter();

        StartCoroutine(AbilityHandler.Coroutine_RotateTowards(caster, target));

        DamageInfo damageInfo = WeaponDefinition.Get(caster.GetWeaponID()).CalculateDamage(caster,target);

        target.HandleEvent(CharacterEvent.TARGETED);
    }

    private IEnumerator SpawnCharacters()
    {
        foreach (CharacterComponent characterComponent in _model.GetAllCharacters())
        {
            yield return characterComponent.Coroutine_Spawn();
        }
    }

    private IEnumerator DespawnCharacters()
    {
        foreach (CharacterComponent characterComponent in _model.GetAllCharacters())
        {
            yield return characterComponent.PerformCleanup();
        }
    }

    public EncounterState GetCurrentState()
    {
        if(_model != null)
        {
            return _model.GetState();
        }

        return EncounterState.DONE;
    }

    public bool IsPlayerTurn()
    {
        return _model.IsPlayerTurn();
    }

    public CharacterComponent GetCurrentCharacter()
    {
        if(IsModelActive())
        {
            return _model.GetCurrentCharacter();
        }

        return null;
    }

    public void RequestEventBanner(string message, float duration)
    {
        _canvas.ShowEventBanner(message, duration);
    }

    public bool ShouldAllowInput()
    {
        return (_model.GetState() == EncounterState.CHOOSE_ACTION && !_model.IsCurrentTeamCPU());
    }

    public bool IsModelActive()
    {
        return _model != null;
    }

    public static bool IsActive()
    {
        return Instance != null;
    }
}
