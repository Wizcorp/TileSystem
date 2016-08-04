using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Management
{
	/// <summary>
	/// Used to manage the tiles in the area / world
	/// </summary>
	public interface IManageTiles
	{
		event EventHandler<TileAddedArgs> TileAdded;
		event EventHandler<TileRemovedArgs> TileRemoved;

		void Add(ITile tile);
		void Remove(ITile tile);
	}
}
