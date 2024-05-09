using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class ResourceUtil : MonoBehaviour
{
    public static List<CharacterData> GetAllCharacterData()
    {
        return new List<CharacterData>(Resources.LoadAll<CharacterData>("Data/Character"));
    }

    public static CharacterData GetCharacterData(CharacterID ID)
    {
        foreach(CharacterData data in GetAllCharacterData())
        {
            if(data.ID == ID)
            {
                return data;
            }
        }

        return null;
    }

    public static List<CharacterData> GetAllEncounterSetupData()
    {
        return new List<CharacterData>(Resources.LoadAll<CharacterData>("Data/EncounterSetup"));
    }
}
