using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Management
{
	/// <summary>
	/// Emited when a tile is added to an object implementing
	/// IManageTiles interface
	/// </summary>
	public class TileAddedArgs : EventArgs
	{
		public ITile Tile { get; private set; }

		public TileAddedArgs(ITile tile)
		{
			Tile = tile;
		}

		public override string ToString()
		{
			return string.Format("[TileAddedArgs: Tile={0}]", Tile);
		}
	}
}
