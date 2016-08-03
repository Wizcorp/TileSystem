using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Management
{
	/// <summary>
	/// Used by the tiles to manage the entities being moved
	/// between them
	/// </summary>
	public interface IManageEntities
	{
		event EventHandler<EntityAddedArgs> EntityAdded;
		event EventHandler<EntityRemovedArgs> EntityRemoved;

		void Add(IEntity entity);
		void Remove(IEntity entity);
	}
}
