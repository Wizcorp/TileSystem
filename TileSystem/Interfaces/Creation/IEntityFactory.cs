using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Creation
{
	/// <summary>
	/// Factory for entity creation, defined by the framework to be easily linked
	/// to the ICreateEntities interface
	/// </summary>
	public interface IEntityFactory
	{
		IEntity CreateEntity(string type, string variation, params object[] properties);
	}
}
