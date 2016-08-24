using Moq;

using NUnit.Framework;

using TileSystem.Interfaces.Base;
using TileSystem.Interfaces.TwoDimension;
using TileSystem.Implementation.TwoDimension;
using TileSystem.Interfaces.Creation;

namespace Tests.TwoDimension
{
	[TestFixture]
	[Category("TwoDimension")]
	public class LevelTests
	{
		[Test]
		public void LevelFactoryConstructor()
		{
			// Create mock factories
			var mockAreaFactory = new Mock<IAreaFactory>();
			var mockTileFactory = new Mock<ITileFactory>();
			var mockEntityFactory = new Mock<IEntityFactory>();

			// Test Area Null
			Assert.That(() => new Level(null, mockTileFactory.Object, mockEntityFactory.Object), Throws.ArgumentNullException);

			// Test Tile Null
			Assert.That(() => new Level(mockAreaFactory.Object, null, mockEntityFactory.Object), Throws.ArgumentNullException);

			// Test Entity Null
			Assert.That(() => new Level(mockAreaFactory.Object, mockTileFactory.Object, null), Throws.ArgumentNullException);

			// Test Create Works
			Assert.That(() => new Level(mockAreaFactory.Object, mockTileFactory.Object, mockEntityFactory.Object), Throws.Nothing);
		}

		[Test]
		public void AreaAdd()
		{
			Level level = new Level();
			var mockArea = new Mock<IArea>();

			bool addCalled = false;

			// Register added event and make sure it is called
			level.AreaAdded += (sender, args) =>
			{
				addCalled = true;
			};

			// Test Null
			Assert.That(() => level.Add(null), Throws.ArgumentNullException);

			// Assert add event was not called
			Assert.IsFalse(addCalled);

			// Test Add Works
			Assert.That(() => level.Add(mockArea.Object), Throws.Nothing);

			// Assert add event was called
			Assert.IsTrue(addCalled);

			// Reset before next test
			addCalled = false;

			// Test duplicate fails
			Assert.That(() => level.Add(mockArea.Object), Throws.ArgumentException);

			// Assert add event was not called
			Assert.IsFalse(addCalled);
		}

		[Test]
		public void AreaRemove()
		{
			Level level = new Level();
			var mockArea = new Mock<IArea>();

			bool removeCalled = false;

			// Register removed event and make sure it is called
			level.AreaRemoved += (sender, args) =>
			{
				removeCalled = true;
			};

			// Add Area
			level.Add(mockArea.Object);

			// Test Null
			Assert.That(() => level.Remove(null), Throws.ArgumentNullException);

			// Assert remove event was not called
			Assert.IsFalse(removeCalled);

			// Test Remove (true removing the object)
			Assert.That(level.Remove(mockArea.Object), Is.True);

			// Assert remove event was called
			Assert.IsTrue(removeCalled);

			// Reset before next test
			removeCalled = false;

			// Test Remove (false not removing the object)
			Assert.That(level.Remove(mockArea.Object), Is.False);

			// Assert remove event was not called
			Assert.IsFalse(removeCalled);
		}

		[Test]
		public void PositionGet()
		{
			// TODO: Issue 15 (https://github.com/Wizcorp/TileSystem/issues/15)
			Assert.Fail();
		}

		[Test]
		public void PositionGetNeighbours()
		{
			// TODO: Issue 15 (https://github.com/Wizcorp/TileSystem/issues/15)
			Assert.Fail();
		}
	}
}
