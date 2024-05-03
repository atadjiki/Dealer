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

    PointNode CreateNode(Vector3 position)
    {
        var node = new PointNode(active);

        // Node positions are stored as Int3. We can convert a Vector3 to an Int3 like this
        node.position = (Int3)position;
        node.GraphIndex = graphIndex;
        return node;
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

            List<PointNode> allNodes = new List<PointNode>();

            for(int Row = 0; Row < graph.width; Row++)
            {
                for(int Column = 0; Column < graph.width; Column++)
                {
                    Vector3 origin = CalculateTileOrigin(Row, Column);

                    EnvironmentLayer layer = EnvironmentUtil.CheckTileLayer(origin);

                    PointNode node = graph.CreateNode(origin);

                    node.Walkable = IsLayerTraversible(layer);

                    allNodes.Add(node);
                }
            }

            graph.nodes = allNodes.ToArray();
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
}