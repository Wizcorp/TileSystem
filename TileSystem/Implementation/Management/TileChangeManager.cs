using System;
using System.Collections.Generic;

using TileSystem.Interfaces.Base;
using TileSystem.Interfaces.Creation;
using TileSystem.Interfaces.Management;
using TileSystem.Interfaces.Solvers;
using TileSystem.Interfaces.TileChange;

namespace TileSystem.Implementation.Management
{
	/// <summary>
	/// Implementation of IManageTileChange interface, this class is a basic
	/// implementation that can be inherited and overridden as necessary.
	/// 
	/// Notes:
	/// Add will throw ArgumentException to stop multiples of the same solver
	/// instance being added, and ArgumentNullException for null values
	/// (like a Dictionary)
	/// 
	/// Current warnings for registered events not being IEntity are not
	/// logged. TODO: Decide if we should throw these events, techinically
	/// they should not be able to happen and if they do it's likely to be an
	/// error
	/// </summary>
	public class TileChangeManager : IManageTileChange
	{
		protected List<ISolver> solvers;
		protected List<ICreateEntities> creators;

		/// <summary>
		/// Constructor Creates List<ISolver>, and List<ICreateEntities> for use by
		/// this class and any derived classes
		/// </summary>
		public TileChangeManager()
		{
			solvers = new List<ISolver>();
			creators = new List<ICreateEntities>();
		}

		/// <summary>
		/// Constructor to allow injection of a list of solvers when created
		/// </summary>
		/// <param name="solvers">List of solvers that will solve Tile Change events</param>
		public TileChangeManager(List<ISolver> solvers)
		{
			this.solvers = solvers;
		}

		/// <summary>
		/// Add a solver that will be executed on an entity changing tiles
		/// </summary>
		/// <param name="solver">Solver that will register to tile change events</param>
		public virtual void Add(ISolver solver)
		{
			if (solver == null)
			{
				throw new ArgumentNullException("solver", "Solvers can not be null");
			}

			if (solvers.Contains(solver))
			{
				throw new ArgumentException("Duplicate value", "solver");
			}

			solvers.Add(solver);
		}

		/// <summary>
		/// Removes a solver from the solvers list
		/// </summary>
		/// <param name="solver">Solver to be removed</param>
		/// <returns>true if an element is removed, false otherwise</returns>
		public virtual bool Remove(ISolver solver)
		{
			if (solver == null)
			{
				throw new ArgumentNullException("solver", "Solvers can not be null");
			}

			return solvers.Remove(solver);
		}

		/// <summary>
		/// Register a creator, used when an entity is created to check if we need to
		/// execute on the tile change events, if it does not then we return, else we
		/// add the tile change events for the entity which will trigger the solvers
		/// </summary>
		/// <param name="creator">Creator that can create entities in the system</param>
		public virtual void RegisterEntityCreator(ICreateEntities creator)
		{
			if (creator == null)
			{
				throw new ArgumentNullException("creator", "Creators can not be null");
			}

			if (creators.Contains(creator))
			{
				throw new ArgumentException("Duplicate value", "creator");
			}

			creators.Add(creator);

			creator.EntityCreated += RegisterTileChangeEvents;
		}

		/// <summary>
		/// Deregister a creator, used to remove an entity creator from the system
		/// 
		/// The entities that were created by this creator will remain registered to the
		/// system but no more entities will be added if this creator creates them
		/// </summary>
		/// <param name="creator">Creator that can create entities in the system</param>
		/// <returns>true if an element is removed, false otherwise</returns>
		public virtual bool DeregisterEntityCreator(ICreateEntities creator)
		{
			if (creator == null)
			{
				throw new ArgumentNullException("creator", "Creators can not be null");
			}

			bool removed = creators.Remove(creator);

			if (removed)
			{
				creator.EntityCreated -= RegisterTileChangeEvents;
			}

			return removed;
		}

		/// <summary>
		/// Register to tile change events, used when an entity is created
		/// 
		/// If the entity implements IChangeTile then we register those events with
		/// the functions that will execute the solvers
		/// </summary>
		/// <param name="sender">Normally an object implementing ICreateEntities</param>
		/// <param name="e">Entity created events, holds a reference to the entity that was created</param>
		private void RegisterTileChangeEvents(object sender, EntityCreatedArgs e)
		{
			IChangeTile movable = e.Entity as IChangeTile;

			if (movable == null)
			{
				return;
			}

			movable.ChangeStarted += EntityTileChangeStarted;
			movable.Changing += EntityTileChanged;
			movable.ChangeFinished += EntityTileChangeFinished;

			e.Entity.Destroyed += DeregisterTileChangeEvents;
		}

		/// <summary>
		/// Deregister the tile change events, used when an entity is destroyed
		/// </summary>
		/// <param name="sender">Normally an object of type IEntity</param>
		/// <param name="e">Entity destroyed events, holds a reference to the entity that was destroyed</param>
		private void DeregisterTileChangeEvents(object sender, EntityDestroyedArgs e)
		{
			IChangeTile movableEntity = e.Entity as IChangeTile;

			if (movableEntity == null)
			{
				return;
			}

			movableEntity.ChangeStarted -= EntityTileChangeStarted;
			movableEntity.Changing -= EntityTileChanged;
			movableEntity.ChangeFinished -= EntityTileChangeFinished;
		}

		/// <summary>
		/// Entities tile change started
		/// </summary>
		/// <param name="sender">Game Entity</param>
		/// <param name="e">Tile changed event args</param>
		private void EntityTileChangeStarted(object sender, TileChangedArgs e)
		{
			IEntity entity = sender as IEntity;
			if (entity == null)
			{
				// TODO: Issue 7 (https://github.com/Wizcorp/TileSystem/issues/7)
				return;
			}

			ExecuteSolvers(entity, e);
		}

		/// <summary>
		/// Enitity changed tile
		/// </summary>
		/// <param name="sender">Game Entity</param>
		/// <param name="e">Tile changed event args</param>
		private void EntityTileChanged(object sender, TileChangedArgs e)
		{
			IEntity entity = sender as IEntity;
			if (entity == null)
			{
				// TODO: Issue 7 (https://github.com/Wizcorp/TileSystem/issues/7)
				return;
			}

			ExecuteSolvers(entity, e);
		}

		/// <summary>
		/// Entities the tile change finished
		/// </summary>
		/// <param name="sender">Game Entity</param>
		/// <param name="e">Tile changed event args</param>
		private void EntityTileChangeFinished(object sender, TileChangedArgs e)
		{
			IEntity entity = sender as IEntity;
			if (entity == null)
			{
				// TODO: Issue 7 (https://github.com/Wizcorp/TileSystem/issues/7)
				return;
			}

			ExecuteSolvers(entity, e);
		}

		/// <summary>
		/// Execute every Solver the specified entity and e
		/// Return true if any interruption is returned by a solver
		/// 
		/// TODO: Issue 9 (https://github.com/Wizcorp/TileSystem/issues/9)
		/// </summary>
		private void ExecuteSolvers(IEntity entity, TileChangedArgs e)
		{
			foreach (ISolver solver in solvers)
			{
				if (solver.Solve(entity, e))
				{
					break;
				}
			}
		}
	}
}
