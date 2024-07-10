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

        private Dictionary<EnvironmentDirection, TileConnectionInfo> _neighborMap;

        private TileCoordinates _coords;

        private Vector3 _truePosition; //raycasted position 

        public void Setup(TileCoordinates coordinates, uint graphIndex)
        {
            _coords = coordinates;

            Vector3 origin = _coords.GetOrigin();

            TileRaycastInfo raycastInfo = EnvironmentUtil.RaycastForTile(origin);

            // Node positions are stored as Int3. We can convert a Vector3 to an Int3 like this
            this.position = (Int3)origin; //the true position of the tile in the world. 
            _truePosition = raycastInfo.HitInfo.point;
            this.GraphIndex = graphIndex;
            layer = raycastInfo.Layer;
            Tag = (uint)layer;
            Walkable = IsLayerWalkable(layer);

            _transitions = new Dictionary<TileNode, MovementType>();
            _neighborMap = new Dictionary<EnvironmentDirection, TileConnectionInfo>();
        }

        public void FindNodeConnections(TileGraph graph)
        {
            Vector3 origin = _coords.GetOrigin();

            int Level = _coords.Level;

            List<Connection> found = new List<Connection>();

            foreach (EnvironmentDirection dir in GetAllDirections())
            {
                TileConnectionInfo info = CheckNeighborConnection(dir);

                Int3 coords = GetNeighboringTileCoordinates(origin, dir);

                if (graph.AreValidCoordinates(coords))
                {
                    TileNode neighbor = graph.GetNode(coords.x, coords.y, Level);
                    info.Node = neighbor; //important!

                    bool valid = info.IsValid() && graph.AllowedMovementTypes.HasFlag(MovementType.MOVE);

                    uint cost = GetDirectionCost(dir);

                    Connection connection = new Connection(neighbor, cost, valid, valid);

                    _neighborMap.Add(dir, info);
                    found.Add(connection);
                }
            }

            connections = found.ToArray();
        }

        public void FindNodeTransitions(TileGraph graph)
        {
            foreach (EnvironmentDirection dir in GetCardinalDirections())
            {
                TileConnectionInfo neighborInfo;
                if (GetNeighborInfo(dir, out neighborInfo))
                {
                    TileNode neighbor = neighborInfo.Node;

                    //check for jumps over half obstacles
                    if (!neighborInfo.IsWallBetween() && graph.AllowedMovementTypes.HasFlag(MovementType.VAULT_OBSTACLE))
                    {
                        if (GetCoverType(neighborInfo) == EnvironmentCover.HALF)
                        {
                            if (!neighborInfo.IsValid())
                            {
                                //get the node after this node
                                TileConnectionInfo nextInfo;
                                if (neighbor.GetNeighborInfo(dir, out nextInfo))
                                {
                                    TileNode next = nextInfo.Node;

                                    if (next.Walkable)
                                    {
                                        var cost = GetDirectionCost(dir);

                                        AddPartialConnection(next, cost, true, true);

                                        AddTransition(next, MovementType.VAULT_OBSTACLE);
                                    }
                                }
                            }
                        }
                    }
                    //check for wall vaults
                    else if (neighborInfo.IsLayerBetween(EnvironmentLayer.WALL_HALF) && graph.AllowedMovementTypes.HasFlag(MovementType.VAULT_WALL))
                    {
                        if (!neighborInfo.IsValid())
                        {
                            uint cost = GetDirectionCost(dir);

                            AddPartialConnection(neighbor, cost, true, true);

                            AddTransition(neighbor, MovementType.VAULT_WALL);
                        }
                    }
                }
            }
        }

        public bool HasCoverInDirection(EnvironmentDirection dir)
        {
            TileConnectionInfo info = _neighborMap[dir];

            return (IsLayerCover(info.Obstruction));
        }

        public EnvironmentCover GetCoverInDirection(EnvironmentDirection dir)
        {
            TileConnectionInfo info = _neighborMap[dir];

            return GetCoverType(info);
        }

        private TileConnectionInfo CheckNeighborConnection(EnvironmentDirection dir)
        {
            Vector3 direction = GetDirectionVector(dir);
            Vector3 neighborOrigin = GetNeighboringTileLocation(GetGridPosition(), dir);

            TileConnectionInfo info = TileConnectionInfo.Build();
            TileRaycastInfo raycastInfo  = EnvironmentUtil.RaycastForTile(neighborOrigin);

            info.Layer = raycastInfo.Layer;

            Vector3 offset = new Vector3(0, ENV_TILE_SIZE / 2, 0);

            //now check that nothing is in the way between this tile and its neighbor (like walls or corners)
            Ray ray = new Ray(GetGridPosition() + offset, direction);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, direction.magnitude))
            {
                if (hitInfo.collider != null)
                {
                    int layerMask = hitInfo.collider.gameObject.layer;

                    info.Obstruction = GetLayer(layerMask);
                }
            }

            return info;
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

        public Vector3 GetGridPosition()
        {
            return _coords.GetOrigin();
        }

        public Vector3 GetTruePosition()
        {
            return _truePosition;
        }

        public int GetLevel()
        {
            return _coords.Level;
        }

        public bool GetNeighborInfo(EnvironmentDirection dir, out TileConnectionInfo neighbor)
        {
            if(_neighborMap.ContainsKey(dir))
            {
                neighbor = _neighborMap[dir];
                return true;
            }

            neighbor = new TileConnectionInfo();
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

