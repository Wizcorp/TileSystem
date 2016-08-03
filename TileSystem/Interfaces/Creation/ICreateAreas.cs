using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Creation
{
	/// <summary>
	/// 
	/// </summary>
	public interface ICreateAreas
	{
		event EventHandler<EntityCreatedArgs> EntityCreated;
		IArea CreateArea(ILevel level, string type, string variation, params object[] properties);
		IAreaFactory AreaFactory { get; }
	}
}
