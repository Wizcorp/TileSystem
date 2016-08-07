using System;

namespace TileSystem.Interfaces.Base
{
	/// <summary>
	/// Entity that was destroyed is emited with this event
	/// Used by IEntity Destroy event
	/// </summary>
	public class EntityDestroyedArgs : EventArgs
	{
		public IEntity Entity { get; private set; }

		public EntityDestroyedArgs(IEntity entity)
		{
			Entity = entity;
		}
	}
}
