using System;

namespace TileSystem.Interfaces.Base
{
	/// <summary>
	/// Currently this is empty because there are no references required for
	/// destroy in the tile system that you do not have a reference to from the
	/// tile you are registered to
	/// </summary>
	public class TileDestroyedArgs : EventArgs
	{
	}
}
