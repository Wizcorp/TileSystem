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
	public class CreationTests
	{
		[Test]
		public void CreateArea()
		{
			// Create mock factories
			var mockAreaFactory = new Mock<IAreaFactory>();
			var mockTileFactory = new Mock<ITileFactory>();
			var mockEntityFactory = new Mock<IEntityFactory>();

			// Create a mock position that will be used with the area set position
			var mockPosition2D = new Mock<IPosition2D>();

			// Set up area factory so it will have a mock area to invoke without throwing in the create area
			mockAreaFactory.Setup(factory => factory.CreateArea(null, null)).Returns(new Mock<IArea>().Object);

			// Create level with mock factories
			Level level = new Level(mockAreaFactory.Object, mockTileFactory.Object, mockEntityFactory.Object);

			bool createdCalled = false;

			// Register added event and make sure it is called
			level.AreaCreated += (sender, args) =>
			{
				createdCalled = true;
			};

			// Test Null
			Assert.That(() => level.CreateArea(null, mockPosition2D.Object, null, null), Throws.ArgumentNullException);

			// Test Null
			Assert.That(() => level.CreateArea(level, null, null, null), Throws.ArgumentNullException);

			// Assert add event was not called
			Assert.IsFalse(createdCalled);

			// Test Create Works
			Assert.That(() => level.CreateArea(level, mockPosition2D.Object, null, null), Throws.Nothing);

			// Assert add event was called
			Assert.IsTrue(createdCalled);
		}

		[Test]
		public void CreateTile()
		{
			// Create mock factories
			var mockAreaFactory = new Mock<IAreaFactory>();
			var mockTileFactory = new Mock<ITileFactory>();
			var mockEntityFactory = new Mock<IEntityFactory>();

			// Create a mock position that will be used with the tile set position
			var mockPosition2D = new Mock<IPosition2D>();
			// Create a mock area that will be used with tile set position
			var mockArea = new Mock<IArea>();

			// Set up tile factory so it will have a mock tile to invoke without throwing in the create tile
			mockTileFactory.Setup(factory => factory.CreateTile(null, null)).Returns(new Mock<ITile>().Object);

			// Create level with mock factories
			Level level = new Level(mockAreaFactory.Object, mockTileFactory.Object, mockEntityFactory.Object);

			bool createdCalled = false;

			// Register added event and make sure it is called
			level.TileCreated += (sender, args) =>
			{
				createdCalled = true;
			};

			// Test Null
			Assert.That(() => level.CreateTile(null, mockPosition2D.Object, null, null), Throws.ArgumentNullException);

			// Test Null
			Assert.That(() => level.CreateTile(mockArea.Object, null, null, null), Throws.ArgumentNullException);

			// Assert add event was not called
			Assert.IsFalse(createdCalled);

			// Test Create Works
			Assert.That(() => level.CreateTile(mockArea.Object, mockPosition2D.Object, null, null), Throws.Nothing);

			// Assert add event was called
			Assert.IsTrue(createdCalled);
		}

		[Test]
		public void CreateEntity()
		{
			// Create mock factories
			var mockAreaFactory = new Mock<IAreaFactory>();
			var mockTileFactory = new Mock<ITileFactory>();
			var mockEntityFactory = new Mock<IEntityFactory>();

			// Create a mock tile that will be used with entity set parent
			var mockTile = new Mock<ITile>();

			// Set up entity factory so it will have a mock entity to invoke without throwing in the create entity
			mockEntityFactory.Setup(factory => factory.CreateEntity(null, null)).Returns(new Mock<IEntity>().Object);

			// Create level with mock factories
			Level level = new Level(mockAreaFactory.Object, mockTileFactory.Object, mockEntityFactory.Object);

			bool createdCalled = false;

			// Register added event and make sure it is called
			level.EntityCreated += (sender, args) =>
			{
				createdCalled = true;
			};

			// Test Null
			Assert.That(() => level.CreateEntity(null, null, null), Throws.ArgumentNullException);

			// Assert add event was not called
			Assert.IsFalse(createdCalled);

			// Test Create Works
			Assert.That(() => level.CreateEntity(mockTile.Object, null, null), Throws.Nothing);

			// Assert add event was called
			Assert.IsTrue(createdCalled);
		}
	}
}
