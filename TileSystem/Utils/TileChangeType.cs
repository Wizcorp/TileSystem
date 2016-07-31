namespace TileSystem.Utils
{
	/// <summary>
	/// When an entity is changing tile this is the change type
	/// that is emitted with the event arguments and is used by the solvers
	/// </summary>
	public enum TileChangeType
	{
		Start,
		Change,
		Finish
	}
}
