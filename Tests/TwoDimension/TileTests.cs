using Moq;

using NUnit.Framework;

using TileSystem.Interfaces.Base;
using TileSystem.Interfaces.TwoDimension;
using TileSystem.Implementation.TwoDimension;


namespace Tests.TwoDimension
{
	[TestFixture]
	[Category("TwoDimension")]
	public class TileTests
	{
		[Test]
		public void TileCreate()
		{
			string type = "MyType";
			string variation = "MyVariation";

			// Empty Constructor
			Tile tile = new Tile();

			// Test Type
			Assert.IsNull(tile.Type);
			// Test Variation
			Assert.IsNull(tile.Variation);

			// Initialised Constructor
			Tile tileTwo = new Tile(type, variation);

			// Test Type
			Assert.AreSame(tileTwo.Type, type);
			// Test Variation
			Assert.AreSame(tileTwo.Variation, variation);
		}

		[Test]
		public void TileCleanUp()
		{
			Tile tile = new Tile();
			var mockEntity = new Mock<IEntity>();

			// Add entity to Tile
			tile.Add(mockEntity.Object);

			// Clean Up
			tile.CleanUp();

			// Make sure the entity clean up was called
			mockEntity.Verify(entity => entity.CleanUp(), Times.Exactly(1));
		}

		[Test]
		public void TileDestroy()
		{
			Tile tile = new Tile();

			bool destroyedCalled = false;

			// Register destroyed event and make sure it is called
			tile.Destroyed += (sender, args) =>
			{
				destroyedCalled = true;
			};

			tile.Destroy();

			// Destroy event was fired
			Assert.IsTrue(destroyedCalled);
		}

		[Test]
		public void EntityAdd()
		{
			Tile tile = new Tile();
			var mockEntity = new Mock<IEntity>();

			bool addCalled = false;

			// Register added event and make sure it is called
			tile.EntityAdded += (sender, args) =>
			{
				addCalled = true;
			};

			// Test Null
			Assert.That(() => tile.Add(null), Throws.ArgumentNullException);
			
			// Assert add event was not called
			Assert.IsFalse(addCalled);

			// Test Add Works
			Assert.That(() => tile.Add(mockEntity.Object), Throws.Nothing);

			// Assert add event was called
			Assert.IsTrue(addCalled);

			// Reset before next test
			addCalled = false;

			// Test duplicate fails
			Assert.That(() => tile.Add(mockEntity.Object), Throws.ArgumentException);

			// Assert add event was not called
			Assert.IsFalse(addCalled);
		}

		[Test]
		public void EntityRemove()
		{
			Tile tile = new Tile();
			var mockEntity = new Mock<IEntity>();

			bool removeCalled = false;

			// Register removed event and make sure it is called
			tile.EntityRemoved += (sender, args) =>
			{
				removeCalled = true;
			};

			// Add Entity
			tile.Add(mockEntity.Object);

			// Test Null
			Assert.That(() => tile.Remove(null), Throws.ArgumentNullException);

			// Assert remove event was not called
			Assert.IsFalse(removeCalled);

			// Test Remove (true removing the object)
			Assert.That(tile.Remove(mockEntity.Object), Is.True);

			// Assert remove event was called
			Assert.IsTrue(removeCalled);

			// Reset before next test
			removeCalled = false;

			// Test Remove (false not removing the object)
			Assert.That(tile.Remove(mockEntity.Object), Is.False);

			// Assert remove event was not called
			Assert.IsFalse(removeCalled);
		}

		[Test]
		public void PositionSet()
		{
			Tile tile = new Tile();
			var mockArea = new Mock<IArea>();
			var mockPosition = new Mock<IPosition2D>();

			// Test Nulls
			Assert.That(() => tile.SetPosition(null, mockPosition.Object), Throws.ArgumentNullException);
			Assert.That(() => tile.SetPosition(mockArea.Object, null), Throws.ArgumentNullException);

			// Test Set Position Works
			Assert.That(() => tile.SetPosition(mockArea.Object, mockPosition.Object), Throws.Nothing);

			// Test Area
			Assert.AreSame(tile.Area, mockArea.Object);
			// Test Position
			Assert.AreSame(tile.Position, mockPosition.Object);
		}
	}
}
