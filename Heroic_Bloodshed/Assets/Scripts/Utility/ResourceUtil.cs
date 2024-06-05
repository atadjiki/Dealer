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
    public static CharacterDefinition GetCharacterData(CharacterID ID)
    {
        foreach (CharacterDefinition data in GetAllCharacterData())
        {
            if (data.ID == ID)
            {
                return data;
            }
        }

        return null;
    }

    //Character Models
    public static GameObject GetModelPrefab(ModelID ID)
    {
        Debug.Log("Loading Character Model " + ID.ToString());

        CharacterModelData data = Resources.Load<CharacterModelData>("Data/CharacterModelData");

        return data.GetModel(ID);
    }

    public static List<CharacterDefinition> GetAllCharacterData()
    {
        Debug.Log("Loading All Character Data ");

        return new List<CharacterDefinition>(Resources.LoadAll<CharacterDefinition>("Data/Character"));
    }

    //Encounter Setup Data
    public static List<CharacterDefinition> GetAllEncounterSetupData()
    {
        Debug.Log("Loading All Encounter Setup Data");
        return new List<CharacterDefinition>(Resources.LoadAll<CharacterDefinition>("Data/EncounterSetup"));
    }
}
