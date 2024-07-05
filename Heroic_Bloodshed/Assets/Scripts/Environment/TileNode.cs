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

        private Dictionary<TileNode, MovementType> _transitions;

        private Dictionary<EnvironmentDirection, TileNode> _neighborMap;

        private TileCoordinates _coords;

        public void Setup(TileCoordinates coordinates, uint graphIndex)
        {
            _coords = coordinates;

            Vector3 origin = _coords.GetOrigin();

            // Node positions are stored as Int3. We can convert a Vector3 to an Int3 like this
            this.position = (Int3) origin;
            this.GraphIndex = graphIndex;
            layer = EnvironmentUtil.CheckTileLayer(origin);
            Tag = (uint)layer;
            Walkable = IsLayerWalkable(layer);

            _transitions = new Dictionary<TileNode, MovementType>();
            _neighborMap = new Dictionary<EnvironmentDirection, TileNode>();
        }

        public void FindNodeConnections(TileGraph graph)
        {
            Vector3 origin = _coords.GetOrigin();

            int Level = _coords.Level;

            List<Connection> found = new List<Connection>();

            foreach (EnvironmentDirection dir in GetAllDirections())
            {
                TileConnectionInfo info = EnvironmentUtil.CheckNeighborConnection(origin, dir);

                Int3 coords = GetNeighboringTileCoordinates(origin, dir);

                if (graph.AreValidCoordinates(coords))
                {
                    TileNode neighbor = graph.GetNode(coords.x, coords.y, Level);

                    bool valid = info.IsValid() && graph.AllowedMovementTypes.HasFlag(MovementType.MOVE);

                    uint cost = GetDirectionCost(dir);

                    Connection connection = new Connection(neighbor, cost, valid, valid);

                    _neighborMap.Add(dir, neighbor);
                    found.Add(connection);
                }
            }

            this.connections = found.ToArray();
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

        public void AddTransition(TileNode node, MovementType type)
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

        public MovementType GetTransiton(TileNode node)
        {
            return _transitions[node];
        }

        public TileCoordinates GetCoordinates()
        {
            return _coords;
        }

        public Vector3 GetOrigin()
        {
            return _coords.GetOrigin();
        }

        public int GetLevel()
        {
            return _coords.Level;
        }

        public bool GetNeighborInDirection(EnvironmentDirection dir, out TileNode neighbor)
        {
            if(_neighborMap.ContainsKey(dir))
            {
                neighbor = _neighborMap[dir];
                return true;
            }

            neighbor = null;
            return false;
        }

        public bool HasNeighborInDirection(EnvironmentDirection dir)
        {
            return _neighborMap.ContainsKey(dir);
        }

        public string GetInfo()
        {
            return
                position.ToString() + ",\n" +
                layer.ToString() + ",\n";
        }
    }
}

