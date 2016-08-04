using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Creation
{
	/// <summary>
	/// ICreateAreas is used by the level to create entities
	/// using a normal creational pattern
	/// 
	/// The level and position will determine where the area is created
	/// </summary>
	public interface ICreateAreas
	{
		event EventHandler<EntityCreatedArgs> EntityCreated;
		IArea CreateArea(ILevel level, IPosition position, string type, string variation, params object[] properties);
		IAreaFactory AreaFactory { get; }
	}
}
