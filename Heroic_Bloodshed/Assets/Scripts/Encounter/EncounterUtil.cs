using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//Interface for State Graphs (visual scripting) to use 
public class EncounterUtil : MonoBehaviour
{
    public static GameObject CreateCharacterHighlight(CharacterComponent character)
    {
        Debug.Log("Creating character highlight");

        GameObject gameObject = Instantiate<GameObject>(ResourceUtil.GetCharacterHighlight());

        gameObject.transform.parent = character.GetModel().transform;
        gameObject.transform.localPosition = Vector3.zero;

        return gameObject;

    }
    public static GameObject CreateDestinationHighlight(Vector3 destination)
    {
        Debug.Log("Creating destination highlight");

        GameObject gameObject = Instantiate<GameObject>(ResourceUtil.GetDestinationHighlight());
        gameObject.transform.position = destination;

        return gameObject;
    }

    public static GameObject CreateTileSelector()
    {
        Debug.Log("Creating tile selector");

        return Instantiate<GameObject>(ResourceUtil.GetTileSelector());
    }

    public static GameObject CreatePathDisplay(CharacterComponent character)
    {
        Debug.Log("Creating character path renderer");

        GameObject gameObject = Instantiate<GameObject>(ResourceUtil.GetCharacterPathRenderer());

        CharacterPathRenderer pathRenderer = gameObject.GetComponent<CharacterPathRenderer>();
        pathRenderer.Setup(character);

        return gameObject;
    }

    public static GameObject CreateMovementRadius(CharacterComponent character)
    {
        Debug.Log("Creating movement radius");

        GameObject gameObject = Instantiate<GameObject>(ResourceUtil.GetMovementRadius());

        EnvironmentMovementRadius movementRadius = gameObject.GetComponent<EnvironmentMovementRadius>();
        movementRadius.Setup(character);

        return gameObject;
    }

    public static EncounterModel CreateEncounterModel()
    {
        GameObject gameObject = new GameObject("Encounter Model");
        return gameObject.AddComponent<EncounterModel>();
    }
}
