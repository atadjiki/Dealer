using System.Collections.Generic;
using UnityEngine;
// Include the Pathfinding namespace to gain access to a lot of useful classes
using Pathfinding;
// Required to save the settings
using Pathfinding.Serialization;
using static Constants;
using Pathfinding.Drawing;
using System;

namespace Pathfinding
{
	[JsonOptIn]
	// Make sure the class is not stripped out when using code stripping (see https://docs.unity3d.com/Manual/ManagedCodeStripping.html)
	[Pathfinding.Util.Preserve]
	public class TileNode : PointNode
	{
        public EnvironmentLayer layer;

        private Dictionary<TileNode, MovementPathType> _transitions;

        public void Setup(Vector3 _position, EnvironmentLayer _layer, uint graphIndex)
        {
            // Node positions are stored as Int3. We can convert a Vector3 to an Int3 like this
            this.position = (Int3)_position;
            this.GraphIndex = graphIndex;
            layer = _layer;
            Tag = (uint)layer;
            Walkable = IsLayerWalkable(_layer);

            _transitions = new Dictionary<TileNode, MovementPathType>();

            connections = FindConnections();
        }

        private Connection[] FindConnections()
        {
            TileGraph graph = EnvironmentUtil.GetEnvironmentGraph();

            Vector3 origin = (Vector3)position;

            int Level = CalculateTileCoordinates(origin).z;

            List<Connection> connections = new List<Connection>();

            foreach (EnvironmentDirection dir in GetAllDirections())
            {
                TileConnectionInfo info = EnvironmentUtil.CheckNeighborConnection(origin, dir);

                Int3 coords = GetNeighboringTileCoordinates(origin, dir);

                if (graph.AreValidCoordinates(coords))
                {
                    TileNode neighbor = graph.GetNode(coords.x, coords.y, Level);

                    bool valid = info.IsValid() && graph.AllowedMovementTypes.HasFlag(MovementPathType.MOVE);

                    uint cost = GetDirectionCost(dir);

                    Connection connection = new Connection(neighbor, cost, valid, valid);

                    connections.Add(connection);
                }
            }

            return connections.ToArray();
        }

        public bool HasCoverInDirection(EnvironmentDirection dir)
        {
            TileConnectionInfo info = EnvironmentUtil.CheckNeighborConnection((Vector3)position, dir);

            return (IsLayerCover(info.Obstruction));
        }

        public EnvironmentCover GetCoverInDirection(EnvironmentDirection dir)
        {
            TileConnectionInfo info = EnvironmentUtil.CheckNeighborConnection((Vector3)position, dir);

            return GetCoverType(info);
        }

        public Dictionary<EnvironmentDirection, EnvironmentCover> GetCoverMap()
        {
            Dictionary<EnvironmentDirection, EnvironmentCover> map = new Dictionary<EnvironmentDirection, EnvironmentCover>();

            foreach (EnvironmentDirection dir in GetCardinalDirections())
            {
                if(HasCoverInDirection(dir))
                {
                    map.Add(dir, GetCoverInDirection(dir));
                }
            }

            return map;
        }

        public void AddTransition(TileNode node, MovementPathType type)
        {
            _transitions.Add(node, type);
        }

        public void RemoveTransition(TileNode node)
        {
            _transitions.Remove(node);
        }

        public bool HasTransitionTo(TileNode node)
        {
            return _transitions.ContainsKey(node);
        }

        public MovementPathType GetTransiton(TileNode node)
        {
            return _transitions[node];
        }

        public string GetInfo()
        {
            return
                position.ToString() + ",\n" +
                layer.ToString() + ",\n";
        }
    }
}

