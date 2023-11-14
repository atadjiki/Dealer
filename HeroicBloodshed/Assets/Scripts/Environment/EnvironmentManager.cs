using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using static Constants;

public class EnvironmentManager: MonoBehaviour, IEncounterEventHandler
{
    //singleton
    private static EnvironmentManager _instance;
    public static EnvironmentManager Instance { get { return _instance; } }

    private List<EnvironmentSpawnPoint> _spawnPoints;

    private List<EnvironmentObstacle> _obstacles;

    //Setup
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
    }

    public IEnumerator Coroutine_Build()
    {
        yield return Coroutine_ScanNavmesh();
        yield return Coroutine_GatherEnvironmentObjects();

        //dispose of the setup navmesh after tiles are built
        GridGraph gridGraph = AstarPath.active.data.gridGraph;
        AstarPath.active.data.RemoveGraph(gridGraph);

        Debug.Log("Environment Ready");
    }

    private IEnumerator Coroutine_ScanNavmesh()
    {
        Debug.Log("Scanning NavMesh");
        GridGraph gridGraph = AstarPath.active.data.gridGraph;
        AstarPath.active.Scan(gridGraph);

        yield return new WaitWhile(() => AstarPath.active.isScanning);
    }

    private IEnumerator Coroutine_GatherEnvironmentObjects()
    {
        Debug.Log("Gathering Environment Objects...");

        _spawnPoints = new List<EnvironmentSpawnPoint>();
        _obstacles = new List<EnvironmentObstacle>();

        foreach(EnvironmentSpawnPoint spawnPoint in GetComponentsInChildren<EnvironmentSpawnPoint>())
        {
            Vector3 position = spawnPoint.transform.position;

            Vector3 result;
            if(GetClosestNodeToPosition(position, out result))
            {
                _spawnPoints.Add(spawnPoint);
            }
            else
            {
                Debug.Log("Failed to gather spawn point " + spawnPoint.name);
                Destroy(spawnPoint.gameObject);
            }
        }

        foreach (EnvironmentObstacle obstacle in GetComponentsInChildren<EnvironmentObstacle>())
        {
            Vector3 position = obstacle.transform.position;

            Vector3 result;
            if (GetClosestNodeToPosition(position, out result))
            {
                _obstacles.Add(obstacle);
            }
            else
            {
                Debug.Log("Failed to gather obstacle " + obstacle.name);
                Destroy(obstacle.gameObject);
            }
        }

        yield return null;
    }

    //Encounter Events

    public IEnumerator Coroutine_EncounterStateUpdate(EncounterState stateID, EncounterModel model)
    {
        //if (_tileGrid != null)
        //{
        //    yield return _tileGrid.Coroutine_EncounterStateUpdate(stateID, model);
        //}

        yield return null;
    }

    //Helpers/interface 
    public bool GetClosestNodeToPosition(Vector3 position, out Vector3 result)
    {
        GridGraph gridGraph = AstarPath.active.data.gridGraph;

        NNInfoInternal nnInfo = gridGraph.GetNearest(position);

        if (nnInfo.node != null)
        {
            result = nnInfo.clampedPosition;
            return true;
        }
        else
        {
            result = Vector3.zero;
            return false;
        }
    }
    public CharacterComponent SpawnCharacter(TeamID teamID, CharacterID characterID)
    {
        //find a spawn point to place the character
        //see if we have a marker available to spawn them in
        foreach (EnvironmentSpawnPoint spawnPoint in _spawnPoints)
        {
            Vector3 location;
            if (GetClosestNodeToPosition(spawnPoint.transform.position, out location))
            {
                if (spawnPoint.GetTeam() == teamID)
                {
                    GameObject characterObject = new GameObject(teamID + "_" + characterID);

                    CharacterComponent characterComponent = AddComponentByTeam(characterID, characterObject);

                    characterObject.transform.parent = this.transform;
                    characterObject.transform.localPosition = Vector3.zero;
                    characterObject.transform.localEulerAngles = spawnPoint.transform.eulerAngles;
                    characterObject.transform.position = location;

                    return characterComponent;
                }
            }
           else
            {
                Debug.Log("Couldnt find location to spawn character");
            }
        }

        return null;
    }

    private static CharacterComponent AddComponentByTeam(CharacterID characterID, GameObject characterObject)
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

    public List<EnvironmentSpawnPoint> GetSpawnPoints()
    {
        return _spawnPoints;
    }

    public List<EnvironmentObstacle> GetObstacles()
    {
        return _obstacles;
    }
}
