using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Management
{
	/// <summary>
	/// Emited when an entity is removed from an object implementing
	/// IManageEntities interface
	/// </summary>
	public class EntityRemovedArgs : EventArgs
	{
		public IEntity Entity { get; private set; }

		public EntityRemovedArgs(IEntity entity)
		{
			Entity = entity;
		}

		public override string ToString()
		{
			return string.Format("[EntityRemovedArgs: Entity={0}]", Entity);
		}
	}
}
