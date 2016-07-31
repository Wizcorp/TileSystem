using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Creation
{
	/// <summary>
	/// Factory for tile creation, defined by the framework to be easily linked
	/// to the ICreateTiles interface
	/// </summary>
	public interface ITileFactory
	{
		ITile CreateTile(string type, string variation, params object[] properties);
	}
}
