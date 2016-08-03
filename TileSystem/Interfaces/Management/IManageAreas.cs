using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Management
{
	/// <summary>
	/// 
	/// </summary>
	public interface IManageAreas
	{
		event EventHandler<AreaAddedArgs> AreaAdded;
		event EventHandler<AreaRemovedArgs> AreaRemoved;

		void Add(IArea area);
		void Remove(IArea area);
	}
}
