using System;
using System.Collections.Generic;

using TileSystem.Interfaces.Base;
using TileSystem.Interfaces.Creation;
using TileSystem.Interfaces.Management;
using TileSystem.Interfaces.TwoDimension;

namespace TileSystem.Implementation.TwoDimension
{
	/// <summary>
	/// Level is the main container for our 2d space, it allows
	/// for addition of areas and controls the creation of
	/// areas, tiles and entities.
	/// 
	/// Notes:
	/// We provide a 2d implementation in this format but this does
	/// not have to be used in the same way, all the ICreateX interfaces
	/// are designed to be placed anywhere in the structure, however the
	/// ILevel, IArea, ITile, IEntity hierarchy is fixed.
	/// </summary>
	public class Level : ILevel, ICreateAreas, ICreateTiles, ICreateEntities
	{
		// List of areas in the system
		private List<IArea> areas;

		// Area Events
		public event EventHandler<AreaAddedArgs> AreaAdded;
		public event EventHandler<AreaRemovedArgs> AreaRemoved;

		// Creation Events
		public event EventHandler<AreaCreatedArgs> AreaCreated;
		public event EventHandler<TileCreatedArgs> TileCreated;
		public event EventHandler<EntityCreatedArgs> EntityCreated;

		// Factories
		public IAreaFactory AreaFactory { get; protected set; }
		public ITileFactory TileFactory { get; protected set; }
		public IEntityFactory EntityFactory { get; protected set; }

		/// <summary>
		/// Default constructor sets up a list of IAreas
		/// </summary>
		public Level()
		{
			areas = new List<IArea>();
		}

		/// <summary>
		/// Constructor to allow inject of factories that are used
		/// with the creation of Areas, Tiles, and Entities
		/// </summary>
		/// <param name="areaFactory">Area factory instance</param>
		/// <param name="tileFactory">Tile factory instance</param>
		/// <param name="entityFactory">Entity factory instance</param>
		public Level(IAreaFactory areaFactory, ITileFactory tileFactory, IEntityFactory entityFactory) : this()
		{
			if (areaFactory == null)
			{
				throw new ArgumentNullException("areaFactory", "Area Factory can not be null");
			}

			if (tileFactory == null)
			{
				throw new ArgumentNullException("tileFactory", "Tile Factory can not be null");
			}

			if (entityFactory == null)
			{
				throw new ArgumentNullException("entityFactory", "Entity Factory can not be null");
			}

			AreaFactory = areaFactory;
			TileFactory = tileFactory;
			EntityFactory = entityFactory;
		}

		#region Creation Methods

		/// <summary>
		/// Create an area in the level specified, with the type and variation
		/// </summary>
		/// <param name="level">Level to add the area to</param>
		/// <param name="position">Position to add the area in</param>
		/// <param name="type">Type of the area</param>
		/// <param name="variation">Variation of the type</param>
		/// <param name="properties">Instantiation parameters</param>
		/// <returns>Reference to the area created</returns>
		public IArea CreateArea(ILevel level, IPosition position, string type, string variation, params object[] properties)
		{
			if (level == null)
			{
				throw new ArgumentNullException("level", "level can not be null");
			}

			if (position == null)
			{
				throw new ArgumentNullException("position", "position can not be null");
			}

			IArea area = AreaFactory.CreateArea(type, variation, properties);

			area.SetPosition(level, position);

			level.Add(area);

			if (AreaCreated != null)
			{
				AreaCreated.Invoke(this, new AreaCreatedArgs(level, area));
			}

			return area;
		}

		/// <summary>
		/// Create a tile in the area specified, with the type and variatiion
		/// </summary>
		/// <param name="area">Area to add the tile to</param>
		/// <param name="position">Position to add the tile in</param>
		/// <param name="type">Type of the tile</param>
		/// <param name="variation">Variation of the type</param>
		/// <param name="properties">Instantiation parameters</param>
		/// <returns>Reference to the tile created</returns>
		public ITile CreateTile(IArea area, IPosition position, string type, string variation, params object[] properties)
		{
			if (area == null)
			{
				throw new ArgumentNullException("area", "area can not be null");
			}

			if (position == null)
			{
				throw new ArgumentNullException("position", "position can not be null");
			}

			ITile tile = TileFactory.CreateTile(type, variation, properties);

			tile.SetPosition(area, position);

			area.Add(tile);

			if (TileCreated != null)
			{
				TileCreated.Invoke(this, new TileCreatedArgs(area, tile));
			}

			return tile;
		}

		/// <summary>
		/// Create an entity on the tile specified, with the type and variation
		/// </summary>
		/// <param name="tile">Tile to create the entity on</param>
		/// <param name="type">Type of the entity</param>
		/// <param name="variation">Variation of the type</param>
		/// <param name="properties">Instantiation parameters</param>
		/// <returns>Reference to the entity created</returns>
		public IEntity CreateEntity(ITile tile, string type, string variation, params object[] properties)
		{
			if (tile == null)
			{
				throw new ArgumentNullException("tile", "tile can not be null");
			}

			IEntity entity = EntityFactory.CreateEntity(type, variation, properties);

			entity.SetTile(tile);

			tile.Add(entity);

			if (EntityCreated != null)
			{
				EntityCreated.Invoke(this, new EntityCreatedArgs(tile, entity));
			}

			return entity;
		}

		#endregion

		#region Area Methods

		/// <summary>
		/// Add area to the current level
		/// </summary>
		/// <param name="area">Area you are adding</param>
		public void Add(IArea area)
		{
			if (area == null)
			{
				throw new ArgumentNullException("area", "Areas can not be null");
			}

			if (areas.Contains(area))
			{
				throw new ArgumentException("Duplicate value", "area");
			}

			areas.Add(area);

			if (AreaAdded != null)
			{
				AreaAdded.Invoke(this, new AreaAddedArgs(area));
			}
		}

		/// <summary>
		/// Remove area from the current level
		/// </summary>
		/// <param name="area">Area to remove</param>
		/// <returns>true if an area was removed</returns>
		public bool Remove(IArea area)
		{
			if (area == null)
			{
				throw new ArgumentNullException("area", "Areas can not be null");
			}

			bool removed = areas.Remove(area);

			if (removed && AreaRemoved != null)
			{
				AreaRemoved.Invoke(this, new AreaRemovedArgs(area));
			}

			return removed;
		}

		/// <summary>
		/// Get an area at the position defined
		/// </summary>
		/// <param name="position">Position to look</param>
		/// <returns>Reference to the area</returns>
		public IArea Get(IPosition position)
		{
			IArea currentArea = null;

			//Assuming it's 2D for comparison sake
			IPosition2D position2D = position as IPosition2D;

			//Looking through all areas in current level
			foreach (Area area in areas)
			{
				IPosition2D areaPosition2D = area.Position as IPosition2D;

				if (areaPosition2D.X == position2D.X && areaPosition2D.Y == position2D.Y)
				{
					currentArea = area;
					//We can stop searching since there won't be any areas with duplicate positions
					break;
				}
			}

			if (currentArea == null)
			{
				throw new ArgumentNullException("area", "No Area found at position");
			}

			return currentArea;
		}

		/// <summary>
		/// Get a list of neighbours for the specified area
		/// </summary>
		/// <param name="area">Area to get the neighbours around</param>
		/// <returns>List of IArea which are next to the supplied area</returns>
		public List<IArea> GetNeighbours(IArea area)
		{
			//The method will return maximum of 4 neighbours in the 4 cardinal directions, doesn't take into account diagonal neighbours
			List<IArea> neighbours = new List<IArea>();

			//Assuming it's 2D for ease of comparison
			IPosition2D areaPosition2D = area.Position as IPosition2D;

			foreach (Area a in areas)
			{
				IPosition2D potentialNeighbourPosition2D = a.Position as IPosition2D;

				//Assuming negative positions in areas are possible
				for (int x = -1; x < 2; x++)
				{
					for (int y = -1; y < 2; y++)
					{
						//If the position is 0, 0 offset from current area, ignore it, since it's the current area
						if (x == 0 && y == 0)
							continue;

						if (potentialNeighbourPosition2D.X + x == areaPosition2D.X && potentialNeighbourPosition2D.Y + y == areaPosition2D.Y)
						{
							//If potential Neighbour Position + offset equals the current area position, add it to our list, since it's a neighbour
							neighbours.Add(a);
						}
					}
				}
			}

			return neighbours;
		}

		#endregion
	}
}
