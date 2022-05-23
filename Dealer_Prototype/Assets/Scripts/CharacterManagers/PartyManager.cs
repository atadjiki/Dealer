using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Constants;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    private Dictionary<CharacterInfo, CharacterTask> CharacterMap;

    private List<MarkedLocation> waitLocations;
    private const int _popCap = 4;

    private static PartyManager _instance;

    public static PartyManager Instance { get { return _instance; } }

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
        GameStateManager.Instance.onLevelStart += OnLevelStart;
    }

    private void InitializeCharacters(GameState state)
    {
        CharacterMap = new Dictionary<CharacterInfo, CharacterTask>();

        foreach (CharacterInfo info in state.partyInfo)
        {
            RegisterCharacter(info);
            SpawnPoint.Instance.AttemptSpawn(info);
        }
    }

    public bool RegisterCharacter(CharacterInfo characterInfo)
    {
        if (CharacterMap.Keys.Count == _popCap)
        {
            DebugManager.Instance.Print(DebugManager.Log.LogNPCManager, "Could not register NPC, reached pop cap");
            return false;
        }

        CharacterMap.Add(characterInfo, CharacterTask.Empty());

        DebugManager.Instance.Print(DebugManager.Log.LogNPCManager, "Registered NPC " + characterInfo.ID);

        if (PartyPanelList.Instance)
        {
            PartyPanelList.Instance.UpdateList();
        }

        return true;
    }

    public void UnRegisterCharacter(CharacterInfo Character)
    {
        CharacterMap.Remove(Character);

        if (PartyPanelList.Instance)
        {
            PartyPanelList.Instance.UpdateList();
        }
    }

    public void RegisterLocation(MarkedLocation location)
    {
        if(location is WaitLocation)
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
        return CharacterMap.Keys.ToList(); ;
    }

    public CharacterTask GetCharacterTask(CharacterInfo characterInfo)
    {
        return CharacterMap[characterInfo];
    }

    public void UpdateCharacterTasks()
    {
        foreach(CharacterInfo characterInfo in CharacterMap.Keys.ToArray())
        {
            CharacterTask task = CharacterMap[characterInfo];

            //if this character is inactive, make sure they are performing a wait
            if(task.State == TaskConstants.TaskState.InActive)
            {
                //find an unoccupied wait location
                foreach(WaitLocation waitLocation in waitLocations)
                {
                    if(waitLocation.occupied == false)
                    {
                        task.markedLocation = waitLocation;
                        waitLocation.occupied = true;
                    }
                }
            }
        }
    }

}
