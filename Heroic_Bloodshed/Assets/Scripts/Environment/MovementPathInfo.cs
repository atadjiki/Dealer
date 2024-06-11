using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using static Constants;

public class MovementPathInfo
{
    public MovementPathType PathType;
    public List<TileNode> Nodes;

    public MovementPathInfo(List<TileNode> _nodes)
    {
        PathType = MovementPathType.MOVE;
        Nodes = _nodes;
    }

    public MovementPathInfo()
    {
        PathType = MovementPathType.MOVE;
        Nodes = new List<TileNode>();
    }

    public MovementPathInfo(TileNode jumpStart, TileNode jumpEnd)
    {
        PathType = MovementPathType.JUMP;
        Nodes = new List<TileNode>() { jumpStart, jumpEnd };
    }

    public List<Vector3> GetVectors()
    {
        List<Vector3> vectors = new List<Vector3>();

        foreach(TileNode node in Nodes)
        {
            Vector3 vector = (Vector3) node.position;

            vectors.Add(vector);
        }

        return vectors;
    }

    public Vector3 GetStart()
    {
        return (Vector3)Nodes[0].position;
    }

    public Vector3 GetEnd()
    {
        return (Vector3)Nodes[Nodes.Count-1].position;
    }
}