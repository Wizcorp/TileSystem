using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Management
{
	/// <summary>
	/// Emited when an area is added to an object implementing
	/// IManageAreas interface
	/// </summary>
	public class AreaAddedArgs : EventArgs
	{
		public IArea Area { get; private set; }

		public AreaAddedArgs(IArea area)
		{
			Area = area;
		}

		public override string ToString()
		{
			return string.Format("[AreaAddedArgs: Area={0}]", Area);
		}
	}
}
