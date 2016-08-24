using System;

namespace TileSystem.Interfaces.Base
{
	/// <summary>
	/// Specifies an entity which can exist in the tile system
	/// 
	/// Notes:
	/// Type and Variation are used for the factory functions
	/// 
	/// The Tile is a reference to the containing parent for fast traversing
	/// 
	/// Set Tile sets the tile that the entity is attached to
	/// 
	/// The Destroyed, Destroy and CleanUp are management functions so you can cleanup
	/// without triggering the event, much like destroy immediate in Unity
	/// </summary>
	public interface IEntity
	{
		string Type { get; }
		string Variation { get; }

		ITile Tile { get; }
		void SetTile(ITile tile);

		event EventHandler<EntityDestroyedArgs> Destroyed;
		void Destroy();
	}
}
