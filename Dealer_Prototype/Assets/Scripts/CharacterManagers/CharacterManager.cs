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

        characterComponent.ToWaitingForUpdate();
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
                character.timeSinceLastUpdate += Time.fixedDeltaTime;
                //  Debug.Log("character tick " + character.timeSinceLastUpdate + "/" + character.updateTime);

                //if it is time for an update:
                if (character.timeSinceLastUpdate >= (character.updateTime))
                {
                    character.timeSinceLastUpdate = 0; //reset time

                    //if we are waiting for an update, find a new wait location
                    if (character.GetState() == CharacterConstants.CharacterState.WaitingForUpdate)
                    {
                        EjectCharacterFromWaitLocation(character); //remove the character from any locations

                        List<WaitLocation> avaialbleWaitLocations = GetWaitLocationsForCharacter(character); //get locations to place them

                        if (avaialbleWaitLocations.Count > 0)
                        {
                            WaitLocation waitLocation = avaialbleWaitLocations[Random.Range(0, avaialbleWaitLocations.Count)];

                            waitLocation.SetOccupant(character);

                            character.GetNavigatorComponent().onReachedLocation += OnReachedLocation; //callback for when we get there

                            //move to location 
                            character.GetAnimationComponent().ToggleVisiblity(true); //unhide model 
                            character.GetNavigatorComponent().MoveToLocation(waitLocation, true); //start moving

                            character.ToMoving(); //change state
                        }
                        else
                        {
                            character.ToWaiting(); //if there are no locations for us, go back to waiting
                        }
                    }
                    //if they're already waiting but its time for an update, kick them off their wait location
                    else if (character.GetState() == CharacterConstants.CharacterState.Waiting)
                    {
                        character.ToWaitingForUpdate();
                        character.timeSinceLastUpdate = character.updateTime;
                      //  character.updateTime = 0; //force an update next time
                    }

                }
            }
        }
    }

    public void OnReachedLocation(CharacterComponent character, MarkedLocation location)
    {
        if (location is WaitLocation && character.GetState() == CharacterConstants.CharacterState.Moving)
        {
            character.ToWaiting();
            character.updateTime = ((WaitLocation)location).GetWaitTime();

         //   Debug.Log("next update in " + character.updateTime);
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
