using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.TwoDimension
{
	public interface IPosition2D : IPosition
	{
		int X { get; }
		int Y { get; }
	}
}
