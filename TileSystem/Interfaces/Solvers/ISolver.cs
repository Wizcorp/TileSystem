using TileSystem.Interfaces.Base;
using TileSystem.Interfaces.TileChange;

namespace TileSystem.Interfaces.Solvers
{
	/// <summary>
	/// The solver interface this allows the definition of a component that will
	/// solve the movement of an entity (IEntity) between two tiles (ITile), 
	/// passed to it by TileChangedArgs
	/// </summary>
	public interface ISolver
	{
		bool Solve(IEntity entity, TileChangedArgs e);
	}
}
