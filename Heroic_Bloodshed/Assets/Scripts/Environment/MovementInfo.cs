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
            Vector3 vector = node.GetTruePosition();

            vectors.Add(vector);
        }

        return vectors;
    }

    public Vector3 GetStart()
    {
        return Nodes[0].GetGridPosition();
    }

    public Vector3 GetEnd()
    {
        return Nodes[Nodes.Count - 1].GetGridPosition();
    }
}