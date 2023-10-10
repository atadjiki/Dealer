using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using static Constants;

[Serializable]
public struct SpawnMarkerData
{
    public TeamID Team;
    public List<EncounterGridSpawnMarker> Markers;
}

[RequireComponent(typeof(AstarPath))]
public class EncounterGridManager: MonoBehaviour
{
    //singleton
    private static EncounterGridManager _instance;
    public static EncounterGridManager Instance { get { return _instance; } }

    //public
    [SerializeField] private List<SpawnMarkerData> SpawnMarkers;

    //private
    private GridGraph _grid;

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

    private void Build()
    {
        _grid = AstarPath.active.data.gridGraph;
        Debug.Log("Found A* grid graph for " + this.name);
    }

    public CharacterComponent SpawnCharacter(TeamID teamID, CharacterID characterID)
    {
        //see if we have a marker available to spawn them in
        foreach (EncounterGridSpawnMarker marker in GetSpawnMarkers(teamID))
        {
            if (marker.IsOccupied() == false)
            {
                marker.SetOccupied(true);

                GameObject characterObject = CreateCharacterObject(teamID + "_" + characterID, marker);
                CharacterComponent characterComponent = AddComponentByTeam(characterID, characterObject);

                return characterComponent;
            }
        }

        return null;
    }

    public Vector3 GetClosestTile(Vector3 worldPosition)
    {
        if(_grid != null)
        {
            NNInfoInternal nodeInfo = _grid.GetNearest(worldPosition);
            return ((Vector3)nodeInfo.node.position);
        }
        else
        {
            Debug.Log("Could not find node close to " + worldPosition.ToString());
            return worldPosition;
        }
    }

    //helpers
    private List<EncounterGridSpawnMarker> GetSpawnMarkers(TeamID team)
    {
        foreach (SpawnMarkerData data in SpawnMarkers)
        {
            if (data.Team == team)
            {
                return data.Markers;
            }
        }

        return null;
    }

    private GameObject CreateCharacterObject(string name, EncounterGridSpawnMarker spawnMarker)
    {

        //adjust spawn marker to the position of the closest tile
        Vector3 initialPos = spawnMarker.transform.position;
        Vector3 closestPos = GetClosestTile(initialPos);

        Debug.Log("Adjusted spawn marker from " + initialPos.ToString() + " to " + closestPos.ToString());

        GameObject characterObject = new GameObject(name);
        characterObject.transform.parent = this.transform;
        characterObject.transform.localPosition = Vector3.zero;
        characterObject.transform.localRotation = Quaternion.identity;
        characterObject.transform.position = closestPos;
        return characterObject;
    }

    private CharacterComponent AddComponentByTeam(CharacterID characterID, GameObject characterObject)
    {
        TeamID teamID = GetTeamByID(characterID);

        switch (teamID)
        {
            case TeamID.Player:
                PlayerCharacterComponent playerCharacterComponent = characterObject.AddComponent<PlayerCharacterComponent>();
                playerCharacterComponent.SetID(characterID);
                return playerCharacterComponent;
            case TeamID.Enemy:
                EnemyCharacterComponent enemyCharacterComponent = characterObject.AddComponent<EnemyCharacterComponent>();
                enemyCharacterComponent.SetID(characterID);
                return enemyCharacterComponent;
            default:
                return null;
        }
    }
}
