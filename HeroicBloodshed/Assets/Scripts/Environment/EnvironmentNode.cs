using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentNode 
{
    public Vector3 position;
    public List<Vector3> neighbors;

    public EnvironmentSpawnPoint spawnPoint;
    public EnvironmentObstacle obstacle;
    public CharacterComponent character;
}
