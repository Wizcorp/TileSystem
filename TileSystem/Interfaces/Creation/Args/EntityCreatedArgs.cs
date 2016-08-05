using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Creation
{
	/// <summary>
	/// Emited when an entity is created by an object implementing
	/// ICreateEntities interface
	/// </summary>
	public class EntityCreatedArgs : EventArgs
	{
		public ITile Tile { get; private set; }
		public IEntity Entity { get; private set; }

		public EntityCreatedArgs(ITile tile, IEntity entity)
		{
			Tile = tile;
			Entity = entity;
		}

		public override string ToString()
		{
			return string.Format("[EntityCreatedArgs: Tile={0}, Entity={1}]", Tile, Entity);
		}
	}
}
