using System;

using TileSystem.Utils;
using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.TileChange
{
	/// <summary>
	/// When changing a tile there are three events that will emit this class
	/// IChangeTile: ChangeStarted, Changing, ChangeFinished
	/// 
	/// These can be used by the solvers to correctly filter events and operations
	/// </summary>
	public class TileChangedArgs : EventArgs
	{
		public ITile From { get; private set; }
		public ITile To { get; private set; }
		public TileChangeType Type { get; private set; }

		public TileChangedArgs(ITile from, ITile to, TileChangeType type)
		{
			From = from;
			To = to;
			Type = type;
		}

		public override string ToString()
		{
			return string.Format("[TileChangedArgs: From={0}, To={1}, Type={2}]", From, To, Type);
		}
	}
}
