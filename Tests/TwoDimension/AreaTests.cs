using Moq;

using NUnit.Framework;

using TileSystem.Interfaces.Base;
using TileSystem.Interfaces.TwoDimension;
using TileSystem.Implementation.TwoDimension;

namespace Tests.TwoDimension
{
	[TestFixture]
	[Category("TwoDimension")]
	public class AreaTests
	{
		[Test]
		public void AreaCreate()
		{
			string type = "MyType";
			string variation = "MyVariation";

			// Empty Constructor
			Area area = new Area();

			// Test Type
			Assert.IsNull(area.Type);
			// Test Variation
			Assert.IsNull(area.Variation);

			// Initialised Constructor
			Area areaTwo = new Area(type, variation);

			// Test Type
			Assert.AreSame(areaTwo.Type, type);
			// Test Variation
			Assert.AreSame(areaTwo.Variation, variation);
		}

		[Test]
		public void AreaDestroy()
		{
			Area area = new Area();

			bool destroyedCalled = false;

			// Register destroyed event and make sure it is called
			area.Destroyed += (sender, args) =>
			{
				destroyedCalled = true;
			};

			area.Destroy();

			// Destroy event was fired
			Assert.IsTrue(destroyedCalled);
		}

		[Test]
		public void AreaDestroyPropagation()
		{
			Area area = new Area();
			var mockTile = new Mock<ITile>();

			// Add entity to Tile
			area.Add(mockTile.Object);

			// Destroy with propagation
			area.Destroy(true);

			// Make sure the tile below also received the destroy
			mockTile.Verify(tile => tile.Destroy(true), Times.Exactly(1));
		}

		[Test]
		public void TileAdd()
		{
			Area area = new Area();
			var mockTile = new Mock<ITile>();

			bool addCalled = false;

			// Register added event and make sure it is called
			area.TileAdded += (sender, args) =>
			{
				addCalled = true;
			};

			// Test Null
			Assert.That(() => area.Add(null), Throws.ArgumentNullException);

			// Assert add event was not called
			Assert.IsFalse(addCalled);

			// Test Add Works
			Assert.That(() => area.Add(mockTile.Object), Throws.Nothing);

			// Assert add event was called
			Assert.IsTrue(addCalled);

			// Reset before next test
			addCalled = false;

			// Test duplicate fails
			Assert.That(() => area.Add(mockTile.Object), Throws.ArgumentException);

			// Assert add event was not called
			Assert.IsFalse(addCalled);
		}

		[Test]
		public void TileRemove()
		{
			Area area = new Area();
			var mockTile = new Mock<ITile>();

			bool removeCalled = false;

			// Register removed event and make sure it is called
			area.TileRemoved += (sender, args) =>
			{
				removeCalled = true;
			};

			// Add Tile
			area.Add(mockTile.Object);

			// Test Null
			Assert.That(() => area.Remove(null), Throws.ArgumentNullException);

			// Assert remove event was not called
			Assert.IsFalse(removeCalled);

			// Test Remove (true removing the object)
			Assert.That(area.Remove(mockTile.Object), Is.True);

			// Assert remove event was called
			Assert.IsTrue(removeCalled);

			// Reset before next test
			removeCalled = false;

			// Test Remove (false not removing the object)
			Assert.That(area.Remove(mockTile.Object), Is.False);

			// Assert remove event was not called
			Assert.IsFalse(removeCalled);
		}

		[Test]
		public void PositionSet()
		{
			Area area = new Area();
			var mockLevel = new Mock<ILevel>();
			var mockPosition = new Mock<IPosition2D>();

			// Test Nulls
			Assert.That(() => area.SetPosition(null, mockPosition.Object), Throws.ArgumentNullException);
			Assert.That(() => area.SetPosition(mockLevel.Object, null), Throws.ArgumentNullException);

			// Test Set Position Works
			Assert.That(() => area.SetPosition(mockLevel.Object, mockPosition.Object), Throws.Nothing);

			// Test Area
			Assert.AreSame(area.Level, mockLevel.Object);
			// Test Position
			Assert.AreSame(area.Position, mockPosition.Object);
		}

		[Test]
		public void PositionGet()
		{
			// TODO: Issue 6 (https://github.com/Wizcorp/TileSystem/issues/6)
			Assert.Fail();
		}

		[Test]
		public void PositionGetNeighbours()
		{
			// TODO: Issue 6 (https://github.com/Wizcorp/TileSystem/issues/6)
			Assert.Fail();
		}
	}
}
