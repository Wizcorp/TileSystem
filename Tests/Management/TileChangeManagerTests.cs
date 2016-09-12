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

            // Entity that will trigger NULL check
            var mockNullEntity = new Mock<IEntity>();
            // Mock IChangeTile on entity that triggers NULL
            var mockNullMovable = mockNullEntity.As<IChangeTile>();

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


            // Setup events for Start, Change, and Finish functions so they emit NULL trigger on the events
            mockNullMovable.Setup(movable => movable.StartChangeTile(mockTileOne.Object, mockTileTwo.Object))
                .Raises(m => m.ChangeStarted += null, null, tileStartArgs);

            mockNullMovable.Setup(movable => movable.ChangeTile(mockTileOne.Object, mockTileTwo.Object))
                .Raises(m => m.Changing += null, null, tileChangeArgs);

            mockNullMovable.Setup(movable => movable.FinishChangeTile(mockTileOne.Object, mockTileTwo.Object))
                .Raises(m => m.ChangeFinished += null, null, tileFinishArgs);

            // Register Creator
            manager.RegisterEntityCreator(mockCreator.Object);

			// Add solver that will handlt the entities Tile Change
			manager.Add(mockSolver.Object);

			// Raise entity created event
			mockCreator.Raise(creator => creator.EntityCreated += null, mockCreator.Object, new EntityCreatedArgs(mockTileOne.Object, mockEntity.Object));
            mockCreator.Raise(creator => creator.EntityCreated += null, mockCreator.Object, new EntityCreatedArgs(mockTileOne.Object, mockNullEntity.Object));

            // Move the entity
            mockMovable.Object.StartChangeTile(mockTileOne.Object, mockTileTwo.Object);
            mockMovable.Object.ChangeTile(mockTileOne.Object, mockTileTwo.Object);
            mockMovable.Object.FinishChangeTile(mockTileOne.Object, mockTileTwo.Object);

            // Asset if event was called with NULL object
            Assert.That(() => mockNullMovable.Object.StartChangeTile(mockTileOne.Object, mockTileTwo.Object), Throws.ArgumentNullException);
            Assert.That(() => mockNullMovable.Object.ChangeTile(mockTileOne.Object, mockTileTwo.Object), Throws.ArgumentNullException);
            Assert.That(() => mockNullMovable.Object.FinishChangeTile(mockTileOne.Object, mockTileTwo.Object), Throws.ArgumentNullException);

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
			// TODO: Issue 8 (https://github.com/Wizcorp/TileSystem/issues/8)
			Assert.Fail();
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