using System.Collections.Generic;
using UnityEngine;
// Include the Pathfinding namespace to gain access to a lot of useful classes
using Pathfinding;
// Required to save the settings
using Pathfinding.Serialization;
using Pathfinding.Util;
using static Constants;

[JsonOptIn]
// Make sure the class is not stripped out when using code stripping (see https://docs.unity3d.com/Manual/ManagedCodeStripping.html)
[Pathfinding.Util.Preserve]
// Inherit our new graph from a base graph type
public class TileGraph : NavGraph
{
    [JsonMember]
    public int width = 12;

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
        node.Tag = GetTag(layer);
        return node;
    }

    PointNode GetNode(int Row, int Column)
    {
        if(nodes != null)
        {
            return nodes[Row * width + Column];
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

            for (int Row = 0; Row < graph.width; Row++)
            {
                for (int Column = 0; Column < graph.width; Column++)
                {
                    Vector3 origin = CalculateTileOrigin(Row, Column);

                    EnvironmentLayer layer = EnvironmentUtil.CheckTileLayer(origin);

                    PointNode node = graph.CreateNode(origin, layer);

                    nodes.Add(node);
                }
            }

            for (int Row = 0; Row < graph.width; Row++)
            {
                for (int Column = 0; Column < graph.width; Column++)
                {
                    Vector3 origin = CalculateTileOrigin(Row, Column);

                    PointNode node = nodes[(Row * graph.width) + Column];

                    EnvironmentTileConnectionMap neighborMap = EnvironmentUtil.GenerateNeighborMap(origin);

                    List<Connection> connections = new List<Connection>();

                    foreach (EnvironmentDirection dir in GetAllDirections())
                    {
                        EnvironmentTileConnectionInfo info = neighborMap[dir];

                        Vector3 direction = GetDirectionVector(dir) / 2;

                        Vector3 neighborOrigin = GetNeighboringTileLocation(origin, dir);

                        Int2 coords = GetNeighboringTileCoordinates(origin, dir);

                        if (AreValidCoordinates(coords, graph.width))
                        {
                            PointNode neighbor = nodes[(coords.x * graph.width) + coords.y];

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

    private uint GetTag(EnvironmentLayer layer)
    {
        switch(layer)
        {
            case EnvironmentLayer.Ground:
                return 0;
            case EnvironmentLayer.Obstacle_Half:
                return 1;
            case EnvironmentLayer.Obstacle_Full:
                return 2;
            case EnvironmentLayer.Wall:
                return 3;
        }

        return 0;
    }
}