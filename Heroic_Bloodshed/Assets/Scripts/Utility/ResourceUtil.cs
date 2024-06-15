using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class ResourceUtil : MonoBehaviour
{
    //Prefabs
    public static GameObject GetEncounterCameraRig()
    {
        return Resources.Load<GameObject>("Prefabs/Encounter/EncounterCameraRig");
    }

    public static GameObject GetCharacterHighlight()
    {
        return Resources.Load<GameObject>("Prefabs/Encounter/CharacterHighlight");
    }

    public static GameObject GetDestinationHighlight()
    {
        return Resources.Load<GameObject>("Prefabs/Encounter/DestinationHighlight");
    }

    public static GameObject GetTileSelector()
    {
        return Resources.Load<GameObject>("Prefabs/Encounter/TileSelector");
    }

    public static GameObject GetCharacterPathRenderer()
    {
        return Resources.Load<GameObject>("Prefabs/Encounter/CharacterPathRenderer");
    }

    public static GameObject GetMovementRadius()
    {
        return Resources.Load<GameObject>("Prefabs/Encounter/MovementRadius");
    }

    //Character Data
    public static CharacterDefinition GetCharacterDefinition(CharacterID ID)
    {
        foreach (CharacterDefinition data in GetAllCharacterDefinition())
        {
            if (data.ID == ID)
            {
                return data;
            }
        }

        return null;
    }

    public static List<CharacterDefinition> GetAllCharacterDefinition()
    {
        return new List<CharacterDefinition>(Resources.LoadAll<CharacterDefinition>("Data/Character"));
    }

    //Encounter Setup Data
    public static List<CharacterDefinition> GetAllEncounterSetupData()
    {
        return new List<CharacterDefinition>(Resources.LoadAll<CharacterDefinition>("Data/EncounterSetup"));
    }
}
