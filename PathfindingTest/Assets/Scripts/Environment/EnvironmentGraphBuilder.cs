using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using static Constants;
using Unity.Jobs;

public class EnvironmentGraphBuilder : MonoBehaviour
{
    [SerializeField] public int Width = 12;

    private TileGraph _graph;

    public void Awake()
    {
        Build();
    }

    private void Build()
    {
        // This holds all graph data
        AstarData data = AstarPath.active.data;

        // This creates a Grid Graph
        _graph = data.AddGraph(typeof(TileGraph)) as TileGraph;
        _graph.drawGizmos = true;

        // Updates internal size from the above values

        //int offset = (Width * 3) / 4;

        //_graph.SetDimensions(Width, Width, ENV_TILE_SIZE);
        //_graph.center = new Vector3(offset, 0, offset);

        //GridNode[] nodes = new GridNode[Width*Width];
        //JobHandle job = AstarPath.active.AllocateNodes(nodes, 128, () => new GridNode(), 1);

        //AstarPath.active.FlushGraphUpdates();

        //job.Complete();

        // _graph.nodes = new GridNodeBase[_graph.width*_graph.width];

        ////create a grid node for each tile in our map 
        //for (int Row = 0; Row < _graph.width; Row++)
        //{
        //    for (int Column = 0; Column < _graph.width; Column++)
        //    {
        //        Vector3 origin = CalculateTileOrigin(Row, Column);

        //        EnvironmentLayer layer = EnvironmentUtil.CheckTileLayer(origin);

        //        //create a grid node for this tile
        //        GridNode node = new GridNode();
        //        node.position = new Int3(origin);
        //        node.Walkable = IsLayerTraversible(layer);

        //        _graph.nodes[Row * _graph.width + Column] = node;

        //        _graph.nodes[Row * _graph.width + Column].Walkable = IsLayerTraversible(layer);
        //    }
        //}

        ////now, find the neighbors for each node
        //foreach (GridNode node in _graph.nodes)
        //{
        //    List<Connection> connections = new List<Connection>();

        //    foreach (Vector3 origin in EnvironmentUtil.GetTileNeighbors(((Vector3)node.position)))
        //    {
        //        NNInfo info = _graph.GetNearest(origin);
        //        GridNode neighbor = (GridNode) info.node;

        //        if(neighbor != null)
        //        {
        //            connections.Add(new Connection(neighbor, 1, true, true));
        //        }
        //    }

        //    node.connections = connections.ToArray();
        //}
    }
}
