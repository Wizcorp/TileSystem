using Moq;

using NUnit.Framework;

using TileSystem.Interfaces.Base;
using TileSystem.Interfaces.Creation;
using TileSystem.Implementation.Management;
using TileSystem.Interfaces.Solvers;
using TileSystem.Interfaces.TileChange;
using TileSystem.Utils;

namespace Tests
{
	[TestFixture]
	[Category("Management")]
	public class TileChangeManagerTests
	{
		#region Add / Remove

		[Test]
		public void SolverAdd()
		{
			TileChangeManager manager = new TileChangeManager();
			var mock = new Mock<ISolver>();

			// Test Null
			Assert.That(() => manager.Add(null), Throws.ArgumentNullException);
			// Test Add Works
			Assert.That(() => manager.Add(mock.Object), Throws.Nothing);
			// Test duplicate fails
			Assert.That(() => manager.Add(mock.Object), Throws.ArgumentException);
		}

		[Test]
		public void SolverRemove()
		{
			TileChangeManager manager = new TileChangeManager();
			var mock = new Mock<ISolver>();

			// Add Solver
			manager.Add(mock.Object);

			// Test Null
			Assert.That(() => manager.Remove(null), Throws.ArgumentNullException);
			// Test Remove (true removing the object)
			Assert.That(manager.Remove(mock.Object), Is.True);
			// Test Remove (false not removing the object)
			Assert.That(manager.Remove(mock.Object), Is.False);
		}

		[Test]
		public void CreatorAdd()
		{
			TileChangeManager manager = new TileChangeManager();
			var mock = new Mock<ICreateEntities>();

			// Test Null
			Assert.That(() => manager.RegisterEntityCreator(null), Throws.ArgumentNullException);
			// Test Add Works
			Assert.That(() => manager.RegisterEntityCreator(mock.Object), Throws.Nothing);
			// Test duplicate fails
			Assert.That(() => manager.RegisterEntityCreator(mock.Object), Throws.ArgumentException);
		}

		[Test]
		public void CreatorRemove()
		{
			TileChangeManager manager = new TileChangeManager();
			var mock = new Mock<ICreateEntities>();

			// Add Creator
			manager.RegisterEntityCreator(mock.Object);

			// Test Null
			Assert.That(() => manager.DeregisterEntityCreator(null), Throws.ArgumentNullException);
			// Test Remove (true removing the object)
			Assert.That(manager.DeregisterEntityCreator(mock.Object), Is.True);
			// Test Remove (false not removing the object)
			Assert.That(manager.DeregisterEntityCreator(mock.Object), Is.False);
		}

		#endregion

		#region Tile Change executions solvers correctly

		[Test]
		public void TileChangeEvents()
		{
			TileChangeManager manager = new TileChangeManager();

			// Entity that create entities will create
			var mockEntity = new Mock<IEntity>();
			// Mock IChangeTile on entity
			var mockMovable = mockEntity.As<IChangeTile>();

			// Tile from
			var mockTileOne = new Mock<ITile>();
			// Tile to
			var mockTileTwo = new Mock<ITile>();

			// Will make sure that it recieves the correct events
			var mockSolver = new Mock<ISolver>();

			// Will create one entity and then have the entity emit the change events
			var mockCreator = new Mock<ICreateEntities>();

			// Setup args classes for verification later
			var tileStartArgs = new TileChangedArgs(mockTileOne.Object, mockTileTwo.Object, TileChangeType.Start);
			var tileChangeArgs = new TileChangedArgs(mockTileOne.Object, mockTileTwo.Object, TileChangeType.Change);
			var tileFinishArgs = new TileChangedArgs(mockTileOne.Object, mockTileTwo.Object, TileChangeType.Finish);

			// Setup events for Start, Change, and Finish functions so they emit the events
			mockMovable.Setup(movable => movable.StartChangeTile(mockTileOne.Object, mockTileTwo.Object))
				.Raises(m => m.ChangeStarted += null, mockMovable.Object, tileStartArgs);

			mockMovable.Setup(movable => movable.ChangeTile(mockTileOne.Object, mockTileTwo.Object))
				.Raises(m => m.Changing += null, mockMovable.Object, tileChangeArgs);

			mockMovable.Setup(movable => movable.FinishChangeTile(mockTileOne.Object, mockTileTwo.Object))
				.Raises(m => m.ChangeFinished += null, mockMovable.Object, tileFinishArgs);

			// Register Creator
			manager.RegisterEntityCreator(mockCreator.Object);

			// Add solver that will handlt the entities Tile Change
			manager.Add(mockSolver.Object);

			// Raise entity created event
			mockCreator.Raise(creator => creator.EntityCreated += null, mockCreator.Object, new EntityCreatedArgs(mockTileOne.Object, mockEntity.Object));

			// Move the entity
			mockMovable.Object.StartChangeTile(mockTileOne.Object, mockTileTwo.Object);
			mockMovable.Object.ChangeTile(mockTileOne.Object, mockTileTwo.Object);
			mockMovable.Object.FinishChangeTile(mockTileOne.Object, mockTileTwo.Object);

			// Verify the solver was executed with correct args
			mockSolver.Verify(solver => solver.Solve(mockEntity.Object, tileStartArgs), Times.Exactly(1));
			mockSolver.Verify(solver => solver.Solve(mockEntity.Object, tileChangeArgs), Times.Exactly(1));
			mockSolver.Verify(solver => solver.Solve(mockEntity.Object, tileFinishArgs), Times.Exactly(1));

			// Add new solver
			var mockSolverTwo = new Mock<ISolver>();
			manager.Add(mockSolverTwo.Object);

			// Move the entity
			mockMovable.Object.StartChangeTile(mockTileOne.Object, mockTileTwo.Object);
			mockMovable.Object.ChangeTile(mockTileOne.Object, mockTileTwo.Object);
			mockMovable.Object.FinishChangeTile(mockTileOne.Object, mockTileTwo.Object);

			// Verify solver one was executed with correct args
			mockSolver.Verify(solver => solver.Solve(mockEntity.Object, tileStartArgs), Times.Exactly(2));
			mockSolver.Verify(solver => solver.Solve(mockEntity.Object, tileChangeArgs), Times.Exactly(2));
			mockSolver.Verify(solver => solver.Solve(mockEntity.Object, tileFinishArgs), Times.Exactly(2));

			// Verify solver two was executed with correct args
			mockSolverTwo.Verify(solver => solver.Solve(mockEntity.Object, tileStartArgs), Times.Exactly(1));
			mockSolverTwo.Verify(solver => solver.Solve(mockEntity.Object, tileChangeArgs), Times.Exactly(1));
			mockSolverTwo.Verify(solver => solver.Solve(mockEntity.Object, tileFinishArgs), Times.Exactly(1));
		}

		[Test]
		public void TileChangeEntityDestroyedDeregisterEvents()
		{
			// A tile manager should deregister an entity when it raises a Destroy event.

			TileChangeManager manager = new TileChangeManager();

			// Our mock entity should implement both IEntity (for raising Destroy) and IChangeTile (for performing movement)
			Mock<IEntity> mockEntity = new Mock<IEntity>();
			Mock<IChangeTile> mockMovable = mockEntity.As<IChangeTile>();

			// Our mock creator is used only to create our mocked entity.
			Mock<ICreateEntities> mockCreator = new Mock<ICreateEntities>();

			// Two mocked tiles to move between
			Mock<ITile> mockTileFrom = new Mock<ITile>();
			Mock<ITile> mockTileTo = new Mock<ITile>();

			// Our first mock solver will be checked to ensure it's receiving the events.
			Mock<ISolver> mockSolverOne = new Mock<ISolver>();

			// These tile change args classes will permit the solver to be invoked upon event propogation.
			TileChangedArgs tileStartArgs = new TileChangedArgs(mockTileFrom.Object, mockTileTo.Object, TileChangeType.Start);
			TileChangedArgs tileChangeArgs = new TileChangedArgs(mockTileFrom.Object, mockTileTo.Object, TileChangeType.Change);
			TileChangedArgs tileFinishArgs = new TileChangedArgs(mockTileFrom.Object, mockTileTo.Object, TileChangeType.Finish);

			// Our mocked movable entity should emit tile change start, changing, and finished events.
			mockMovable.Setup(movable => movable.StartChangeTile(mockTileFrom.Object, mockTileTo.Object))
				.Raises(m => m.ChangeStarted += null, mockMovable.Object, tileStartArgs);

			mockMovable.Setup(movable => movable.ChangeTile(mockTileFrom.Object, mockTileTo.Object))
				.Raises(m => m.Changing += null, mockMovable.Object, tileChangeArgs);

			mockMovable.Setup(movable => movable.FinishChangeTile(mockTileFrom.Object, mockTileTo.Object))
				.Raises(m => m.ChangeFinished += null, mockMovable.Object, tileFinishArgs);

			// Register the entity creator with the tile manager.
			manager.RegisterEntityCreator(mockCreator.Object);

			// Our first solver must be added before it will receive events.
			manager.Add(mockSolverOne.Object);

			// Raise an entity creation event to register that entity for tile change events.
			mockCreator.Raise(creator => creator.EntityCreated += null, mockCreator.Object, new EntityCreatedArgs(mockTileFrom.Object, mockEntity.Object));

			// Move the entity.
			mockMovable.Object.StartChangeTile(mockTileFrom.Object, mockTileTo.Object);
			mockMovable.Object.ChangeTile(mockTileFrom.Object, mockTileTo.Object);
			mockMovable.Object.FinishChangeTile(mockTileFrom.Object, mockTileTo.Object);

			// The entity's movement should have invoked each corresponding state transition method once.
			// Solver One has received one set of events.

			// Add a second solver, for differential comparison.
			Mock<ISolver> mockSolverTwo = new Mock<ISolver>();
			manager.Add(mockSolverTwo.Object);

			// Move the entity a second time.
			mockMovable.Object.StartChangeTile(mockTileFrom.Object, mockTileTo.Object);
			mockMovable.Object.ChangeTile(mockTileFrom.Object, mockTileTo.Object);
			mockMovable.Object.FinishChangeTile(mockTileFrom.Object, mockTileTo.Object);

			// The two solvers should have again incremented their movement method invocation count once each.
			// Solver One has received two sets of events, Solver Two has received one set of events.

			// Raise a Destroy event from the entity (via its IEntity mock), which should trigger deregistration.
			mockEntity.Raise(movable => movable.Destroyed += null, new EntityDestroyedArgs(mockEntity.Object));

			// Move the entity a third time.
			mockMovable.Object.StartChangeTile(mockTileFrom.Object, mockTileTo.Object);
			mockMovable.Object.ChangeTile(mockTileFrom.Object, mockTileTo.Object);
			mockMovable.Object.FinishChangeTile(mockTileFrom.Object, mockTileTo.Object);

			// Because the entity was deregistered, neither solver incremented its movement method invocation count.
			// Solver One has received two sets of events, Solver Two has received one set of events.
			// Should deregistration be faulty, we'd expect the invocation counts to stand at 3 and 2, repsectively.
			mockSolverOne.Verify(solver => solver.Solve(mockEntity.Object, tileStartArgs), Times.Exactly(2));
			mockSolverOne.Verify(solver => solver.Solve(mockEntity.Object, tileChangeArgs), Times.Exactly(2));
			mockSolverOne.Verify(solver => solver.Solve(mockEntity.Object, tileFinishArgs), Times.Exactly(2));
			mockSolverTwo.Verify(solver => solver.Solve(mockEntity.Object, tileStartArgs), Times.Exactly(1));
			mockSolverTwo.Verify(solver => solver.Solve(mockEntity.Object, tileChangeArgs), Times.Exactly(1));
			mockSolverTwo.Verify(solver => solver.Solve(mockEntity.Object, tileFinishArgs), Times.Exactly(1));
		}

		[Test]
		public void TileChangeSolverBreak()
		{
			// TODO: Issue 9 (https://github.com/Wizcorp/TileSystem/issues/9)
			Assert.Fail();
		}

		#endregion
	}
}