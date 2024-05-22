using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//Interface for State Graphs (visual scripting) to use 
public class EncounterUtil : MonoBehaviour
{
    public static GameObject CreatePathDisplay(Vector3 origin)
    {
        Debug.Log("Creating path display");

        EncounterPrefabData prefabData = ResourceUtil.GetEncounterPrefabs();

        GameObject gameObject = Instantiate<GameObject>(prefabData.PathDisplay);
        gameObject.transform.position = origin;

        EnvironmentPathDisplay pathDisplay = gameObject.GetComponent <EnvironmentPathDisplay>();

        return gameObject;
    }

    public static GameObject CreateTileSelector()
    {
        Debug.Log("Creating tile selector");

        EncounterPrefabData prefabData = ResourceUtil.GetEncounterPrefabs();

        return Instantiate<GameObject>(prefabData.TileSelector);
    }

    public static GameObject CreateMovementRadius()
    {
        Debug.Log("Creating movement radius");

        EncounterPrefabData prefabData = ResourceUtil.GetEncounterPrefabs();

        return Instantiate<GameObject>(prefabData.MovementRadius);
    }

    public static GameObject CreateCameraRig()
    {
        EncounterPrefabData prefabData = ResourceUtil.GetEncounterPrefabs();

        return Instantiate<GameObject>(prefabData.CameraRig);
    }
}
