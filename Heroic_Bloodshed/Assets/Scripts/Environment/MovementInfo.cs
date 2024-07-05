using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using static Constants;

public class MovementInfo
{
    public MovementType Type;
    public List<TileNode> Nodes;

    public MovementInfo(List<TileNode> _nodes)
    {
        Type = MovementType.MOVE;
        Nodes = _nodes;
    }

    public MovementInfo()
    {
        Type = MovementType.MOVE;
        Nodes = new List<TileNode>();
    }

    public MovementInfo(TileNode jumpStart, TileNode jumpEnd, MovementType _type)
    {
        Type = _type;
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