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

			// TODO: Issue 13 (https://github.com/Wizcorp/TileSystem/issues/13)

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
			Area area = new Area();
			var mockLevel = new Mock<ILevel>();
			var mockPosition = new Mock<IPosition2D>();
			var mockTile = new Mock<Tile>();
			var tilePosition = new Position2D(0, 0);
			var tilePositionNotFound = new Position2D(1, 0);

			area.SetPosition(mockLevel.Object, mockPosition.Object);
			mockTile.Object.SetPosition(area, tilePosition);
			area.Add(mockTile.Object);

			// Test Nulls
			Assert.That(() => area.Get(null), Throws.ArgumentNullException);

			// Test Same Tile
			Assert.AreSame(mockTile.Object, area.Get(tilePosition));

			// Test Tile Not Found
			Assert.AreNotSame(mockTile.Object, area.Get(tilePositionNotFound));
        }

		[Test]
		public void PositionGetNeighbours()
		{
			Area area = new Area();
			var mockLevel = new Mock<ILevel>();
			var mockPosition = new Mock<IPosition2D>();
			var mockTile = new Mock<Tile>();
			var tilePosition = new Position2D(0, 0);
			var mockTileNotInArea = new Mock<Tile>();
			var mockFirstNeighbourTile = new Mock<Tile>();
			var firstNeighbourTilePosition = new Position2D(1, 0);
			var mockSecondNeighbourTile = new Mock<Tile>();
			var secondNeighbourTilePosition = new Position2D(1, 1);
			var mockNotANeighbourTile = new Mock<Tile>();
			var notANeighbourTilePosition = new Position2D(4, 0);

			area.SetPosition(mockLevel.Object, mockPosition.Object);
			mockTile.Object.SetPosition(area, tilePosition);
			area.Add(mockTile.Object);

			// Test Nulls
			Assert.That(() => area.GetNeighbours(null), Throws.ArgumentNullException);

			// Test Tile Not In Area
			Assert.That(() => area.GetNeighbours(mockTileNotInArea.Object), Throws.ArgumentException);

			// Test No Neighbours
			Assert.Null(area.GetNeighbours(mockTile.Object));

			// Test One Or More Neighbours
			mockFirstNeighbourTile.Object.SetPosition(area, firstNeighbourTilePosition);
			area.Add(mockFirstNeighbourTile.Object);
			Assert.IsTrue(area.GetNeighbours(mockTile.Object).Count == 1);

			mockSecondNeighbourTile.Object.SetPosition(area, secondNeighbourTilePosition);
			area.Add(mockSecondNeighbourTile.Object);
			Assert.IsTrue(area.GetNeighbours(mockTile.Object).Count == 2);

			mockNotANeighbourTile.Object.SetPosition(area, notANeighbourTilePosition);
			area.Add(mockNotANeighbourTile.Object);
			Assert.IsTrue(area.GetNeighbours(mockTile.Object).Count == 2);
        }
    }
}
