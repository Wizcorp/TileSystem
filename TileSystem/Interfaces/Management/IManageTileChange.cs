using TileSystem.Interfaces.Creation;
using TileSystem.Interfaces.Solvers;

namespace TileSystem.Interfaces.Management
{
	/// <summary>
	/// IManageTileChange ties the solvers and the creators together
	/// when an entity is created in the system it allows for this
	/// interface to pick up the event and register any entity that 
	/// needs to be passed to the solvers.  You can add and remove solvers
	/// for these entities via the Add and Remove functions.
	/// </summary>
	public interface IManageTileChange
	{
		void Add(ISolver solver);
		bool Remove(ISolver solver);

		void RegisterEntityCreator(ICreateEntities creator);
		bool DeregisterEntityCreator(ICreateEntities creator);
	}
}
