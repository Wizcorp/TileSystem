using System;
using System.Collections.Generic;

using TileSystem.Interfaces.Base;
using TileSystem.Interfaces.Creation;
using TileSystem.Interfaces.Management;

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
			AreaFactory = areaFactory;
			TileFactory = tileFactory;
			EntityFactory = entityFactory;
		}

		#region Creation Methods

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
			throw new NotImplementedException();
		}

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
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		#endregion

		#region Area Methods

		/// <summary>
		/// Add area to the current level
		/// </summary>
		/// <param name="area">Area you are adding</param>
		public void Add(IArea area)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Remove area from the current level
		/// </summary>
		/// <param name="area">Area to remove</param>
		/// <returns>true if an area was removed</returns>
		public bool Remove(IArea area)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Get an area at the position defined
		/// </summary>
		/// <param name="position">Position to look</param>
		/// <returns>Reference to the area</returns>
		public IArea Get(IPosition position)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Get a list of neighbours for the specified area
		/// </summary>
		/// <param name="area">Area to get the neighbours around</param>
		/// <returns>List of IArea which are next to the supplied area</returns>
		public List<IArea> GetNeighbours(IArea area)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
