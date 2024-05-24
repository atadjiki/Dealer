using System.Collections;
using UnityEditor;
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
    private EncounterStateData _data;

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
        //cache our libraries
        ResourceUtil.LoadLibraries();

        //scan the environment
        EnvironmentUtil.Scan();

        //assign any listeners
        EncounterStateData.OnStateChanged += StateChangeCallback;
        OnAbilityChosen += AbilityChosenCallback;

        //TODO eventually we will tie this to user input
        EncounterModel.Init();
    }

    private void OnDestroy()
    {
        //unassign any listers
        EncounterStateData.OnStateChanged -= StateChangeCallback;
        OnAbilityChosen -= AbilityChosenCallback;
    }

    public void AbilityChosenCallback(AbilityID ability, object data)
    {
        _data.SetActiveAbility(ability);

        switch (ability)
        {
            case AbilityID.MOVE_FULL:
            case AbilityID.MOVE_HALF:
            {
                Vector3 destination = ((Vector3)data);
                _data.SetActiveDestination(destination);
                EncounterUtil.CreateDestinationHighlight(destination);
                EncounterModel.Transition(_data);
                break;
            }
            default:
                break;
        }
    }

    public void StateChangeCallback(EncounterStateData stateData)
    {
        StartCoroutine(Coroutine_StateChangeCallback(stateData));
    }

    private IEnumerator Coroutine_StateChangeCallback(EncounterStateData Data)
    {
        //yield return new WaitForSecondsRealtime(1.0f);

        _data = Data;

        EncounterState state = _data.GetCurrentState();

        Debug.Log("StateMachineCallback " + state);

        switch (state)
        {
            case EncounterState.SETUP_START:
            {
                //spawn our characters 
                foreach (EncounterTeamData teamData in SetupData.Teams)
                {
                    foreach (CharacterID ID in teamData.Characters)
                    {
                        CharacterDefinition characterData = ResourceUtil.GetCharacterData(ID);

                        if (characterData != null)
                        {
                            CharacterComponent character = CharacterUtil.BuildCharacterObject(characterData, this.transform);

                            yield return character.Coroutine_Setup(characterData);

                            Vector3 randomLocation = EnvironmentUtil.GetRandomTile();
                            character.Teleport(randomLocation);

                            //Vector3 destination = EnvironmentUtil.GetClosestTileWithCover(character.GetWorldLocation());
                            //character.Teleport(destination);
                            Debug.Log("Character " + characterData.ID + " spawned at " + randomLocation.ToString());

                            _data.AddCharacter(teamData.Team, character);
                        }
                    }
                }

                //kick off a camera rig if one isnt in the scene
                CameraRig.Launch();
                break;
            }
            case EncounterState.SETUP_COMPLETE:
            {
                //BroadcastCharacterEvent(CharacterEvent.UPDATE);
                break;
            }
            case EncounterState.SELECT_CURRENT_CHARACTER:
            {
                CharacterComponent currentCharacter = _data.GetCurrentCharacter();

                if (currentCharacter.IsAlive())
                {
                    currentCharacter.OnSelected();
                    EnvironmentUtil.Scan();

                    CameraRig.Instance.Follow(currentCharacter);

                    EncounterUtil.CreateCharacterHighlight(currentCharacter);

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

                CharacterComponent currentCharacter = _data.GetCurrentCharacter();

                currentCharacter.OnDeselected();

                break;
            }
            case EncounterState.CHOOSE_ACTION:
            {
                CharacterComponent currentCharacter = _data.GetCurrentCharacter();

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
                CharacterComponent currentCharacter = _data.GetCurrentCharacter();
                yield return PerformAbility(currentCharacter);
                //TODO BroadcastCharacterEvent(CharacterEvent.UPDATE);
                break;
            }
            default:
                break;
        }

        EncounterModel.Transition(_data);

        yield return null;
    }

    public IEnumerator PerformAbility(CharacterComponent caster)
    {
        AbilityID abilityID = _data.GetActiveAbility();

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

    //Helpers
    public bool GetCurrentCharacter(out CharacterComponent character)
    {
        character = _data.GetCurrentCharacter();

        if (character != null)
        {
            return true;
        }
        else
        {
            Debug.LogError("Current character was null!");
            return false;
        }
    }

    public Vector3 GetCurrentCharacterLocation()
    {
        CharacterComponent currentCharacter;
        if (GetCurrentCharacter(out currentCharacter))
        {
            return currentCharacter.GetWorldLocation();
        }

        return Vector3.zero;
    }

    public static bool IsActive()
    {
        return _instance != null;
    }

    //Debug 
    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            CharacterComponent currentCharacter = _data.GetCurrentCharacter();

            if (currentCharacter != null)
            {
                Gizmos.color = Color.green;
                Handles.color = Color.green;
                Handles.Label(currentCharacter.GetWorldLocation(), currentCharacter.GetID().ToString());
            }
        }
    }

    private void OnGUI()
    {
        if (Application.isPlaying)
        {
            int TextWidth = 200;
            GUI.contentColor = Color.green;
            GUI.Label(new Rect(Screen.width - TextWidth, 10, TextWidth, 22), _data.CurrentState.ToString());
        }
    }
}
