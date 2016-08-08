using System;
using System.Collections.Generic;

using TileSystem.Interfaces.Base;
using TileSystem.Interfaces.Management;
using TileSystem.Interfaces.TwoDimension;

namespace TileSystem.Implementation.TwoDimension
{
	/// <summary>
	/// Tile in 2D space, implementing ITile
	/// 
	/// Notes:
	/// Need to decide on the destroy event and clean up whether they should call
	/// one another or should be two function calls
	/// </summary>
	public class Tile : ITile
	{
		private List<IEntity> entities;

		// Destroyed event from ITile
		public event EventHandler<TileDestroyedArgs> Destroyed;

		// IManageEntities event handlers for entities
		public event EventHandler<EntityAddedArgs> EntityAdded;
		public event EventHandler<EntityRemovedArgs> EntityRemoved;

		// Representation in the system
		public string Type { get; protected set; }
		public string Variation { get; protected set; }

		// Location in the system
		public IArea Area { get; private set; }
		public IPosition2D Position { get; private set; }

		/// <summary>
		/// Default constructor sets up a list of IEntity
		/// </summary>
		public Tile()
		{
			entities = new List<IEntity>();
		}

		/// <summary>
		/// Constructor that is used by factory methods to set other properties
		/// type and variation are required
		/// </summary>
		/// <param name="type">The type of tile</param>
		/// <param name="variation">the variation on the type of tile</param>
		public Tile(string type, string variation) : this()
		{
			Type = type;
			Variation = variation;
		}

		/// <summary>
		/// Set position in the area of the tile
		/// </summary>
		/// <param name="area">Parent area</param>
		/// <param name="position">position in 2d</param>
		public void SetPosition(IArea area, IPosition2D position)
		{
			if (area == null)
			{
				throw new ArgumentNullException("area", "Area can not be null");
			}

			if (position == null)
			{
				throw new ArgumentNullException("position", "Position can not be null");
			}

			Area = area;
			Position = position;
		}

		/// <summary>
		/// Adds the entity to the entities list
		/// </summary>
		/// <param name="entity">Entity to add</param>
		public virtual void Add(IEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "Entities can not be null");
			}

			if (entities.Contains(entity))
			{
				throw new ArgumentException("Duplicate value", "entity");
			}

			entities.Add(entity);

			if (EntityAdded != null)
			{
				EntityAdded.Invoke(this, new EntityAddedArgs(entity));
			}
		}

		/// <summary>
		/// Removes the entity from the entities list
		/// </summary>
		/// <param name="entity">Entity to remove</param>
		/// <returns>true if an element is removed, false otherwise</returns>
		public virtual bool Remove(IEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity", "Entities can not be null");
			}

			bool removed = entities.Remove(entity);

			if (removed && EntityRemoved != null)
			{
				EntityRemoved.Invoke(this, new EntityRemovedArgs(entity));
			}

			return removed;
		}

		/// <summary>
		/// Destroy this tile and emit the event
		/// </summary>
		public virtual void Destroy(bool propagate = false)
		{
			if (propagate)
			{
				for (int i = entities.Count - 1; i >= 0; i--)
				{
					entities[i].Destroy();
				}
			}

			if (Destroyed != null)
			{
				Destroyed.Invoke(this, new TileDestroyedArgs());
			}
		}

		/// <summary>
		/// Override string for debug
		/// </summary>
		/// <returns>Formatted string representation of the Tile(X,Y, Entity Count)</returns>
		public override string ToString()
		{
			return string.Format("[Tile X:{0} Y:{1}, Entities Count:{2}]", Position.X, Position.Y, entities.Count);
		}
	}
}
