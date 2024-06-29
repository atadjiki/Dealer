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

    [JsonMember] public MovementPathType AllowedMovementTypes = MovementPathType.MOVE | MovementPathType.VAULT_OBSTACLE | MovementPathType.VAULT_WALL;

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

    public TileNode GetNode(int row, int column, int level)
    {
        int index = row + Width * (level + Levels * column);

        return nodes[index];
    }

    private bool AreValidCoordinates(Int3 coords)
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


    private void PerformScan()
    {
        // Destroy previous nodes (if any)
        DestroyAllNodes();

        int totalSize = Width * Width * Levels;

        //allocate our nodes
        nodes = new TileNode[totalSize];
        JobHandle job = AstarPath.active.AllocateNodes(nodes, totalSize, () => new TileNode(), 1);
        job.Complete();

        //iterate through each tile and connect them
        for (int Level = 0; Level < Levels; Level++)
        {
            for (int Row = 0; Row < Width; Row++)
            {
                for (int Column = 0; Column < Width; Column++)
                {
                    Vector3 origin = CalculateTileOrigin(Row, Level, Column);

                    EnvironmentLayer layer = EnvironmentUtil.CheckTileLayer(origin);

                    TileNode node = GetNode(Row, Column, Level); 
                    node.Setup(origin, layer, graphIndex);

                    List<Connection> connections = new List<Connection>();

                    foreach (EnvironmentDirection dir in GetAllDirections())
                    {
                        TileConnectionInfo info = EnvironmentUtil.CheckNeighborConnection(origin, dir);

                        Vector3 direction = GetDirectionVector(dir) / 2;

                        Vector3 neighborOrigin = GetNeighboringTileLocation(origin, dir);

                        Int3 coords = GetNeighboringTileCoordinates(origin, dir);

                        if (AreValidCoordinates(coords))
                        {
                            TileNode neighbor = GetNode(coords.x, coords.y, Level);

                            bool valid = info.IsValid() && AllowedMovementTypes.HasFlag(MovementPathType.MOVE);

                            uint cost = GetDirectionCost(dir);

                            Connection connection = new Connection(neighbor, cost, valid, valid);

                            connections.Add(connection);
                        }
                    }

                    node.connections = connections.ToArray();
                }
            }
        }

        //now iterate again so we can find special connections (obstacles, wall vaults, stairs, ladders)
        for (int Level = 0; Level < Levels; Level++)
        {
            for (int Row = 0; Row < Width; Row++)
            {
                for (int Column = 0; Column < Width; Column++)
                {
                    TileNode node = GetNode(Row, Column, Level);

                    if (node.Walkable)
                    {
                        Vector3 origin = (Vector3)node.position;
                        foreach (EnvironmentDirection dir in GetCardinalDirections())
                        {
                            TileConnectionInfo info = EnvironmentUtil.CheckNeighborConnection(origin, dir);

                            //check for jumps over half obstacles
                            if (!info.IsWallBetween() && AllowedMovementTypes.HasFlag(MovementPathType.VAULT_OBSTACLE))
                            {
                                if (GetCoverType(info) == EnvironmentCover.HALF)
                                {
                                    Vector3 neighborOrigin = GetNeighboringTileLocation(origin, dir);

                                    Int3 neighborCoords = CalculateTileCoordinates(neighborOrigin);

                                    if (AreValidCoordinates(neighborCoords))
                                    {
                                        //if we're next to cover, check if we can jump this tile
                                        TileNode neighbor = GetNode(neighborCoords.x, neighborCoords.y, Level);

                                        TileConnectionInfo neighborInfo = EnvironmentUtil.CheckNeighborConnection(neighborOrigin, dir);

                                        if (neighborInfo.IsValid())
                                        {
                                            //get the node after this node
                                            Vector3 nextOrigin = GetNeighboringTileLocation(neighborOrigin, dir);

                                            Int3 nextCoords = CalculateTileCoordinates(nextOrigin);

                                            if (AreValidCoordinates(nextCoords))
                                            {
                                                TileNode next = GetNode(nextCoords.x, nextCoords.y, Level);
                                            
                                                if (next.Walkable)
                                                {
                                                    var cost = GetDirectionCost(dir);

                                                    node.AddPartialConnection(next, cost, true, true);

                                                    node.AddTransition(next, MovementPathType.VAULT_OBSTACLE);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //check for wall vaults
                            else if (info.IsLayerBetween(EnvironmentLayer.WALL_HALF) && AllowedMovementTypes.HasFlag(MovementPathType.VAULT_WALL))
                            {
                                Vector3 neighborOrigin = GetNeighboringTileLocation(origin, dir);

                                Int3 neighborCoords = CalculateTileCoordinates(neighborOrigin);

                                if (AreValidCoordinates(neighborCoords))
                                {
                                    //if the tile over this wall is walkable, we can jump it
                                    TileNode neighbor = GetNode(neighborCoords.x, neighborCoords.y, Level);
                                    
                                    TileConnectionInfo neighborInfo = EnvironmentUtil.CheckNeighborConnection(neighborOrigin, dir);

                                    if (neighborInfo.IsValid())
                                    {
                                        uint cost = GetDirectionCost(dir);

                                        node.AddPartialConnection(neighbor, cost, true, true);

                                        node.AddTransition(neighbor, MovementPathType.VAULT_WALL);
                                    }
                                }
                            }
                        }

                    }


                    //if the current node is a stair, and it's neighbor is a stair, and the neighbor's neighbor is a floor,
                    //string them all together
                    //keep in mind one floor tile is at the current level, and the other is at level ++
                    //this case only applies to one node in the staircase, so we can do all the operations at once

                    else if (node.layer == EnvironmentLayer.STAIRS && AllowedMovementTypes.HasFlag(MovementPathType.STAIRS))
                    {
                        Vector3 origin = (Vector3)node.position;

                        //strip any existing connections since stairs have special movement
                        node.ClearConnections(true);

                        foreach (EnvironmentDirection dir in GetCardinalDirections())
                        {
                            Vector3 neighborOrigin = GetNeighboringTileLocation(origin, dir);

                            Int3 neighborCoords = CalculateTileCoordinates(neighborOrigin);

                            if (AreValidCoordinates(neighborCoords))
                            {
                                //if we're next to cover, check if we can jump this tile
                                TileNode neighbor = GetNode(neighborCoords.x, neighborCoords.y, Level);

                                if (neighbor.layer == EnvironmentLayer.STAIRS)
                                {
                                    //get the node after this node
                                    Vector3 nextOrigin = GetNeighboringTileLocation(neighborOrigin, dir);

                                    Int3 nextCoords = CalculateTileCoordinates(nextOrigin);

                                    if (AreValidCoordinates(nextCoords))
                                    {
                                        TileNode next = GetNode(nextCoords.x, nextCoords.y, Level);
                                        
                                        if (next.Walkable)
                                        {
                                            //one last check - check behind the origin node to make sure it's on the next floor
                                            EnvironmentDirection opposite = GetOpposingDirection(dir);

                                            Vector3 previousOrigin = GetNeighboringTileLocation(origin, opposite, Level + 1);

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
        }
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