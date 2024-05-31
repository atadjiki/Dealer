using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class ResourceUtil : MonoBehaviour
{
    public static void LoadLibraries()
    {
        AudioLibrary.Load();
        ColorLibrary.Load();
        HighlightProfileLibrary.Load();
        MaterialLibrary.Load();
        PrefabLibrary.Load();
        SpriteLibrary.Get();
    }

    public static List<CharacterDefinition> GetAllCharacterData()
    {
        return new List<CharacterDefinition>(Resources.LoadAll<CharacterDefinition>("Data/Character"));
    }

    public static CharacterDefinition GetCharacterData(CharacterID ID)
    {
        foreach(CharacterDefinition data in GetAllCharacterData())
        {
            if(data.ID == ID)
            {
                return data;
            }
        }

        return null;
    }

    public static GameObject GetModel(ModelID ID)
    {
        CharacterModelData data = Resources.Load<CharacterModelData>("Data/CharacterModelData");

        return data.GetModel(ID);
    }

    public static List<CharacterDefinition> GetAllEncounterSetupData()
    {
        return new List<CharacterDefinition>(Resources.LoadAll<CharacterDefinition>("Data/EncounterSetup"));
    }
}
