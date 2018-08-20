using Moq;

using NUnit.Framework;

using TileSystem.Interfaces.Base;
using TileSystem.Interfaces.TwoDimension;
using TileSystem.Implementation.TwoDimension;
using TileSystem.Interfaces.Creation;
using System.Collections.Generic;

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
			Level level = new Level();
			var mockArea = new Mock<IArea>();

			mockArea.Object.SetPosition(level, new Position2D(5, 5));

			//Add Area to Level
			level.Add(mockArea.Object);

			IPosition2D position2D = new Position2D(5, 5);

			//Grab the Area by position
			IArea area = level.Get(position2D);

			//Test that area isn't null
			Assert.IsNotNull(area);

			//Test that wrong position throws error
			IPosition2D wrongPosition2D = new Position2D(3, 2);
			Assert.That(() => level.Get(wrongPosition2D), Throws.ArgumentNullException);
		}

		[Test]
		public void PositionGetNeighbours()
		{
			Level level = new Level();
			var mockArea1 = new Mock<IArea>();
			var mockArea2 = new Mock<IArea>();
			var mockArea3 = new Mock<IArea>();
			var mockArea4 = new Mock<IArea>();
			var mockArea5 = new Mock<IArea>();
			var mockArea6 = new Mock<IArea>();

			//3 Areas that will be each other neighbours
			mockArea1.Object.SetPosition(level, new Position2D(2, 2));
			mockArea2.Object.SetPosition(level, new Position2D(1, 2));
			mockArea3.Object.SetPosition(level, new Position2D(2, 3));

			//2 Areas that will be each other neighbours
			mockArea4.Object.SetPosition(level, new Position2D(8, 8));
			mockArea5.Object.SetPosition(level, new Position2D(8, 9));

			//Single Area with no neighbours
			mockArea3.Object.SetPosition(level, new Position2D(0, 0));

			//Add Areas to the level
			level.Add(mockArea1.Object);
			level.Add(mockArea2.Object);
			level.Add(mockArea3.Object);
			level.Add(mockArea4.Object);
			level.Add(mockArea5.Object);
			level.Add(mockArea6.Object);

			//Test that mockArea2 has 2 neighbours
			List<IArea> neighbours1 = level.GetNeighbours(mockArea2.Object);
			Assert.IsTrue(neighbours1.Count == 2);

			//Test that mockArea4 has 1 neighbour
			List<IArea> neighbours2 = level.GetNeighbours(mockArea4.Object);
			Assert.IsTrue(neighbours2.Count == 1);

			//Test that mockArea6 has no neighbours
			List<IArea> neighbours3 = level.GetNeighbours(mockArea6.Object);
			Assert.IsTrue(neighbours3.Count == 0);
		}
	}
}
