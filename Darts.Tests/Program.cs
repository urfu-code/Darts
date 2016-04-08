using System.Linq;
using Darts.Domain.Models;
using NUnit.Framework;

namespace Darts.Tests
{
	[TestFixture]
	public class SinglePlayerLeg_should
	{
		[Test]
		public void SplitThrowsByTurns()
		{
			var leg = new SinglePlayerLeg(301)

				.WithAdditionalThrow(ThrowResult.InnerBull)
				.WithAdditionalThrow(ThrowResult.InnerBull)
				.WithAdditionalThrow(ThrowResult.InnerBull)
				.WithAdditionalThrow(ThrowResult.Outside);

			Assert.AreEqual(2, leg.Turns.Count);
			CollectionAssert.AreEqual(new[] { ThrowResult.Outside }, leg.Turns.Last().Throws);
		}

		[Test]
		public void StartNewTurnAfterBust()
		{
			var leg = new SinglePlayerLeg(10)
			
				.WithAdditionalThrow(ThrowResult.InnerBull)
				.WithAdditionalThrow(ThrowResult.Outside);

			Assert.AreEqual(2, leg.Turns.Count);
			CollectionAssert.AreEqual(new[] { ThrowResult.InnerBull }, leg.Turns[0].Throws);
			CollectionAssert.AreEqual(new[] { ThrowResult.Outside }, leg.Turns[1].Throws);
		}
		[Test]
		public void AddTurnScore_IfNotBust()
		{
			var leg = new SinglePlayerLeg(103)

				.WithAdditionalThrow(new ThrowResult(1, SectionArea.Single))
				.WithAdditionalThrow(new ThrowResult(1, SectionArea.Single))
				.WithAdditionalThrow(new ThrowResult(1, SectionArea.Single));

			Assert.AreEqual(100, leg.Score);
		}
		
		[Test]
		public void DoNotAddScore_IfBust()
		{
			var leg = new SinglePlayerLeg(10)

				.WithAdditionalThrow(new ThrowResult(1, SectionArea.Single))
				.WithAdditionalThrow(new ThrowResult(1, SectionArea.Single))
				.WithAdditionalThrow(new ThrowResult(7, SectionArea.Single));

			Assert.AreEqual(10, leg.Score);
		}

	}
}
