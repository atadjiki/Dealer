using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Unity.Jobs;

// Inherit our new graph from the base graph type
[JsonOptIn]
// Make sure the class is not stripped out when using code stripping (see https://docs.unity3d.com/Manual/ManagedCodeStripping.html)
[Pathfinding.Util.Preserve]
public class TileGraph : NavGraph
{
    // This should return true if the graph is scanned and can be used for pathfinding
    public override bool isScanned => true;

    class TileGraphScanPromise : IGraphUpdatePromise
    {
        public TileGraph graph;

        // In this method you may run async calculations required for updating the graph.
        // After this coroutine has finished, the Apply method will be called.
        // Any JobHandles that are yielded by this method will be waited for before the next iteration, and before the Apply method is called.
        public IEnumerator<JobHandle> Prepare() => null;

        public void Apply(IGraphUpdateContext ctx)
        {
            // Here we will place our code for scanning the graph

            // Destroy previous nodes (if any)
            graph.DestroyAllNodes();
        }
    }

    protected override IGraphUpdatePromise ScanInternal() => new TileGraphScanPromise { graph = this };

    public override void GetNodes(System.Action<GraphNode> action)
    {
        // This method should call the delegate with all nodes in the graph
    }
}