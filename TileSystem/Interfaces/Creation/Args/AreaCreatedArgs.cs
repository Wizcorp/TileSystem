using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Creation
{
	/// <summary>
	/// Emited when an area is created by an object implementing
	/// ICreateAreas interface
	/// </summary>
	public class AreaCreatedArgs : EventArgs
	{
		public ILevel Level { get; private set; }
		public IArea Area { get; private set; }

		public AreaCreatedArgs(ILevel level, IArea area)
		{
			Level = level;
			Area = area;
		}

		public override string ToString()
		{
			return string.Format("[AreaCreatedArgs: Level={0}, Area={1}]", Level, Area);
		}
	}
}
