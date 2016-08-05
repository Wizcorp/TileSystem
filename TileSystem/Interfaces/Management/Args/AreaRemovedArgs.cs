using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Management
{
	/// <summary>
	/// Emited when an area is removed from an object implementing
	/// IManageAreas interface
	/// </summary>
	public class AreaRemovedArgs : EventArgs
	{
		public IArea Area { get; private set; }

		public AreaRemovedArgs(IArea area)
		{
			Area = area;
		}

		public override string ToString()
		{
			return string.Format("[AreaRemovedArgs: Area={0}]", Area);
		}
	}
}
