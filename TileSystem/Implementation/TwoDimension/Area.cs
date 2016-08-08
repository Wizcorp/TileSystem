using System;
using System.Collections.Generic;

using TileSystem.Interfaces.Base;
using TileSystem.Interfaces.Management;
using TileSystem.Interfaces.TwoDimension;

namespace TileSystem.Implementation.TwoDimension
{
	/// <summary>
	/// Area in 2D space, implementing IArea
	/// 
	/// Notes:
	/// Same as Tile, need to decide on the destroy event and clean up whether they should call
	/// one another or should be two function calls
	/// 
	/// TODO: PositionGet and PositionGetNeighbours need to be implemented
	/// </summary>
	public class Area : IArea
	{
		private List<ITile> tiles;

		// Destroyed event from IArea
		public event EventHandler<AreaDestroyedArgs> Destroyed;
		
		// IManageTiles event handlers for tiles
		public event EventHandler<TileAddedArgs> TileAdded;
		public event EventHandler<TileRemovedArgs> TileRemoved;

		// Representation in the system
		public string Type { get; private set; }
		public string Variation { get; private set; }

		// Location in the system
		public ILevel Level { get; private set; }
		public IPosition2D Position { get; private set; }

		/// <summary>
		/// Default constructor sets up a list of ITile
		/// </summary>
		public Area()
		{
			tiles = new List<ITile>();
		}

		/// <summary>
		/// Constructor that is used by factory methods to set other properties
		/// type and variation are required
		/// </summary>
		/// <param name="type">The type of area</param>
		/// <param name="variation">the variation on the type of area</param>
		public Area(string type, string variation) : this()
		{
			Type = type;
			Variation = variation;
		}

		/// <summary>
		/// Set position in the level of the area
		/// </summary>
		/// <param name="level">Parent level</param>
		/// <param name="position">position in 2d</param>
		public void SetPosition(ILevel level, IPosition2D position)
		{
			if (level == null)
			{
				throw new ArgumentNullException("level", "Level can not be null");
			}

			if (position == null)
			{
				throw new ArgumentNullException("position", "Position can not be null");
			}

			Level = level;
			Position = position;
		}

		/// <summary>
		/// Adds the tile to the tiles list
		/// </summary>
		/// <param name="tile">Tile to add</param>
		public virtual void Add(ITile tile)
		{
			if (tile == null)
			{
				throw new ArgumentNullException("tile", "Tiles can not be null");
			}

			if (tiles.Contains(tile))
			{
				throw new ArgumentException("Duplicate value", "tile");
			}

			tiles.Add(tile);

			if (TileAdded != null)
			{
				TileAdded.Invoke(this, new TileAddedArgs(tile));
			}
		}

		/// <summary>
		/// Removes the tile from the tiles list
		/// </summary>
		/// <param name="tile">Tile to remove</param>
		/// <returns>true if an element is removed, false otherwise</returns>
		public virtual bool Remove(ITile tile)
		{
			if (tile == null)
			{
				throw new ArgumentNullException("tile", "Tiles can not be null");
			}

			bool removed = tiles.Remove(tile);

			if (removed && TileRemoved != null)
			{
				TileRemoved.Invoke(this, new TileRemovedArgs(tile));
			}

			return removed;
		}

		/// <summary>
		/// Gets the tile at this position
		/// </summary>
		/// <param name="position"></param>
		/// <returns>Tile instance or null</returns>
		public ITile Get(IPosition position)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the neighbours of a tile
		/// </summary>
		/// <param name="tile">Tile to search around</param>
		/// <returns>List of neighbours or null</returns>
		public List<ITile> GetNeighbours(ITile tile)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Clean up all the tiles and call the destroy events
		/// </summary>
		public virtual void CleanUp()
		{
			for (int i = tiles.Count - 1; i >= 0; i--)
			{
				tiles[i].CleanUp();
			}

			// TODO: Is this necessary here?
			Destroy();
		}

		/// <summary>
		/// Destroy this area and emit the event
		/// </summary>
		public virtual void Destroy()
		{
			if (Destroyed != null)
			{
				Destroyed.Invoke(this, new AreaDestroyedArgs());
			}
		}

		/// <summary>
		/// Override string for debug
		/// </summary>
		/// <returns>Formatted string representation of the Area(X,Y, Tile Count)</returns>
		public override string ToString()
		{
			return string.Format("[Area X:{0} Y:{1}, Tiles Count:{2}]", Position.X, Position.Y, tiles.Count);
		}
	}
}
