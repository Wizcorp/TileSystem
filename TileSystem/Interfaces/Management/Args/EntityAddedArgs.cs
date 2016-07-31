using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Management
{
	/// <summary>
	/// Emited when an entity is added to an object implementing
	/// IManageEntities interface
	/// </summary>
	public class EntityAddedArgs : EventArgs
	{
		public IEntity Entity { get; private set; }

		public EntityAddedArgs(IEntity entity)
		{
			Entity = entity;
		}

		public override string ToString()
		{
			return string.Format("[EntityAddedArgs: Entity={0}]", Entity);
		}
	}
}
