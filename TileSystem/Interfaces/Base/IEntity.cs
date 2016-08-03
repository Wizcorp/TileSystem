using System;

namespace TileSystem.Interfaces.Base
{
	/// <summary>
	/// Specifies an entity which can exist in the tile system
	/// TODO: Expand the properties to be correct
	/// </summary>
	public interface IEntity
	{
		string Type { get; }
		string Variation { get; }

		event EventHandler<EntityDestroyedArgs> Destroyed;
		void Destroy();
		void CleanUp();
	}
}
