using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentNode : MonoBehaviour
{
    [SerializeField] private List<Vector3> neighbors;

    [SerializeField] private EnvironmentSpawnPoint spawnPoint;
    [SerializeField] private EnvironmentObstacle obstacle;
    [SerializeField] private CharacterComponent character;

    public void SetNeighbors(List<Vector3> _neighbors)
    {
        neighbors = _neighbors;
    }

    public List<Vector3> GetNeighbors()
    {
        return neighbors;
    }

    public void SetSpawnPoint(EnvironmentSpawnPoint _spawnPoint)
    {
        spawnPoint = _spawnPoint;
    }

    public EnvironmentSpawnPoint GetSpawnPoint()
    {
        return spawnPoint;
    }

    public bool HasSpawnPoint()
    {
        return spawnPoint != null;
    }

    public void SetObstacle(EnvironmentObstacle _obstacle)
    {
        obstacle = _obstacle;
    }

    public bool HasObstacle()
    {
        return obstacle != null;
    }

    public EnvironmentObstacle GetObstacle()
    {
        return obstacle;
    }

    public void SetCharacter(CharacterComponent _character)
    {
        character = _character;
    }

    public CharacterComponent GetCharacter()
    {
        return character;
    }

    public bool HasCharacter()
    {
        return character != null;
    }
}
