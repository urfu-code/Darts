using Darts.Domain.Models;
using NUnit.Framework;

namespace Darts.Tests
{
	[TestFixture]
	public class ThrowResult_should
	{
		[TestCase("11", 11, SectionArea.Single)]
		[TestCase("1", 1, SectionArea.Single)]
		[TestCase("d2", 2, SectionArea.Double)]
		[TestCase("D12", 12, SectionArea.Double)]
		[TestCase("t13", 13, SectionArea.Triple)]
		[TestCase("t3", 3, SectionArea.Triple)]
		public void Parse(string input, int expectedSector, SectionArea expedtedMultiplier)
		{
			var t = ThrowResult.Parse(input);
			Assert.AreEqual(new ThrowResult(expectedSector, expedtedMultiplier), t);
		}

		[TestCase("21", false)]
		[TestCase("q1", false)]
		[TestCase("-1", false)]
		[TestCase("111", false)]
		[TestCase("0", true)]
		[TestCase("1", true)]
		[TestCase("d2", true)]
		[TestCase("D12", true)]
		[TestCase("t3", true)]
		[TestCase("T13", true)]
		[TestCase("1", true)]
		public void ValidateInput(string input, bool expectedIsValid)
		{
			Assert.AreEqual(expectedIsValid, ThrowResult.IsValid(input));
		}
	}
}