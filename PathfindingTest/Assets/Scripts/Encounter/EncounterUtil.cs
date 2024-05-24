using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//Interface for State Graphs (visual scripting) to use 
public class EncounterUtil : MonoBehaviour
{
    public static GameObject CreateTileSelector()
    {
        Debug.Log("Creating tile selector");

        GameObject gameObject = new GameObject("EnvironmentTileSelector");
        EnvironmentTileSelector tileSelector = gameObject.AddComponent<EnvironmentTileSelector>();

        return gameObject;
    }

    public static GameObject CreatePathDisplay(CharacterComponent character)
    {
        Debug.Log("Creating path display");

        GameObject gameObject = new GameObject("EnvironmentPathDisplay");

        EnvironmentPathDisplay pathDisplay = gameObject.AddComponent<EnvironmentPathDisplay>();
        pathDisplay.Setup(character);

        return gameObject;
    }

    public static GameObject CreateMovementRadius(CharacterComponent character)
    {
        Debug.Log("Creating movement radius");

        GameObject gameObject = new GameObject("EnvironmentMovementRadius");

        EnvironmentMovementRadius movementRadius = gameObject.AddComponent<EnvironmentMovementRadius>();
        movementRadius.Setup(character);

        return gameObject;
    }

    public static EncounterModel CreateEncounterModel()
    {
        GameObject gameObject = new GameObject("Encounter Model");
        return gameObject.AddComponent<EncounterModel>();
    }
}
