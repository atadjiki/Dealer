using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using static Constants;

public class EnvironmentGraphBuilder : MonoBehaviour
{ 
    private GridGraph _graph;

    public void Awake()
    {
        Build();
    }

    private void Build()
    {
        _graph = new GridGraph();
        _graph.drawGizmos = true;
        _graph.nodeSize = ENV_TILE_SIZE;
        _graph.width = 12;
        _graph.nodes = new GridNodeBase[_graph.width*_graph.width];

        //create a grid node for each tile in our map 
        for (int Row = 0; Row < _graph.width; Row++)
        {
            for(int Column = 0; Column < _graph.width; Column++)
            {
                Vector3 origin = CalculateTileOrigin(Row, Column);

                EnvironmentLayer layer = EnvironmentUtil.CheckTileLayer(origin);

                //create a grid node for this tile
                GridNode node = new GridNode();
                node.position = new Int3(origin);
                node.Walkable = IsLayerTraversible(layer);

                _graph.nodes[Row * _graph.width + Column] = node;
            }
        }

        //now, find the neighbors for each node
        foreach (GridNode node in _graph.nodes)
        {
            List<Connection> connections = new List<Connection>();

            foreach (Vector3 origin in EnvironmentUtil.GetTileNeighbors(((Vector3)node.position)))
            {
                NNInfo info = _graph.GetNearest(origin);
                GridNode neighbor = (GridNode) info.node;

                if(neighbor != null)
                {
                    connections.Add(new Connection(neighbor, 1, true, true));
                }
            }

            node.connections = connections.ToArray();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_graph != null)
        {
            if(_graph.nodes != null)
            {
                foreach (GridNode node in _graph.nodes)
                {
                    if(node.Walkable)
                    {
                        Gizmos.color = Color.green;
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                    }
  
                    Gizmos.DrawCube(((Vector3)node.position), new Vector3(0.1f, 0.1f, 0.1f));


                    if(node.connections != null)
                    {
                        Gizmos.color = Color.white;

                        foreach (Connection connection in node.connections)
                        {
                            GridNode neighbor = (GridNode)connection.node;
                            Gizmos.DrawLine(((Vector3)node.position), ((Vector3)neighbor.position));
                        }
                    }

                }
            }
        }
    }
}
