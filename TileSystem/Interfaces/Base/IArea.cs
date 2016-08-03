using System;
using System.Collections.Generic;

using TileSystem.Interfaces.Management;

namespace TileSystem.Interfaces.Base
{
	/// <summary>
	/// Specifies a group of tiles that make up an area in the level
	/// TODO: Expand the properties to be correct
	/// </summary>
	public interface IArea : IManageTiles
	{
		ITile Get(IPosition position);
		List<ITile> GetNeighbours(ITile tile);

		ILevel Level { get; }

		event EventHandler<AreaDestroyedArgs> Destroyed;
		void Destroy();
		void CleanUp();
	}
}
