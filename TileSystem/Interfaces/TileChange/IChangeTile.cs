using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.TileChange
{
	/// <summary>
	/// Main tile change interface
	/// 
	/// Allows an entity in the level to emit events to the system which will be
	/// passed to the solvers
	/// </summary>
	public interface IChangeTile
	{
		event EventHandler<TileChangedArgs> ChangeStarted;
		event EventHandler<TileChangedArgs> Changing;
		event EventHandler<TileChangedArgs> ChangeFinished;

		void StartChangeTile(ITile from, ITile to);
		void ChangeTile(ITile from, ITile to);
		void FinishChangeTile(ITile from, ITile to);
	}
}
