using System.Collections.Generic;

using TileSystem.Interfaces.Management;

namespace TileSystem.Interfaces.Base
{
	/// <summary>
	/// Specifies a level which is made up of one or more areas
	/// TODO: Expand the properties to be correct
	/// </summary>
	public interface ILevel : IManageAreas
	{
		IArea Get(IPosition position);
		List<IArea> GetNeighbours(IArea tile);
	}
}
