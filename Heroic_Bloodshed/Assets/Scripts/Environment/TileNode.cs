using System.Collections.Generic;
using UnityEngine;
// Include the Pathfinding namespace to gain access to a lot of useful classes
using Pathfinding;
// Required to save the settings
using Pathfinding.Serialization;
using static Constants;
using Pathfinding.Drawing;
using System;

[Serializable]
public struct TIleInfo
{
	public EnvironmentLayer Layer;
}

namespace Pathfinding
{
	[JsonOptIn]
	// Make sure the class is not stripped out when using code stripping (see https://docs.unity3d.com/Manual/ManagedCodeStripping.html)
	[Pathfinding.Util.Preserve]
	public class TileNode : PointNode
	{
		public TIleInfo Info;

		public TileNode() { }
		public TileNode(AstarPath astar)
		{
			astar.InitializeNode(this);
		}
	}
}

