using System.Collections;
using UnityEngine;
using static Constants;

public class EncounterStateMachine: MonoBehaviour
{
    //event handling
    public delegate void EncounterAbilityDelegate(AbilityID ID, object data);
    public static EncounterAbilityDelegate OnAbilityChosen;

    //who should we spawn, etc
    [SerializeField] private EncounterSetupData SetupData;

    //private vars
    private EncounterModel _model;

    //singleton
    private static EncounterStateMachine _instance;
    public static EncounterStateMachine Instance { get { return _instance; } }

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
        OnAbilityChosen += AbilityChosenCallback;

        EnvironmentUtil.Scan();

        _model = EncounterUtil.CreateEncounterModel();

        EncounterModel.OnStateChanged += StateChangeCallback;

        _model.StartModel(SetupData);
    }

    private void OnDestroy()
    {
        EncounterModel.OnStateChanged -= StateChangeCallback;
        OnAbilityChosen -= AbilityChosenCallback;
    }

    public void AbilityChosenCallback(AbilityID ability, object data)
    {
        _model.SetActiveAbility(ability);

        switch (ability)
        {
            case AbilityID.MOVE_FULL:
            case AbilityID.MOVE_HALF:
            {
                Vector3 destination = ((Vector3)data);
                _model.SetActiveDestination(destination);
                _model.HandleState(EncounterState.CHOOSE_ACTION);
                break;
            }
            default:
                break;
        }
    }

    public void StateChangeCallback(EncounterState state)
    {
        StartCoroutine(Coroutine_StateChangeCallback(state));
    }

    private IEnumerator Coroutine_StateChangeCallback(EncounterState state)
    {
        //yield return new WaitForSecondsRealtime(1.0f);

        switch (state)
        {
            case EncounterState.SETUP_COMPLETE:
            {
                //BroadcastCharacterEvent(CharacterEvent.UPDATE);
                break;
            }
            case EncounterState.SELECT_CURRENT_CHARACTER:
            {
                CharacterComponent currentCharacter;
                _model.GetCurrentCharacter(out currentCharacter);

                currentCharacter.OnSelected();
                EnvironmentUtil.Scan();

                CameraRig.Instance.Follow(currentCharacter);

                if (currentCharacter.IsAlive())
                {
                    //TODO currentCharacter.HandleEvent(CharacterEvent.SELECTED);
                }
                break;
            }
            case EncounterState.TEAM_UPDATED:
            {
                yield return new WaitForSecondsRealtime(1.5f);
                break;
            }
            case EncounterState.DESELECT_CURRENT_CHARACTER:
            {
                CameraRig.Instance.Unfollow();

                CharacterComponent currentCharacter;
                _model.GetCurrentCharacter(out currentCharacter);

                currentCharacter.OnDeselected();

                break;
            }
            case EncounterState.CHOOSE_ACTION:
            {
                CharacterComponent currentCharacter;
                _model.GetCurrentCharacter(out currentCharacter);

                EncounterUtil.CreateTileSelector();
                EncounterUtil.CreatePathDisplay(currentCharacter);
                EncounterUtil.CreateMovementRadius(currentCharacter);
                Debug.Log("Waiting for player input");
                //if it's the player's turn, wait for input
                //if enemy turn, let the ai choose an ability and transition
                yield break;
            }
            case EncounterState.CHOOSE_TARGET:
            {
                //if it's the player's turn, wait for input
                Debug.Log("Waiting for player input");
                //if enemy turn, let the ai choose a target
                yield break;
            }
            case EncounterState.PERFORM_ACTION:
            {
                CharacterComponent currentCharacter;
                _model.GetCurrentCharacter(out currentCharacter);
                yield return PerformAbility(currentCharacter);
                //TODO BroadcastCharacterEvent(CharacterEvent.UPDATE);
                break;
            }
            default:
                break;
        }

        _model.HandleState(state);

        yield return null;
    }

    public IEnumerator PerformAbility(CharacterComponent caster)
    {
        AbilityID abilityID = _model.GetActiveAbility();

        //TODO
        //string casterName = GetDisplayString(caster.GetID());
        //string abilityString = GetEventString(abilityID);

        //Debug.Log(casterName + " " + abilityString + "...");

        //TODO caster.PlayAudio(CharacterAudioType.Confirm);

        //UnfollowCharacter();

        //_canvas.ShowEventBanner(casterName + " " + abilityString + "...");
        //yield return new WaitForSeconds(DEFAULT_WAIT_TIME);

        yield return caster.Coroutine_PerformAbility();
    }

    public static bool IsActive()
    {
        return _instance != null;
    }
}
