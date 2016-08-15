using System;

using TileSystem.Interfaces.Base;
using TileSystem.Interfaces.TwoDimension;

namespace TileSystem.Implementation.TwoDimension
{
	/// <summary>
	/// Simple X,Y position in 2d space with compare implementation
	/// </summary>
	public class Position2D : IPosition2D
	{
		public int X { get; private set; }
		public int Y { get; private set; }

		public Position2D(int x, int y)
		{
			X = x;
			Y = y;
		}

		// IComparable
		public int CompareTo(IPosition other)
		{
			IPosition2D other2d = other as IPosition2D;

			if (other2d == null)
			{
				throw new ArgumentException("other has to be of type IPosition2D", "other");
			}

			if (other2d.X == X && other2d.Y == Y)
			{
				return 0;
			}

			// TODO: Issue 11 (https://github.com/Wizcorp/TileSystem/issues/11)
			return -1;
		}

		/// <summary>
		/// Override string for debug
		/// </summary>
		/// <returns>String representation (X,Y)</returns>
		public override string ToString()
		{
			return string.Format("[Position X:{0} Y:{1}]", X, Y);
		}
	}
}
