using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Constants;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private Dictionary<CharacterInfo, CharacterTask> CharacterTaskMap;
    private Dictionary<CharacterInfo, CharacterComponent> CharacterModelMap;

    private List<MarkedLocation> waitLocations;
    private const int _popCap = 4;

    private static CharacterManager _instance;

    public static CharacterManager Instance { get { return _instance; } }

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

    internal void Build()
    {
        CharacterTaskMap = new Dictionary<CharacterInfo, CharacterTask>();
        CharacterModelMap = new Dictionary<CharacterInfo, CharacterComponent>();
        waitLocations = new List<MarkedLocation>();
        GameStateManager.Instance.onLevelStart += OnLevelStart;
    }

    private void InitializeCharacters(GameState state)
    {
        CharacterTaskMap = new Dictionary<CharacterInfo, CharacterTask>();

        foreach (CharacterInfo info in state.partyInfo)
        {
            RegisterCharacter(info);
            SpawnPoint.Instance.AttemptSpawn(info);
        }
    }

    public bool RegisterCharacter(CharacterInfo characterInfo)
    {
        if (CharacterTaskMap.Keys.Count == _popCap)
        {
            DebugManager.Instance.Print(DebugManager.Log.LogNPCManager, "Could not register NPC, reached pop cap");
            return false;
        }

        CharacterTaskMap.Add(characterInfo, CharacterTask.Empty());

        DebugManager.Instance.Print(DebugManager.Log.LogNPCManager, "Registered NPC " + characterInfo.ID);

        if (PartyPanelList.Instance)
        {
            PartyPanelList.Instance.UpdateList();
        }

        return true;
    }

    public void UnRegisterCharacter(CharacterInfo Character)
    {
        CharacterTaskMap.Remove(Character);

        if (PartyPanelList.Instance)
        {
            PartyPanelList.Instance.UpdateList();
        }
    }

    public void RegisterCharacterModel(CharacterInfo characterInfo, CharacterComponent characterModel)
    {
        if (CharacterModelMap.ContainsKey(characterInfo) == false)
        {
            CharacterModelMap.Add(characterInfo, characterModel);
        }
    }

    public void UnRegisterCharacterModel(CharacterInfo characterInfo)
    {
        CharacterModelMap.Remove(characterInfo);
    }

    public void RegisterLocation(MarkedLocation location)
    {
        if (location is WaitLocation)
        {
            waitLocations.Add(location);
        }
    }

    public void UnRegisterLocation(MarkedLocation location)
    {
        if (location is WaitLocation && waitLocations.Contains(location))
        {
            waitLocations.Remove(location);
        }
    }

    internal void OnLevelStart()
    {
        Debug.Log(this.name + " - received on level start");
        InitializeCharacters(GameStateManager.Instance.state);
    }

    public void OnStateChanged(GameState state)
    {

    }

    public List<CharacterInfo> GetParty()
    {
        return CharacterTaskMap.Keys.ToList(); ;
    }

    public CharacterTask GetCharacterTask(CharacterInfo characterInfo)
    {
        return CharacterTaskMap[characterInfo];
    }

    private void FixedUpdate()
    {
        UpdateCharacterTasks();
    }

    public void UpdateCharacterTasks()
    {
        foreach (CharacterInfo characterInfo in CharacterTaskMap.Keys.ToArray())
        {
            CharacterTask task = CharacterTaskMap[characterInfo];


            //if this character is inactive, make sure they are performing a wait
            if (task.Type == TaskConstants.TaskType.Waiting && task.markedLocation == null)
            {
                if (CharacterModelMap.ContainsKey(characterInfo))
                {
                    CharacterComponent model = CharacterModelMap[characterInfo];

                    if (model != null)
                    {
                        //find an unoccupied wait location
                        foreach (WaitLocation waitLocation in waitLocations)
                        {
                            if (waitLocation.occupied == false)
                            {
                                task.markedLocation = waitLocation;
                                waitLocation.occupied = true;
                                CharacterTaskMap[characterInfo] = task;

                                StartCoroutine(MoveModelToMarkedLocation(model, waitLocation));


                                Debug.Log(characterInfo.name + " sent to wait at " + waitLocation.transform.position);
                                break;
                            }

                        }
                    }

                }
            }
        }
    }

    public IEnumerator MoveModelToMarkedLocation(CharacterComponent model, MarkedLocation markedLocation)
    {
        model.GetNavigatorComponent().SetCanMove(true);
        model.GetNavigatorComponent().ToggleMovement(true);
        model.GetNavigatorComponent()._AI.maxSpeed = 5;

        model.GetNavigatorComponent().MoveToLocation(markedLocation.transform.position);

        Debug.Log("attempting move");

        yield return new WaitForSeconds(0.2f);

        yield return new WaitUntil(() => model.GetNavigatorComponent()._AI.isStopped);

        Debug.Log("completed move");

        model.transform.position = markedLocation.transform.position;
        model.transform.rotation = markedLocation.transform.rotation;
        model.FadeToAnimation(markedLocation.LocationAnim, 0.0f, true);

        yield return null;
    }

}