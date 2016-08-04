using System.Collections.Generic;

using TileSystem.Interfaces.Management;

namespace TileSystem.Interfaces.Base
{
	/// <summary>
	/// Specifies a level which is made up of one or more areas
	/// 
	/// Get and GetNeighbours are added so that the level can have any
	/// representation it wants to and the neighbours are managed by this
	/// </summary>
	public interface ILevel : IManageAreas
	{
		IArea Get(IPosition position);
		List<IArea> GetNeighbours(IArea tile);
	}
}
