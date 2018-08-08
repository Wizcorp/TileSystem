using System;
using System.Collections.Generic;
using System.Linq;
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
	/// </summary>
	public class Area : IArea
	{
		// Position in 2d
		private IPosition2D position2d;

		// List of tiles this area contains
		private List<ITile> tiles;

		// Destroyed event from IArea
		public event EventHandler<AreaDestroyedArgs> Destroyed;
		
		// IManageTiles event handlers for tiles
		public event EventHandler<TileAddedArgs> TileAdded;
		public event EventHandler<TileRemovedArgs> TileRemoved;

		// Representation in the system
		public string Type { get; protected set; }
		public string Variation { get; protected set; }

		// Location in the system
		public ILevel Level { get; private set; }
		public IPosition Position
		{
			get { return position2d; }
		}

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
		public void SetPosition(ILevel level, IPosition position)
		{
			if (level == null)
			{
				throw new ArgumentNullException("level", "Level can not be null");
			}

			if (position == null)
			{
				throw new ArgumentNullException("position", "Position can not be null");
			}

			IPosition2D pos = position as IPosition2D;

			if (pos == null)
			{
				throw new ArgumentException("position must be of type IPosition2D", "position");
			}

			Level = level;
			position2d = pos;
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
            // TODO: Issue 6 (https://github.com/Wizcorp/TileSystem/issues/6)
            //Assuming position is IPosition2D
            var currentPosition = position as IPosition2D;

            return tiles?//Using Linq to simplify null check, foreach loop could be used instead.
                .FirstOrDefault(tile => 
                (tile.Position as IPosition2D).X == currentPosition.X
                && (tile.Position as IPosition2D).Y == currentPosition.Y);
		}

		/// <summary>
		/// Gets the neighbours of a tile
		/// </summary>
		/// <param name="tile">Tile to search around</param>
		/// <returns>List of neighbours or null</returns>
		public List<ITile> GetNeighbours(ITile tile)
		{
            // TODO: Issue 6 (https://github.com/Wizcorp/TileSystem/issues/6)
            List<ITile> result = new List<ITile>();

            //Assuming position is IPosition2D
            var currentTilePosition = tile.Position as IPosition2D;

            var maxRow = currentTilePosition.Y + 1;
            var minRow = currentTilePosition.Y - 1;

            var maxColumn = currentTilePosition.X + 1;            
            var minColumn = currentTilePosition.X - 1;
            
            for (int row = minRow; row <= maxRow; row++)
            {
                for (int column = minColumn; column <= maxColumn; column++)
                {
                    if (row == currentTilePosition.Y && column == currentTilePosition.X)
                        continue;

                    result.Add(this.Get(new Position2D(column, row)));
                }
            }
            return result.Any() ? result : null;
        }

		/// <summary>
		/// Destroy this area and emit the event
		/// </summary>
		public virtual void Destroy(bool propagate = false)
		{
			if (propagate)
			{
				for (int i = tiles.Count - 1; i >= 0; i--)
				{
					tiles[i].Destroy(propagate);
				}
			}

			if (Level != null)
			{
				Level.Remove(this);
			}

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
			return string.Format("[Area X:{0} Y:{1}, Tiles Count:{2}]", position2d.X, position2d.Y, tiles.Count);
		}
	}
}
