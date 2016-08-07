using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Management
{
	/// <summary>
	/// Used to manage the areas in the level / world
	/// </summary>
	public interface IManageAreas
	{
		event EventHandler<AreaAddedArgs> AreaAdded;
		event EventHandler<AreaRemovedArgs> AreaRemoved;

		void Add(IArea area);
		bool Remove(IArea area);
	}
}
