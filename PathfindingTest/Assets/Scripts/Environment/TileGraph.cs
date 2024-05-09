using System.Collections.Generic;
using UnityEngine;
// Include the Pathfinding namespace to gain access to a lot of useful classes
using Pathfinding;
// Required to save the settings
using Pathfinding.Serialization;
using Pathfinding.Util;
using static Constants;
using Pathfinding.Drawing;

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
    public PointNode[] nodes;

    public override bool isScanned => nodes != null;

    PointNode CreateNode(Vector3 position, EnvironmentLayer layer)
    {
        var node = new PointNode(active);

        // Node positions are stored as Int3. We can convert a Vector3 to an Int3 like this
        node.position = (Int3)position;
        node.GraphIndex = graphIndex;
        node.Walkable = IsLayerTraversible(layer);
        node.Tag = GetPathfindingTag(layer);
        return node;
    }

    public List<PointNode> GetWalkableNodes()
    {
        List<PointNode> walkables = new List<PointNode>();

        foreach(PointNode node in nodes)
        {
            if(node.Walkable)
            {
                walkables.Add(node);
            }
        }

        return walkables;
    }

    public PointNode GetRandomNode()
    {
        if(nodes != null)
        {
            List<PointNode> walkables = GetWalkableNodes();

            int random = Random.Range(0, walkables.Count);

            return walkables[random];
        }

        return null;
    }

    PointNode GetNode(int Row, int Column)
    {
        if(nodes != null)
        {
            return nodes[Row * Width + Column];
        }

        return null;
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

            List<PointNode> nodes = new List<PointNode>();

            for (int Row = 0; Row < graph.Width; Row++)
            {
                for (int Column = 0; Column < graph.Width; Column++)
                {
                    Vector3 origin = CalculateTileOrigin(Row, Column);

                    EnvironmentLayer layer = EnvironmentUtil.CheckTileLayer(origin);

                    PointNode node = graph.CreateNode(origin, layer);

                    nodes.Add(node);
                }
            }

            for (int Row = 0; Row < graph.Width; Row++)
            {
                for (int Column = 0; Column < graph.Width; Column++)
                {
                    Vector3 origin = CalculateTileOrigin(Row, Column);

                    PointNode node = nodes[(Row * graph.Width) + Column];

                    EnvironmentTileConnectionMap neighborMap = EnvironmentUtil.GenerateNeighborMap(origin);

                    List<Connection> connections = new List<Connection>();

                    foreach (EnvironmentDirection dir in GetAllDirections())
                    {
                        EnvironmentTileConnectionInfo info = neighborMap[dir];

                        Vector3 direction = GetDirectionVector(dir) / 2;

                        Vector3 neighborOrigin = GetNeighboringTileLocation(origin, dir);

                        Int2 coords = GetNeighboringTileCoordinates(origin, dir);

                        if (AreValidCoordinates(coords, graph.Width))
                        {
                            PointNode neighbor = nodes[(coords.x * graph.Width) + coords.y];

                            bool valid = info.IsValid();

                            var cost = (uint)(neighbor.position - node.position).costMagnitude;

                            connections.Add(new Connection(neighbor, cost, valid, valid));
                        }
                    }

                    //Debug.Log("Node[" + Row + "][" + Column + "]    " + connections.Count + " connections");
                    node.connections = connections.ToArray();
                }
            }

            graph.nodes = nodes.ToArray();
        
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
        if(ShowConnections)
        {
            base.OnDrawGizmos(gizmos, drawNodes, redrawScope);

        }

        DrawingData.Hasher hasher = DrawingData.Hasher.Create(this);

        using (var builder = gizmos.GetBuilder(hasher))
        {
            GetNodes(node =>
            {
                Vector3 origin = (Vector3)node.position;
                EnvironmentLayer layer = GetLayerByTag(node.Tag);

                if(layer != EnvironmentLayer.NONE)
                {
                    if (ShowLayers)
                    {
                        Color color = GetLayerDebugColor(layer, true, false);
                        color.a = LayerOpacity;

                        using (builder.WithColor(color))
                        {
                            builder.SolidBox(origin, new Unity.Mathematics.float3(ENV_TILE_SIZE, 0.01f, ENV_TILE_SIZE));
                        }
                    }

                    if (ShowCover)
                    {
                        EnvironmentTileConnectionMap neighborMap = EnvironmentUtil.GenerateNeighborMap(origin);

                        foreach (EnvironmentDirection dir in GetCardinalDirections())
                        {
                            EnvironmentTileConnectionInfo info = neighborMap[dir];

                            if (IsLayerCover(info.Obstruction))
                            {
                                Vector3[] edge = CalculateTileEdge(origin, dir);

                                EnvironmentCover cover = GetCoverType(info);

                                Color color = GetCoverDebugColor(cover);
                                color.a = CoverOpacity;

                                using (builder.WithColor(color))
                                {
                                    using (builder.WithLineWidth(LineWidth))
                                    {
                                        float height = GetCoverHeight(cover);

                                        Vector3 center = (edge[0] + edge[1]) / 2; ;

                                        center += new Vector3(0, 3*height/4, 0);

                                        Vector3 normal = GetCoverNormal(dir);

                                        builder.PlaneWithNormal(center, normal, height);
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }
    }
}