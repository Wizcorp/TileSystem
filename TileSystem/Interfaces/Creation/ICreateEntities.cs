using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Creation
{
	/// <summary>
	/// ICreateEntities is used to create entities
	/// using a normal creational pattern
	/// 
	/// The tile will be where the entity starts in the level
	/// </summary>
	public interface ICreateEntities
	{
		event EventHandler<EntityCreatedArgs> EntityCreated;
		IEntity CreateEntity(ITile tile, string type, string variation, params object[] properties);
		IEntityFactory EntityFactory { get; }
	}
}
