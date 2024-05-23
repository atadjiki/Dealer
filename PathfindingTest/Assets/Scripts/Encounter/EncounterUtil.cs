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

        EncounterPrefabData prefabData = ResourceUtil.GetEncounterPrefabs();

        return Instantiate<GameObject>(prefabData.TileSelector);
    }

    public static GameObject CreatePathDisplay(CharacterComponent character)
    {
        Debug.Log("Creating path display");

        EncounterPrefabData prefabData = ResourceUtil.GetEncounterPrefabs();

        GameObject gameObject = Instantiate<GameObject>(prefabData.PathDisplay);

        EnvironmentPathDisplay pathDisplay = gameObject.GetComponent<EnvironmentPathDisplay>();
        pathDisplay.Setup(character);

        return gameObject;
    }

    public static GameObject CreateMovementRadius(CharacterComponent character)
    {
        Debug.Log("Creating movement radius");

        EncounterPrefabData prefabData = ResourceUtil.GetEncounterPrefabs();

        GameObject gameObject = Instantiate<GameObject>(prefabData.MovementRadius);

        EnvironmentMovementRadius movementRadius = gameObject.GetComponent<EnvironmentMovementRadius>();
        movementRadius.Setup(character);

        return gameObject;
    }

    public static GameObject CreateCameraRig()
    {
        EncounterPrefabData prefabData = ResourceUtil.GetEncounterPrefabs();

        return Instantiate<GameObject>(prefabData.CameraRig);
    }

    public static EncounterModel CreateEncounterModel()
    {
        GameObject gameObject = new GameObject("Encounter Model");
        return gameObject.AddComponent<EncounterModel>();
    }
}
