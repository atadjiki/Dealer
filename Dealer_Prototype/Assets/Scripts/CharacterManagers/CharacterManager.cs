using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Constants;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private List<CharacterComponent> characters;

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
        characters = new List<CharacterComponent>();
        waitLocations = new List<MarkedLocation>();
        GameStateManager.Instance.onLevelStart += OnLevelStart;
    }

    public bool RegisterCharacter(CharacterComponent characterComponent)
    {
        if (characters.Count == _popCap)
        {
            DebugManager.Instance.Print(DebugManager.Log.LogNPCManager, "Could not register NPC, reached pop cap");
            return false;
        }

        characters.Add(characterComponent);

        DebugManager.Instance.Print(DebugManager.Log.LogNPCManager, "Registered NPC " + characterComponent.GetID());

        if (PartyPanelList.Instance)
        {
            PartyPanelList.Instance.UpdateList();
        }
        if (CharacterPanel.Instance)
        {
            CharacterPanel.Instance.RegisterCharacter(characterComponent);
        }

        return true;
    }

    public void UnRegisterCharacter(CharacterComponent characterComponent)
    {
        characters.Remove(characterComponent);

        if (PartyPanelList.Instance)
        {
            PartyPanelList.Instance.UpdateList();
        }
        if (CharacterPanel.Instance)
        {
            CharacterPanel.Instance.UnRegisterCharacter(characterComponent);
        }
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
    }

    public List<CharacterComponent> GetParty()
    {
        return characters;
    }

    private void FixedUpdate()
    {
        UpdateCharacterTasks();
    }

    public bool IsSpawned(CharacterInfo info)
    {
        foreach(CharacterComponent character in characters)
        {
            if(character.GetCharacterID() == info.ID)
            {
                return true;
            }
        }

        return false;
    }

    public void UpdateCharacterTasks()
    {
        foreach (CharacterComponent character in characters)
        {
            CharacterTask task = character.GetTaskComponent().GetTask();

            //if this character is inactive, make sure they are performing a wait
            if (task.Type == TaskConstants.TaskType.Waiting && task.markedLocation == null)
            {
                if (character.GetAnimationComponent().IsHidden() == false)
                {
                    CharacterAnimationComponent model = character.GetAnimationComponent();

                    if (model != null)
                    {
                        //find an unoccupied wait location
                        foreach (WaitLocation waitLocation in waitLocations)
                        {
                            if (waitLocation.occupied == false)
                            {
                                task.markedLocation = waitLocation;
                                waitLocation.occupied = true;
                                character.GetTaskComponent().SetTask(task);

                                //move to location 
                                character.GetNavigatorComponent().MoveToLocation(waitLocation,false);

                                Debug.Log(character.name + " sent to wait at " + waitLocation.transform.position);
                                break;
                            }

                        }
                    }

                }
            }
        }
    }
}