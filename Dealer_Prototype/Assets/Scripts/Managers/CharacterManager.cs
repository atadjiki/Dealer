using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>, IEventReceiver
{
    [Serializable]
    public struct CharacterData
    {
        public Enumerations.CharacterID ID;
        public GameObject Prefab;
    }

    public CharacterData[] characters;

    public CharacterData GetCharacterData(Enumerations.CharacterID characterID, out bool success)
    {
        foreach(CharacterData characterData in characters)
        {
            if(characterData.ID == characterID)
            {
                success = true;
                return characterData;
            }
        }

        success = false;
        return new CharacterData();
    }

    public void HandleEvent(Enumerations.EventID eventID)
    {
        
    }
}
