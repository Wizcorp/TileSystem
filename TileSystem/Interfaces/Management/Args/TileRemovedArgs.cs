using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Management
{
	/// <summary>
	/// Emited when a tile is removed from an object implementing
	/// IManageTiles interface
	/// </summary>
	public class TileRemovedArgs : EventArgs
	{
		public ITile Tile { get; private set; }

		public TileRemovedArgs(ITile tile)
		{
			Tile = tile;
		}

		public override string ToString()
		{
			return string.Format("[TileRemovedArgs: Tile={0}]", Tile);
		}
	}
}
