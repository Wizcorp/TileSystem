using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Creation
{
	/// <summary>
	/// ICreateTiles is used by the level to create tiles
	/// using a normal creational pattern
	/// 
	/// The area and position will be where the tile is created
	/// </summary>
	public interface ICreateTiles
	{
		event EventHandler<TileCreatedArgs> TileCreated;
		ITile CreateTile(IArea area, IPosition position, string type, string variation, params object[] properties);
		ITileFactory TileFactory { get; }
	}
}
