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
    }

    public bool RegisterCharacter(CharacterComponent characterComponent)
    {
        if (characters.Count == _popCap)
        {
            DebugManager.Instance.Print(DebugManager.Log.LogNPCManager, "Could not register NPC, reached pop cap");
            return false;
        }

        characterComponent.SetState(CharacterConstants.CharacterState.WaitingForUpdate);
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

    private void FixedUpdate()
    {
        UpdateCharacterTasks();
    }

    public bool IsSpawned(CharacterInfo info)
    {
        foreach (CharacterComponent character in characters)
        {
            if (character.GetCharacterID() == info.ID)
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
            if (character.GetState() == CharacterConstants.CharacterState.Moving)
            {
                //no update here
            }
            else
            {
                //if it is time for an update:
                if ((character.timeSinceLastUpdate + Time.fixedDeltaTime) >= character.updateTime)
                {
                    //if we are waiting for an update, find a new wait location
                    if (character.GetState() == CharacterConstants.CharacterState.WaitingForUpdate)
                    {
                        List<WaitLocation> avaialbleWaitLocations = GetWaitLocationsForCharacter(character);

                        if (avaialbleWaitLocations.Count > 0)
                        {
                            WaitLocation waitLocation = avaialbleWaitLocations[Random.Range(0, avaialbleWaitLocations.Count)];

                            character.timeSinceLastUpdate = 0;

                            waitLocation.SetOccupant(character);

                            character.GetNavigatorComponent().onReachedLocation += OnReachedLocation;

                            //move to location 
                            character.GetAnimationComponent().ToggleVisiblity(true);
                            character.GetNavigatorComponent().MoveToLocation(waitLocation, true);

                            character.SetState(CharacterConstants.CharacterState.Moving);
                        }
                        else
                        {
                            ResetWaitLocationOccupancies(character);
                        }
                    }
                    //if they're already waiting but its time for an update, kick them off their wait location
                    else if (character.GetState() == CharacterConstants.CharacterState.Waiting)
                    {
                        character.updateTime = 0; //force an update next time

                        EjectCharacterFromWaitLocation(character);
                        character.SetState(CharacterConstants.CharacterState.WaitingForUpdate);
                    }

                }
                //increment the tick
                else
                {
                    character.timeSinceLastUpdate += Time.fixedDeltaTime;
                  //  Debug.Log("character tick " + character.timeSinceLastUpdate + "/" + character.updateTime);
                }

            }
        }
    }

    public void OnReachedLocation(CharacterComponent character, MarkedLocation location)
    {
        if (location is WaitLocation && character.GetState() == CharacterConstants.CharacterState.Moving)
        {
            WaitLocation waitLocation = (WaitLocation)location;

            character.SetState(CharacterConstants.CharacterState.Waiting);
            character.timeSinceLastUpdate = 0;
            character.updateTime = Random.Range(waitLocation.minWaitTime, waitLocation.maxWaitTime);

            Debug.Log("next update in " + character.updateTime);
        }
        else
        {
            Debug.Log("error: character state is waiting on reached location");
        }
    }

    public void EjectCharacterFromWaitLocation(CharacterComponent character)
    {
        foreach (WaitLocation waitLocation in waitLocations)
        {
            if (waitLocation.GetCurrentOccupant() == character)
            {
                waitLocation.SetOccupant(null);
                return;
            }
        }
    }

    public List<WaitLocation> GetWaitLocationsForCharacter(CharacterComponent character)
    {
        List<WaitLocation> unoccupied = new List<WaitLocation>();

        foreach (WaitLocation waitLocation in waitLocations)
        {
            if (waitLocation.GetCurrentOccupant() == null && waitLocation.GetPreviousOccupant() != character)
            {
                unoccupied.Add(waitLocation);
            }
        }

        return unoccupied;
    }

    public void ResetWaitLocationOccupancies(CharacterComponent character)
    {
        foreach(WaitLocation waitLocation in waitLocations)
        {
            if (waitLocation.GetCurrentOccupant() != null &&  waitLocation.GetPreviousOccupant() == character)
            {
                waitLocation.Reset();
            }
        }
    }

    public List<CharacterComponent> GetParty() { return characters; }
}
