using TileSystem.Interfaces.Base;

namespace TileSystem.Interfaces.Creation
{
	/// <summary>
	/// 
	/// </summary>
	public interface IAreaFactory
	{
		IArea CreateArea(string type, string variation, params object[] properties);
	}
}
