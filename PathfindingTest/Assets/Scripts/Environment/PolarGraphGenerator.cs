using System.Collections.Generic;
using UnityEngine;
// Include the Pathfinding namespace to gain access to a lot of useful classes
using Pathfinding;
// Required to save the settings
using Pathfinding.Serialization;
using Pathfinding.Util;

[JsonOptIn]
// Make sure the class is not stripped out when using code stripping (see https://docs.unity3d.com/Manual/ManagedCodeStripping.html)
[Pathfinding.Util.Preserve]
// Inherit our new graph from a base graph type
public class PolarGraph : NavGraph
{
    [JsonMember]
    public int circles = 10;
    [JsonMember]
    public int steps = 20;

    [JsonMember]
    public Vector3 center = Vector3.zero;

    [JsonMember]
    public float scale = 2;

    // Here we will store all nodes in the graph
    public PointNode[] nodes;

    GraphTransform transform;

    public override bool isScanned => nodes != null;

    PointNode CreateNode(Vector3 position)
    {
        var node = new PointNode(active);

        // Node positions are stored as Int3. We can convert a Vector3 to an Int3 like this
        node.position = (Int3)position;
        node.GraphIndex = graphIndex;
        return node;
    }

    static Vector3 CalculateNodePosition(int circle, float angle, GraphTransform transform)
    {
        // Get the direction towards the node from the center
        var pos = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));

        // Multiply it with the circle number to get the node position in graph space
        pos *= circle;

        // Multiply it with the matrix to get the node position in world space
        pos = transform.Transform(pos);
        return pos;
    }
    class PolarGraphScanPromise : IGraphUpdatePromise
    {
        public PolarGraph graph;

        // In this method you may run async calculations required for updating the graph.
        public IEnumerator<Unity.Jobs.JobHandle> Prepare() => null;
        public void Apply(IGraphUpdateContext ctx)
        {
            // Destroy previous nodes (if any)
            graph.DestroyAllNodes();

            var circles = graph.circles;
            var steps = graph.steps;

            // Create a 2D array which will contain all nodes
            // This is just a tempoary array to make it easier to reference different nodes
            PointNode[][] circleNodes = new PointNode[circles][];

            // Create a matrix which just moves the nodes to #center
            // and scales their positions by #scale
            // The GraphTransform class has various utility methods for working with it
            graph.transform = new GraphTransform(Matrix4x4.TRS(graph.center, Quaternion.identity, Vector3.one * graph.scale));

            // Place the center node in the center
            circleNodes[0] = new PointNode[] {
                graph.CreateNode(CalculateNodePosition(0, 0, graph.transform))
            };
            // The size of the angle (in radians) each step will use
            float anglesPerStep = (2 * Mathf.PI) / steps;

            for (int circle = 1; circle < circles; circle++)
            {
                circleNodes[circle] = new PointNode[steps];
                for (int step = 0; step < steps; step++)
                {
                    // Get the angle to the node relative to the center
                    float angle = step * anglesPerStep;

                    Vector3 pos = CalculateNodePosition(circle, angle, graph.transform);
                    circleNodes[circle][step] = graph.CreateNode(pos);
                }
            }
            // Now all nodes are created, let's create some connections between them!

            // Iterate through all circles
            // circle 0 is just the center node so we skip that for now
            for (int circle = 1; circle < circles; circle++)
            {
                for (int step = 0; step < steps; step++)
                {
                    // Get the current node
                    PointNode node = circleNodes[circle][step];

                    // The nodes here will always have exactly four connections, like a grid, but polar.
                    // Except for those in the last circle which will only have three connections
                    int numConnections = circle < circles - 1 ? 4 : 3;
                    var connections = new Connection[numConnections];

                    // Get the next clockwise node in the current circle.
                    // The last node in each circle should be linked to the first node
                    // in the circle which is why we use the modulo operator.
                    connections[0].node = circleNodes[circle][(step + 1) % steps];

                    // Counter clockwise node. Here we check for underflow instead
                    connections[1].node = circleNodes[circle][(step - 1 + steps) % steps];

                    // The node in the previous circle (in towards the center)
                    if (circle > 1)
                    {
                        connections[2].node = circleNodes[circle - 1][step];
                    }
                    else
                    {
                        // Create a connection to the middle node, special case
                        connections[2].node = circleNodes[circle - 1][0];
                    }

                    // Are there any more circles outside this one?
                    if (numConnections == 4)
                    {
                        // The node in the next circle (out from the center)
                        connections[3].node = circleNodes[circle + 1][step];
                    }

                    for (int q = 0; q < connections.Length; q++)
                    {
                        // Node.position is an Int3, here we get the cost of moving between the two positions
                        connections[q].cost = (uint)(node.position - connections[q].node.position).costMagnitude;
                    }

                    node.connections = connections;
                }
            }
            // The center node is a special case, so we have to deal with it separately
            PointNode centerNode = circleNodes[0][0];
            centerNode.connections = new Connection[steps];

            // Assign all nodes in the first circle as connections to the center node
            for (int step = 0; step < steps; step++)
            {
                centerNode.connections[step] = new Connection(
                    circleNodes[1][step],
                    // centerNode.position is an Int3, here we get the cost of moving between the two positions
                    (uint)(centerNode.position - circleNodes[1][step].position).costMagnitude,
                    isOutgoing: true,
                    isIncoming: true
                    );
            }
            // Store all nodes in the nodes array
            List<PointNode> allNodes = new List<PointNode>();
            for (int i = 0; i < circleNodes.Length; i++)
            {
                allNodes.AddRange(circleNodes[i]);
            }
            graph.nodes = allNodes.ToArray();

            // Set all the nodes to be walkable
            for (int i = 0; i < graph.nodes.Length; i++)
            {
                graph.nodes[i].Walkable = true;
            }
        }
    }

    protected override IGraphUpdatePromise ScanInternal() => new PolarGraphScanPromise { graph = this };

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