using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using static Constants;

[RequireComponent(typeof(AstarPath))]
public class EnvironmentManager: MonoBehaviour
{
    //event handling
    public delegate void EnvironmentGeneratedDelegate();
    public EnvironmentGeneratedDelegate OnEnvironmentReady;

    //singleton
    private static EnvironmentManager _instance;
    public static EnvironmentManager Instance { get { return _instance; } }

    //private
    private GridGraph _gridGraph;
    private EnvironmentTileGrid _tileGrid;

    //Collections
    private Dictionary<TeamID, List<EnvironmentSpawnPoint>> _spawnPoints;
    private Dictionary<EnvironmentObstacleType, List<EnvironmentObstacle>> _obstacles;

    private bool _generated = false;

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
        _gridGraph = AstarPath.active.data.gridGraph;

        _tileGrid = GetComponentInChildren<EnvironmentTileGrid>();

        Debug.Log("Found A* grid graph for " + this.name);

        RegisterSpawnMarkers();

        RegisterObstacles();

        if(_tileGrid != null)
        {
            _tileGrid.OnTilesGenerated += OnTilesGenerated;
            _tileGrid.GenerateTiles();
        }
    }

    public void OnTilesGenerated()
    {
        _generated = true;
    }

    public bool AreTilesGenerated()
    {
        return _generated;
    }

    private void RegisterSpawnMarkers()
    {
        _spawnPoints = new Dictionary<TeamID, List<EnvironmentSpawnPoint>>();

        //register all spawn markers
        foreach (TeamID teamID in Enum.GetValues(typeof(TeamID)))
        {
            if (teamID != TeamID.None)
            {
                _spawnPoints.Add(teamID, new List<EnvironmentSpawnPoint>());
            }
        }

        foreach (EnvironmentSpawnPoint spawnPoint in GetComponentsInChildren<EnvironmentSpawnPoint>())
        {
            _spawnPoints[spawnPoint.GetTeam()].Add(spawnPoint);
        }
    }

    private void RegisterObstacles()
    {
        _obstacles = new Dictionary<EnvironmentObstacleType, List<EnvironmentObstacle>>();

        foreach(EnvironmentObstacleType obstacleType in Enum.GetValues(typeof(EnvironmentObstacleType)))
        {
            _obstacles.Add(obstacleType, new List<EnvironmentObstacle>());
        }

        foreach(EnvironmentObstacle obstacle in GetComponentsInChildren<EnvironmentObstacle>())
        {
            _obstacles[obstacle.GetObstacleType()].Add(obstacle);
        }
    }

    public CharacterComponent SpawnCharacter(TeamID teamID, CharacterID characterID)
    {
        //see if we have a marker available to spawn them in
        foreach (EnvironmentSpawnPoint spawnPoint in _spawnPoints[teamID])
        {
            GameObject characterObject = CreateCharacterObject(teamID + "_" + characterID, spawnPoint);
            CharacterComponent characterComponent = AddComponentByTeam(characterID, characterObject);

            return characterComponent;
        }

        return null;
    }

    private GameObject CreateCharacterObject(string name, EnvironmentSpawnPoint spawnPoint)
    {
        //adjust spawn marker to the position of the closest tile
        Vector3 initialPos = spawnPoint.GetSpawnLocation();
        Vector3 closestPos = _tileGrid.GetClosestTilePosition(initialPos);

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
