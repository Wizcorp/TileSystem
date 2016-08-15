using Moq;

using NUnit.Framework;

using TileSystem.Interfaces.Base;
using TileSystem.Interfaces.TwoDimension;
using TileSystem.Implementation.TwoDimension;

namespace Tests.TwoDimension
{
	[TestFixture]
	[Category("TwoDimension")]
	public class PositionTests
	{
		[Test]
		public void PositionConstructor()
		{
			int x = 0;
			int y = 0;

			// Test Area Null
			Assert.That(() => new Position2D(x, y), Throws.Nothing);

			// Create position and check x,y
			IPosition2D pos = new Position2D(x, y);
			Assert.AreEqual(x, pos.X);
			Assert.AreEqual(y, pos.Y);
		}

		[Test]
		public void CompareTo()
		{
			// TODO: Issue 11 (https://github.com/Wizcorp/TileSystem/issues/11)
			Assert.Fail();
		}
	}
}
