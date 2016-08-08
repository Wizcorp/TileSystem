using System;

using TileSystem.Interfaces.Base;

namespace TileSystem.Implementation.TwoDimension
{
	/// <summary>
	/// Entity inside the 2D system, can be used as a base class
	/// 
	/// Notes:
	/// Need to decide on the destroy event and clean up whether they should call
	/// one another or should be two function calls, much like Tile
	/// 
	/// Not sure if type and variation should be null checked
	/// </summary>
	public class Entity : IEntity
	{
		// Destroyed event from IEntity
		public event EventHandler<EntityDestroyedArgs> Destroyed;

		// Representation in the system
		public string Type { get; protected set; }
		public string Variation { get; protected set; }

		// Tile holding the Entity
		public ITile Tile { get; protected set;}

		/// <summary>
		/// Empty constructor to allow easy inheritance
		/// </summary>
		public Entity() { }

		/// <summary>
		/// Constructor that can set properties of the entity in the system
		/// </summary>
		/// <param name="type">Type of Entity</param>
		/// <param name="variation">Variation on type</param>
		public Entity(string type, string variation) : this()
		{
			Type = type;
			Variation = variation;
		}

		/// <summary>
		/// Set the parent tile for this entity
		/// </summary>
		/// <param name="tile">Parent Tile</param>
		public void SetParent(ITile tile)
		{
			if (tile == null)
			{
				throw new ArgumentNullException("tile", "Tile can not be null");
			}

			Tile = tile;
		}

		/// <summary>
		/// Clean up from containing Tile and call the destroy events
		/// </summary>
		public virtual void CleanUp()
		{
			if (Tile != null)
			{
				Tile.Remove(this);
			}

			// TODO: Is this necessary here?
			Destroy();
		}

		/// <summary>
		/// Destroy this entity and emit the event
		/// </summary>
		public virtual void Destroy()
		{
			if (Destroyed != null)
			{
				Destroyed.Invoke(this, new EntityDestroyedArgs(this));
			}
		}
	}
}
