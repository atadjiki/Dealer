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
    [SerializeField] private GameObject Prefab_EncounterCameraRig;
    [SerializeField] private GameObject Prefab_EncounterCanvas;
    [SerializeField] private GameObject Prefab_EncounterAudioManager;

    private static EncounterManager _instance;

    public static EncounterManager Instance { get { return _instance; } }

    private EncounterModel _model;

    private List<EncounterEventReceiver> _eventReceivers;

    private EncounterCanvasManager _canvas;
    private EncounterCameraRig _cameraRig;
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
        _eventReceivers = new List<EncounterEventReceiver>();

        //create a canvas 
        GameObject canvasObject = Instantiate(Prefab_EncounterCanvas, this.transform);
        yield return new WaitWhile(() => canvasObject.GetComponent<EncounterCanvasManager>() == null);
        _canvas = canvasObject.GetComponent<EncounterCanvasManager>();
        _eventReceivers.Add(_canvas);

        //create a camera rig
        GameObject cameraRigObject = Instantiate(Prefab_EncounterCameraRig, this.transform);
        yield return new WaitWhile(() => cameraRigObject.GetComponent<EncounterCameraRig>() == null);
        _cameraRig = cameraRigObject.GetComponent<EncounterCameraRig>();
        _eventReceivers.Add(_cameraRig);

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

        switch (state)
        {
            case EncounterState.SELECT_CURRENT_CHARACTER:
            {
                CharacterComponent character = _model.GetCurrentCharacter();
                if (character.IsAlive())
                {
                    character.CreateDecal();
                }
                break;
            }
            case EncounterState.TEAM_UPDATED:
            {
                yield return new WaitForSeconds(1.5f);
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
                character.DestroyDecal();
                break;
            }
            case EncounterState.PERFORM_ACTION:
            {
                CharacterComponent currentCharacter = _model.GetCurrentCharacter();
                yield return PerformAbility(currentCharacter);
                break;
            }
            default:
            {
                break;
            }
        }

        if (_model != null && state != EncounterState.DONE)
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
                if(currentCharacter.GetRemainingAmmo() == 0)
                {
                    _model.SetActiveAbility(AbilityID.Reload);
                }
                else
                {
                    _model.SetActiveAbility(AbilityID.Attack);
                }
            }
            else
            {
                _model.SetActiveAbility(AbilityID.SkipTurn);
            }

            //pretend like the CPU is thinking :)
            yield return new WaitForSeconds(1.0f);

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

            yield return new WaitForSeconds(1.0f);

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

        _canvas.ShowEventBanner(casterName + " " + abilityString);
        yield return new WaitForSeconds(0.5f);

        switch (abilityID)
        {
            case AbilityID.Attack:
                yield return HandleAbility_Attack(caster);
                break;
            case AbilityID.Reload:
                yield return HandleAbility_Reload(caster);
                break;
            case AbilityID.SkipTurn:
                yield return HandleAbility_SkipTurn(caster);
                break;
            default:
                break;
        }
    }

    private IEnumerator HandleAbility_Attack(CharacterComponent caster)
    {
        CharacterComponent target = _model.GetActiveTarget();
        yield return new WaitForSeconds(0.25f);

        if (target != null)
        {
            UnfollowCharacter();
            yield return target.Coroutine_RotateTowards(caster);

            CharacterDefinition characterDef = CharacterDefinition.Get(caster.GetID());
            WeaponDefinition weaponDef = WeaponDefinition.Get(caster.GetWeaponID());

            target.ToggleHighlight(false);
            //calculate damage
            bool crit = characterDef.RollCritChance();
            DamageInfo damageInfo = weaponDef.CalculateDamage(crit);

            if(crit)
            {
                yield return new WaitForSeconds(0.25f);
                _canvas.ShowEventBanner("Critical Hit!", 1.0f);
                yield return new WaitForSeconds(0.5f);
            }

            yield return caster.Coroutine_FireWeaponAt(target);
            yield return target.Coroutine_HandleDamage(damageInfo);

            _canvas.ShowEventBanner(damageInfo.ActualDamage + " Damage!", 1.0f);
        }
        else
        {
            Debug.Log("target is null!");
        }

        UnfollowCharacter();
        yield return new WaitForSeconds(1f);

        yield return null;
    }

    private IEnumerator HandleAbility_Reload(CharacterComponent caster)
    {
        FollowCharacter(caster);

        yield return caster.Coroutine_PerformReload();

        UnfollowCharacter();
    }

    private IEnumerator HandleAbility_SkipTurn(CharacterComponent caster)
    {
        yield return new WaitForSeconds(1.0f);
    }

    //helpers
    public void FollowCharacter(CharacterComponent characterComponent)
    {
        _cameraRig.GoToCharacter(characterComponent);
    }

    public void UnfollowCharacter()
    {
        _cameraRig.GoToMainCamera();
    }

    public void OnAbilityCancelled()
    {
        FollowCharacter(_model.GetCurrentCharacter());

        _model.CancelActiveAbility();

        _model.TransitionState();
    }

    public void OnAbilitySelected(AbilityID abilityID)
    {
        _model.SetActiveAbility(abilityID);

        _model.TransitionState();
    }

    public void SelectTarget(CharacterComponent character)
    {
        if(_model.GetState() == EncounterState.CHOOSE_TARGET)
        {
            _model.SetTarget(character);

            _model.TransitionState();
        }
    }

    public void OnEnemyHighlighted(CharacterComponent target)
    {
        CharacterComponent caster = _model.GetCurrentCharacter();

        StartCoroutine(caster.Coroutine_RotateTowards(target));
    }

    private IEnumerator SpawnCharacters()
    {
        foreach (CharacterComponent characterComponent in _model.GetAllCharacters())
        {
            yield return characterComponent.SpawnCharacter();
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
}
