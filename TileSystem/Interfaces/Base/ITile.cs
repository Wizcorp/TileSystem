using System;

using TileSystem.Interfaces.Management;

namespace TileSystem.Interfaces.Base
{
	/// <summary>
	/// Specifies a single tile in the system
	/// TODO: Expand the properties to be correct
	/// 
	/// Notes:
	/// Destroy is to tell the game that the entity was destroyed, we use this to only
	/// remove only this tile.  This should be used to emit the event to trigger animations in
	/// the UI.
	/// 
	/// It is separate to the clean up so that we can remove everything from the tile which can be
	/// used to not trigger the destroy event.
	/// 
	/// The Area is a reference to the containing parent for fast traversing
	/// </summary>
	public interface ITile : IManageEntities
	{
		string Type { get; }
		string Variation { get; }

		IArea Area { get; }

		event EventHandler<TileDestroyedArgs> Destroyed;
		void Destroy(bool propagate = false);
	}
}
