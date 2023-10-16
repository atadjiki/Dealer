using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using static Constants;

[RequireComponent(typeof(AstarPath))]
public class EnvironmentManager: MonoBehaviour
{
    //prefabs
    [SerializeField] private GameObject TilePrefab;

    //event handling
    public delegate void EnvironmentGeneratedDelegate();
    public EnvironmentGeneratedDelegate OnEnvironmentReady;

    //singleton
    private static EnvironmentManager _instance;
    public static EnvironmentManager Instance { get { return _instance; } }

    //private
    private GridGraph _grid;

    //Collections
    private Dictionary<TeamID, List<EnvironmentSpawnMarker>> _spawnMarkers;
    private Dictionary<EnvironmentObstacleType, List<EnvironmentObstacle>> _obstacles;

    private Dictionary<Vector3, EnvironmentTile> _tiles;

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
        _grid = AstarPath.active.data.gridGraph;

        Debug.Log("Found A* grid graph for " + this.name);

        RegisterSpawnMarkers();

        RegisterObstacles();

        GenerateTiles();

    }

    private void RegisterSpawnMarkers()
    {
        _spawnMarkers = new Dictionary<TeamID, List<EnvironmentSpawnMarker>>();

        //register all spawn markers
        foreach (TeamID teamID in Enum.GetValues(typeof(TeamID)))
        {
            if (teamID != TeamID.None)
            {
                _spawnMarkers.Add(teamID, new List<EnvironmentSpawnMarker>());
            }
        }

        foreach (EnvironmentSpawnMarker spawnMarker in GetComponentsInChildren<EnvironmentSpawnMarker>())
        {
            _spawnMarkers[spawnMarker.GetTeam()].Add(spawnMarker);
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
        foreach (EnvironmentSpawnMarker marker in _spawnMarkers[teamID])
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

    private void GenerateTiles()
    {
        if(_grid == null) { return; }

        int index = 0;

        int Columns = _grid.width;

        _tiles = new Dictionary<Vector3, EnvironmentTile>();

        _grid.GetNodes(node =>
        {
            Vector3 pos = ((Vector3)node.position);

            int col = (index % Columns);
            int row = (index / Columns);

            string tilename = "Tile " + (index + 1) + " [ " + row + "," + col + " ] "; ;

            GameObject tileDecal = Instantiate<GameObject>(TilePrefab, pos, Quaternion.identity, this.transform);
            tileDecal.name = tilename;

            _tiles.Add(pos, tileDecal.GetComponent<EnvironmentTile>());

            Debug.Log(tilename);

            index++;

            return true;
        });

        _generated = true;
    }

    private GameObject CreateCharacterObject(string name, EnvironmentMarker spawnMarker)
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

    public bool AreTilesGenerated()
    {
        return _generated;
    }
}
