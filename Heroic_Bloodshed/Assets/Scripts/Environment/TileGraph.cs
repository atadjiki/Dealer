using System.Collections.Generic;
using UnityEngine;
// Include the Pathfinding namespace to gain access to a lot of useful classes
using Pathfinding;
// Required to save the settings
using Pathfinding.Serialization;
using Pathfinding.Util;
using static Constants;
using Pathfinding.Drawing;
using Unity.Jobs;

[JsonOptIn]
// Make sure the class is not stripped out when using code stripping (see https://docs.unity3d.com/Manual/ManagedCodeStripping.html)
[Pathfinding.Util.Preserve]
// Inherit our new graph from a base graph type
public class TileGraph : NavGraph
{
    [JsonMember] public int Width = 12;
    [JsonMember] public int Levels = 3;

    [JsonMember] public MovementType AllowedMovementTypes = MovementType.MOVE | MovementType.VAULT_OBSTACLE | MovementType.VAULT_WALL;

    [JsonMember] public bool ShowConnections = true;
    [JsonMember] public bool ShowLayers = true;
    [JsonMember] public bool ShowCover = true;
    [JsonMember] public float LayerOpacity = 0.1f;
    [JsonMember] public float CoverOpacity = 1.0f;
    [JsonMember] public float LineWidth = 2;

    // Here we will store all nodes in the graph
    public TileNode[] nodes;

    public override bool isScanned => nodes != null;

    public List<TileNode> GetWalkableNodes()
    {
        List<TileNode> walkables = new List<TileNode>();

        foreach(TileNode node in nodes)
        {
            if(node.Walkable)
            {
                walkables.Add(node);
            }
        }

        return walkables;
    }

    public TileNode GetRandomNode()
    {
        if(nodes != null)
        {
            List<TileNode> walkables = GetWalkableNodes();

            int random = Random.Range(0, walkables.Count);

            return walkables[random];
        }

        return null;
    }

    public TileNode GetNode(TileCoordinates coords)
    {
        return GetNode(coords.Row, coords.Column, coords.Level);
    }

    public TileNode GetNode(int row, int column, int level)
    {
        int index = row + Width * (level + Levels * column);

        return nodes[index];
    }

    public bool AreValidCoordinates(Int3 coords)
    {
        if (coords.x < 0 || coords.y < 0)
        {
            return false;
        }

        if (coords.x >= Width || coords.y >= Width)
        {
            return false;
        }

        if(coords.z < 0 || coords.z >= Levels)
        {
            return false;
        }

        return true;
    }

    public bool HasValidNeighborInDirection(TileNode node, EnvironmentDirection dir)
    {
        Vector3 neighborOrigin = GetNeighboringTileLocation(node.GetOrigin(), dir);
        Int3 neighborCoords = CalculateTileCoordinates(neighborOrigin);

        return AreValidCoordinates(neighborCoords);
    }

    private void SetupNodes()
    {
        //first create a node for each square in the 3D grid
        for (int Level = 0; Level < Levels; Level++)
        {
            for (int Row = 0; Row < Width; Row++)
            {
                for (int Column = 0; Column < Width; Column++)
                {
                    TileCoordinates coords = TileCoordinates.Build(Row, Column, Level);

                    //let the node do the rest of the setup
                    TileNode node = GetNode(coords);
                    node.Setup(coords, graphIndex);  
                }
            }
        }

        foreach(TileNode node in nodes)
        {
            node.FindNodeConnections(this);
        }
    }

    private void ProcessNodes()
    {
        foreach(TileNode node in GetWalkableNodes())
        {
            node.FindNodeTransitions(this);

            //if the current node is a stair, and it's neighbor is a stair, and the neighbor's neighbor is a floor,
            //string them all together
            //keep in mind one floor tile is at the current level, and the other is at level ++
            //this case only applies to one node in the staircase, so we can do all the operations at once

            if (node.layer == EnvironmentLayer.STAIRS && AllowedMovementTypes.HasFlag(MovementType.STAIRS))
            {
                Vector3 origin = (Vector3)node.position;

                //strip any existing connections since stairs have special movement
                node.ClearConnections(true);

                foreach (EnvironmentDirection dir in GetCardinalDirections())
                {
                    TileConnectionInfo neighborInfo;
                    if (node.GetNeighborInfo(dir, out neighborInfo))
                    {
                        TileNode neighbor = neighborInfo.Node;

                        if (neighbor.layer == EnvironmentLayer.STAIRS)
                        {
                            //get the node after this node
                            TileConnectionInfo nextInfo;
                            if (neighbor.GetNeighborInfo(dir, out nextInfo))
                            {
                                TileNode next = nextInfo.Node;

                                if (next.Walkable)
                                {
                                    //one last check - check behind the origin node to make sure it's on the next floor
                                    EnvironmentDirection opposite = GetOpposingDirection(dir);

                                    Vector3 previousOrigin = GetNeighboringTileLocation(origin, opposite, node.GetLevel() + 1);

                                    Int3 previousCoords = CalculateTileCoordinates(previousOrigin);

                                    if (AreValidCoordinates(previousCoords))
                                    {
                                        TileNode previous = GetNode(previousCoords.x, previousCoords.y, previousCoords.z);

                                        if (previous.Walkable)
                                        {
                                            Debug.Log("Found staircase!");
                                            var cost = GetDirectionCost(dir);

                                            previous.AddPartialConnection(node, cost, true, true);
                                            node.AddPartialConnection(previous, cost, true, true);

                                            node.AddPartialConnection(neighbor, cost, true, true);
                                            neighbor.AddPartialConnection(node, cost, true, true);

                                            neighbor.AddPartialConnection(next, cost, true, true);
                                            next.AddPartialConnection(neighbor, cost, true, true);

                                            node.Walkable = true;
                                            neighbor.Walkable = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void PerformScan()
    {
        // Destroy previous nodes (if any)
        DestroyAllNodes();

        int totalSize = Width * Width * Levels;

        //allocate our nodes
        nodes = new TileNode[totalSize];
        JobHandle job = AstarPath.active.AllocateNodes(nodes, totalSize, () => new TileNode(), 1);
        job.Complete();

        SetupNodes();

        ProcessNodes();
    }


    class TileGraphScanPromise : IGraphUpdatePromise
    {
        public TileGraph graph;

        // In this method you may run async calculations required for updating the graph.
        public IEnumerator<JobHandle> Prepare() => null;
        public void Apply(IGraphUpdateContext ctx)
        {
            graph.PerformScan();
        }
    }

    protected override IGraphUpdatePromise ScanInternal() => new TileGraphScanPromise { graph = this };

    public override void GetNodes(System.Action<GraphNode> action)
    {
        if (nodes == null) return;

        for (int i = 0; i < nodes.Length; i++)
        {
            // Call the delegate
            action(nodes[i]);
        }
    }

    public override void OnDrawGizmos(DrawingData gizmos, bool drawNodes, RedrawScope redrawScope)
    {
        if (ShowConnections)
        {
            base.OnDrawGizmos(gizmos, drawNodes, redrawScope);

        }

        DrawingData.Hasher hasher = DrawingData.Hasher.Create(this);

        using (var builder = gizmos.GetBuilder(hasher))
        {
            GetNodes(graphNode =>
            {
                TileNode node = (TileNode)graphNode;

                Vector3 origin = (Vector3)node.position;

                if (node.layer != EnvironmentLayer.NONE)
                {
                    if (ShowLayers)
                    {
                        Color color;

                        if (node.Walkable)
                        {
                            color = Color.green;
                        }
                        else if(node.layer == EnvironmentLayer.STAIRS)
                        {
                            color = Color.blue;
                        }
                        else
                        {
                            color = Color.red;
                        }

                        color.a = LayerOpacity;

                        using (builder.WithColor(color))
                        {
                            builder.SolidBox(origin, new Unity.Mathematics.float3(ENV_TILE_SIZE, 0.01f, ENV_TILE_SIZE));
                        }
                    }

                    if (ShowCover)
                    {
                        foreach (KeyValuePair<EnvironmentDirection, EnvironmentCover> pair in node.GetCoverMap())
                        {
                            EnvironmentDirection dir = pair.Key;
                            EnvironmentCover cover = pair.Value;

                            Vector3[] edge = CalculateTileEdge(origin, dir);

                            Color color = Color.yellow;

                            if (cover == EnvironmentCover.FULL)
                            {
                                color = Color.magenta;
                            }

                            color.a = CoverOpacity;

                            using (builder.WithColor(color))
                            {
                                using (builder.WithLineWidth(LineWidth))
                                {
                                    float height = GetCoverHeight(cover);

                                    Vector3 center = (edge[0] + edge[1]) / 2; ;

                                    center += new Vector3(0, 3 * height / 4, 0);

                                    Vector3 normal = GetDirectionNormal(dir);

                                    builder.PlaneWithNormal(center, normal, height);
                                }
                            }
                        }
                    }
                }
            });
        }
    }
}