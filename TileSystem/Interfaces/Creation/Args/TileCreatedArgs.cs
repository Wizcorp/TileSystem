using System;
using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Creation
{
	/// <summary>
	/// Emited when a tile is created by an object implementing
	/// ICreateTiles interface
	/// </summary>
	public class TileCreatedArgs : EventArgs
	{
		public IArea Area { get; private set; }
		public ITile Tile { get; private set; }

		public TileCreatedArgs(IArea area, ITile tile)
		{
			Area = area;
			Tile = tile;
		}

		public override string ToString()
		{
			return string.Format("[TileCreatedArgs: Area={0}, Tile={1}]", Area, Tile);
		}
	}
}
