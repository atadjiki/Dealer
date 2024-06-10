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

    public bool AreValidCoordinates(Int2 coords)
    {
        if (coords.x < 0 || coords.y < 0)
        {
            return false;
        }

        if (coords.x >= Width || coords.y >= Width)
        {
            return false;
        }

        return true;
    }

    class TileGraphScanPromise : IGraphUpdatePromise
    {
        public TileGraph graph;

        // In this method you may run async calculations required for updating the graph.
        public IEnumerator<Unity.Jobs.JobHandle> Prepare() => null;
        public void Apply(IGraphUpdateContext ctx)
        {
            // Destroy previous nodes (if any)
            graph.DestroyAllNodes();

            int totalSize = graph.Width * graph.Width;

            //allocate our nodes
            TileNode[] nodes = new TileNode[totalSize];
            JobHandle job = AstarPath.active.AllocateNodes(nodes, totalSize, () => new TileNode(), 1);
            job.Complete();

            //iterate through each tile and connect them
            for (int Row = 0; Row < graph.Width; Row++)
            {
                for (int Column = 0; Column < graph.Width; Column++)
                {
                    Vector3 origin = CalculateTileOrigin(Row, Column);

                    EnvironmentLayer layer = EnvironmentUtil.CheckTileLayer(origin);

                    TileNode node = nodes[(Row * graph.Width) + Column];
                    node.Setup(origin, layer, graph.graphIndex);

                    List<Connection> connections = new List<Connection>();

                    foreach (EnvironmentDirection dir in GetAllDirections())
                    {
                        TileConnectionInfo info = EnvironmentUtil.CheckNeighborConnection(origin, dir);

                        Vector3 direction = GetDirectionVector(dir) / 2;

                        Vector3 neighborOrigin = GetNeighboringTileLocation(origin, dir);

                        Int2 coords = GetNeighboringTileCoordinates(origin, dir);

                        if (graph.AreValidCoordinates(coords))
                        {
                            TileNode neighbor = nodes[(coords.x * graph.Width) + coords.y];

                            bool valid = info.IsValid();

                            var cost = GetDirectionCost(dir);

                            Connection connection = new Connection(neighbor, cost, valid, valid);

                            connections.Add(connection);
                        }
                    }

                    //Debug.Log("Node[" + Row + "][" + Column + "]    " + connections.Count + " connections");
                    node.connections = connections.ToArray();
                }
            }

            //now iterate again so we can find jumps over obstacles
            foreach(TileNode node in nodes)
            {
                if(node.Walkable)
                {
                    Vector3 origin = (Vector3)node.position;
                    foreach (EnvironmentDirection dir in GetCardinalDirections())
                    {
                        TileConnectionInfo info = EnvironmentUtil.CheckNeighborConnection(origin, dir);

                        if(!info.IsWallBetween())
                        {
                            if(GetCoverType(info) == EnvironmentCover.HALF)
                            {
                                Vector3 neighborOrigin = GetNeighboringTileLocation(origin, dir);

                                Int2 neighborCoords = CalculateTileCoordinates(neighborOrigin);

                                if(graph.AreValidCoordinates(neighborCoords))
                                {
                                    //if we're next to cover, check if we can jump this tile
                                    TileNode neighbor = nodes[(neighborCoords.x * graph.Width) + neighborCoords.y];

                                    TileConnectionInfo neighborInfo = EnvironmentUtil.CheckNeighborConnection(neighborOrigin, dir);

                                    if(neighborInfo.IsValid())
                                    {
                                        //get the node after this node
                                        Vector3 nextOrigin = GetNeighboringTileLocation(neighborOrigin, dir);

                                        Int2 nextCoords = CalculateTileCoordinates(nextOrigin);

                                        if (graph.AreValidCoordinates(nextCoords))
                                        {
                                            TileNode next = nodes[(nextCoords.x * graph.Width) + nextCoords.y];

                                            if (next.Walkable)
                                            {
                                                var cost = GetDirectionCost(dir) * 2;

                                                node.AddPartialConnection(next, cost, true, true);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }

            graph.nodes = nodes;
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

                                    Vector3 normal = GetCoverNormal(dir);

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