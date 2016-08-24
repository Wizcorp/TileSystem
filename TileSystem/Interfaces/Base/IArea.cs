using System;
using System.Collections.Generic;

using TileSystem.Interfaces.Management;

namespace TileSystem.Interfaces.Base
{
	/// <summary>
	/// Specifies a group of tiles that make up an area in the level
	/// IManageTiles is a requirement to make sure we have some data structure
	/// underneath the area for Tile management
	/// 
	/// Notes:
	/// Type and Variation are used for the factory functions
	/// 
	/// Get and GetNeighbours are added so that the area can have any
	/// representation it wants to and the neighbours are managed by this
	/// 
	/// The Level is a reference to the containing parent for fast traversing
	/// 
	/// The Destroyed, Destroy and CleanUp are management functions so you can cleanup
	/// without triggering the event, much like destroy immediate in Unity
	/// </summary>
	public interface IArea : IManageTiles
	{
		string Type { get; }
		string Variation { get; }

		ITile Get(IPosition position);
		List<ITile> GetNeighbours(ITile tile);

		ILevel Level { get; }
		IPosition Position { get; }
		void SetPosition(ILevel level, IPosition position);

		event EventHandler<AreaDestroyedArgs> Destroyed;
		void Destroy(bool propagate = false);
	}
}
