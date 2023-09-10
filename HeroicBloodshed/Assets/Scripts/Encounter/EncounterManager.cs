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

    private static EncounterManager _instance;

    public static EncounterManager Instance { get { return _instance; } }

    private EncounterModel _model;

    private List<EncounterEventReceiver> _eventReceivers;

    private EncounterCanvas _canvas;
    private EncounterCameraRig _cameraRig;

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
        GameObject canvasObject = Instantiate(Prefab_EncounterCanvas, null);
        yield return new WaitWhile(() => canvasObject.GetComponent<EncounterCanvas>() == null);
        _canvas = canvasObject.GetComponent<EncounterCanvas>();
        _eventReceivers.Add(_canvas);

        //create a camera rig
        GameObject cameraRigObject = Instantiate(Prefab_EncounterCameraRig, null);
        yield return new WaitWhile(() => cameraRigObject.GetComponent<EncounterCameraRig>() == null);
        _cameraRig = cameraRigObject.GetComponent<EncounterCameraRig>();
        _eventReceivers.Add(_cameraRig);

        //generate encounter and attach to handler
        _model = _setupData.gameObject.AddComponent<EncounterModel>();
        yield return new WaitWhile(() => _setupData.gameObject.GetComponent<EncounterModel>() == null);

        _model.OnStateChanged += this.EncounterStateCallback;

        yield return _model.Setup(_setupData);

        foreach (EncounterEventReceiver receiver in _eventReceivers)
        {
            yield return receiver.Coroutine_Init(_model);
        }

        yield return SpawnCharacters();

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
            case EncounterState.CHOOSE_ACTION:
                {
                    yield return Coroutine_ChooseAction();
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

        yield return new WaitForSeconds(0.25f);

        if (_model != null && state != EncounterState.DONE)
        {
            _model.TransitionState();
        }

        yield return null;
    }

    private IEnumerator Coroutine_ChooseAction()
    {
        TeamID currentTeam = _model.GetCurrentTeam();

        if (_model.IsCurrentTeamCPU())
        {
            if (FindRandomTarget() != null)
            {
                _model.OnAbilitySelected(AbilityID.Attack);
            }
            else
            {
                _model.OnAbilitySelected(AbilityID.SkipTurn);
            }

            //pretend like the CPU is thinking :)
            yield return new WaitForSeconds(1.0f);

            _model.TransitionState();
        }

        yield return null;
    }

    //Abilities
    public IEnumerator PerformAbility(CharacterComponent caster)
    {
        AbilityID abilityID = caster.GetActiveAbility();

        Debug.Log(caster.GetID() + " performing ability " + abilityID.ToString());

        switch (abilityID)
        {
            case AbilityID.Attack:
                yield return HandleAbility_Attack(caster);
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
        CharacterComponent target = FindRandomTarget();

        if (target != null)
        {
            UnfollowCharacter();
            yield return new WaitForSeconds(1.0f);
           // FollowCharacter(target);
          //  yield return new WaitForSeconds(1.0f);
            target.Kill();
            yield return new WaitForSeconds(1.0f);
            UnfollowCharacter();
            yield return new WaitForSeconds(1.0f);
        }
        else
        {
            Debug.Log("target is null!");
        }
        yield return null;
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

    public void SelectAbility(AbilityID abilityID)
    {
        _model.OnAbilitySelected(abilityID);

        _model.TransitionState();
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

    private CharacterComponent FindRandomTarget()
    {
        CharacterComponent characterComponent = _model.GetCurrentCharacter();

        //get a list of this character's enemies in the encounter
        TeamID opposingTeam = GetOpposingTeam(characterComponent);
        List<CharacterComponent> enemies = _model.GetAllCharactersInTeam(opposingTeam);

        if (enemies.Count > 0)
        {
            List<CharacterComponent> targets = new List<CharacterComponent>();

            foreach (CharacterComponent enemy in enemies)
            {
                if (enemy.IsAlive())
                {
                    targets.Add(enemy);
                }
            }

            if (targets.Count > 0)
            {
                CharacterComponent targetCharacter = targets[UnityEngine.Random.Range(0, targets.Count - 1)];
                characterComponent.SetTarget(targetCharacter);
                return targetCharacter;
            }
        }

        return null;
    }
}
