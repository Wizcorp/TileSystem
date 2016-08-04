using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Creation
{
	/// <summary>
	/// Factory for areae creation, defined by the framework to be easily linked
	/// to the ICreateAreas interface 
	/// </summary>
	public interface IAreaFactory
	{
		IArea CreateArea(string type, string variation, params object[] properties);
	}
}
