using Darts.Domain;
using Darts.Domain.Models;
using NUnit.Framework;

namespace Darts.Tests
{
	[TestFixture]
	public class LegPlayerIntegrationTest
	{
		[Test]
		public void PlayMatch()
		{
			var leg = new SinglePlayerLeg(126)

				// Turn #1
				.WithAdditionalThrow(ThrowResult.Triple(20))
				.WithAdditionalThrow(ThrowResult.Double(20))
				.WithAdditionalThrow(ThrowResult.Single(20))
				// Score = 6

				// Turn #2
				.WithAdditionalThrow(ThrowResult.Outside)
				.WithAdditionalThrow(ThrowResult.Outside)
				.WithAdditionalThrow(ThrowResult.Outside)

				// Turn #3. Bust! Not double!
				.WithAdditionalThrow(ThrowResult.Triple(2))

				// Turn #4. Bust! Too many!
				.WithAdditionalThrow(ThrowResult.InnerBull)

				// Turn #5. Bust! Can't leave 1.
				.WithAdditionalThrow(ThrowResult.Single(5))

				// Turn #6. Finished
				.WithAdditionalThrow(ThrowResult.Double(3));

			Assert.AreEqual(6, leg.Turns.Count);
			Assert.AreEqual(0, leg.Score);
		}
	}
}