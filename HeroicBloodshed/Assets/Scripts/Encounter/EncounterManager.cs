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

    private void EncounterStateCallback(EncounterModel model)
    {
        StartCoroutine(Coroutine_EncounterStateCallback(model));
    }

    private IEnumerator Coroutine_LoadEncounter()
    {
        //create a canvas 
        GameObject canvasObject = Instantiate(Prefab_EncounterCanvas, null);
        yield return new WaitWhile(() => canvasObject.GetComponent<EncounterCanvas>() == null);
        _canvas = canvasObject.GetComponent<EncounterCanvas>();

        //create a camera rig
        GameObject cameraRigObject = Instantiate(Prefab_EncounterCameraRig, null);
        yield return new WaitWhile(() => cameraRigObject.GetComponent<EncounterCameraRig>() == null);
        _cameraRig = cameraRigObject.GetComponent<EncounterCameraRig>();
        _cameraRig.Setup(_setupData.CameraFollowTarget);

        //generate encounter and attach to handler
        _model = _setupData.gameObject.AddComponent<EncounterModel>();
        yield return new WaitWhile(() => _setupData.gameObject.GetComponent<EncounterModel>() == null);
        _model.SetSetupData(_setupData);

        _model.OnStateChanged += this.EncounterStateCallback;

        yield return _canvas.HandleInit();
        yield return _model.HandleInit();

        _model.TransitionState();

        yield return null;
    }

    public void SelectAbility(AbilityID abilityID)
    {
        Debug.Log("ability selected " + abilityID);

        _model.OnAbilitySelected(abilityID);

        _model.TransitionState();
    }

    private IEnumerator Coroutine_WaitForPlayerInput()
    {
        Debug.Log("Waiting for player input");

        yield return null;
    }

    private IEnumerator Coroutine_EncounterStateCallback(EncounterModel model)
    {
        EncounterState state = model.GetState();

        _canvas.UpdateCanvas(model);

        switch (state)
        {
            case EncounterState.BUILD_QUEUES:
            {
                _cameraRig.GoToMainCamera();
                yield return new WaitForSeconds(1.0f);
                break;
            }
            case EncounterState.PERFORM_ACTION:
            {
                CharacterComponent currentCharacter = _model.GetCurrentCharacter();
                yield return CharacterAbility.Perform(currentCharacter);
                break;
            }
            case EncounterState.SELECT_CURRENT_CHARACTER:
            {
                CharacterComponent character = model.GetCurrentCharacter();
                if(character.IsAlive())
                {
                    _cameraRig.GoToCharacter(character);
                    character.CreateDecal();
                }

                break;
            }
            case EncounterState.WAIT_FOR_PLAYER_INPUT:
            {
                yield return Coroutine_WaitForPlayerInput();
                yield break;
            }
            case EncounterState.CHOOSE_AI_ACTION:
            {
                yield return new WaitForSeconds(1.0f);
                break;
            }
            case EncounterState.DESELECT_CURRENT_CHARACTER:
            {
                CharacterComponent character = model.GetCurrentCharacter();
                character.DestroyDecal();
                break;
            }
            case EncounterState.DONE:
            {
                _cameraRig.GoToMainCamera();
                break;
            }
            default:
            {
                break;
            }
        }

        yield return new WaitForSeconds(0.2f);

        model.TransitionState();

        if(state == EncounterState.DONE)
        {
            CleanUpCurrentEncounter();
        }

        yield return null;
    }

    private void CleanUpCurrentEncounter()
    {
        if(_model != null)
        {
            _model.OnStateChanged -= this.EncounterStateCallback;

            Destroy(_model.gameObject);
            _model = null;
        }
    }

    public void FollowCharacter(CharacterComponent characterComponent)
    {
        if (_model.GetState() == EncounterState.WAIT_FOR_PLAYER_INPUT)
        {
            _cameraRig.GoToCharacter(characterComponent);
        }
    }

    public void UnfollowCharacter()
    {
        if (_model.GetState() == EncounterState.WAIT_FOR_PLAYER_INPUT)
        {
            FollowCharacter(_model.GetCurrentCharacter());
        }
    }

    public IEnumerator SpawnCharacters(EncounterModel model)
    {
        foreach (CharacterComponent characterComponent in model.GetAllCharacters())
        {
            yield return characterComponent.SpawnCharacter();
        }
    }

    public void OnCharacterSpawned(CharacterComponent characterComponent)
    {
        if (_cameraRig != null)
        {
            _cameraRig.RegisterCharacterCamera(characterComponent);
        }
    }

    public IEnumerator DespawnCharacters(EncounterModel model)
    {
        foreach (CharacterComponent characterComponent in model.GetAllCharacters())
        {
            yield return characterComponent.PerformCleanup();
        }
    }

    public void OnCharacterDespawned(CharacterComponent characterComponent)
    {
        if (_cameraRig != null)
        {
            _cameraRig.UnregisterCharacterCamera(characterComponent);
        }
    }

    //helpers
    public CharacterComponent FindTargetForCharacter(CharacterComponent characterComponent)
    {
        //get a list of this character's enemies in the encounter
        TeamID opposingTeam = GetOpposingTeam(characterComponent);
        List<CharacterComponent> enemies =_model.GetAllCharactersInTeam(opposingTeam);

        if(enemies.Count > 0)
        {
            List<CharacterComponent> targets = new List<CharacterComponent>();

            foreach(CharacterComponent enemy in enemies)
            {
                if(enemy.IsAlive())
                {
                    targets.Add(enemy);
                }
            }

            CharacterComponent targetCharacter = targets[UnityEngine.Random.Range(0, targets.Count - 1)];
            Debug.Log("Found target: " + targetCharacter.gameObject.name);
            characterComponent.SetTarget(targetCharacter);
            return targetCharacter;
        }

        return null;
    }
}
