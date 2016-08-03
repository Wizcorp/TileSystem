using System;

namespace TileSystem.Interfaces.Base
{
	/// <summary>
	/// Specifies a comparable base interface for positional data
	/// inside the tile system
	/// </summary>
	public interface IPosition : IComparable<IPosition>
	{
	}
}
