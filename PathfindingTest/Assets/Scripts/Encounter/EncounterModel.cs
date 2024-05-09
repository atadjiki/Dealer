using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class EncounterModel : MonoBehaviour
{
    //event handling
    public delegate void EncounterStateDelegate();
    public EncounterStateDelegate OnStateChanged;

    //who should we spawn, etc
    [SerializeField] private EncounterSetupData SetupData;

    //what characters are we allowed to spawn?
    [SerializeField] private List<CharacterData> Characters;

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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Coroutine_Init());
    }

    public void TransitionState()
    {
        if (!_busy)
        {
            StartCoroutine(Coroutine_TransitionState());
        }
    }

    private IEnumerator Coroutine_TransitionState()
    {
        _busy = true;

        Debug.Log("Handling state " + _pendingState);

        switch (_pendingState)
        {
            case EncounterState.INIT:
                yield return Coroutine_Init();
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
        foreach (CharacterData data in Characters)
        {
            if (SetupData.ToSpawn == data.ID)
            {
                CharacterComponent character = CharacterUtil.BuildCharacterObject(data, this.transform);

                Vector3 spawnLocation = EncounterUtil.GetRandomTile();

                character.transform.position = spawnLocation;

                character.Setup(data);
            }
        }

        yield return null;
    }

    //state
    public EncounterState GetState() { return _pendingState; }

    private void SetPendingState(EncounterState state)
    {
        _pendingState = state;
    }
}
