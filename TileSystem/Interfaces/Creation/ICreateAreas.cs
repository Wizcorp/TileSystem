using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Creation
{
	/// <summary>
	/// ICreateAreas is used to create areas
	/// using a normal creational pattern
	/// 
	/// The level and position will determine where the area is created
	/// </summary>
	public interface ICreateAreas
	{
		event EventHandler<AreaCreatedArgs> AreaCreated;
		IArea CreateArea(ILevel level, IPosition position, string type, string variation, params object[] properties);
		IAreaFactory AreaFactory { get; }
	}
}
