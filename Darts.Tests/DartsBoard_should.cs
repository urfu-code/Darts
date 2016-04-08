using System;
using Darts.Domain.Models.Dartboard;
using NUnit.Framework;

namespace Darts.Tests
{
	[TestFixture]
	public class DartsBoard_should
	{
		[TestCase(20, 0, 1)]
		[TestCase(3, 0, -1)]
		[TestCase(6, 1, 0)]
		[TestCase(11, -1, 0)]
		[TestCase(4, 10, 9)]
		[TestCase(18, 9, 10)]
		public void CalculateSectorValueByCoordinates(int expectedSectorValue, int x, int y)
		{
			var board = new Dartboard();
			Section section = board.GetSector(x, y);
			Assert.AreEqual(expectedSectorValue, section.Value);
		}

		[TestCase("D25", 0, 0)]
		[TestCase("25", 0, 8)]
		[TestCase("20", 0, 16)]
		[TestCase("T20", 0, 105)]
		[TestCase("D20", 0, 165)]
		public void CalculateThrowResult(string expected, int x, int y)
		{
			var board = new Dartboard();
			var res = board.GetResult(x, y);
			Assert.AreEqual(expected, res.ToString());
		}

		[Test]
		public void BeFilledByItsSectors()
		{
			var board = new Dartboard();
			var ss = board.Sectors;
			for (int i = 0; i < 20; i++)
			{
				var sector = ss[i];
				Console.WriteLine(sector);
				var nextSector = ss[(i+1)%ss.Count];
				Assert.AreEqual(sector.StartAngle, nextSector.EndAngle, sector + ", " + nextSector);
			}
		}
	}
}