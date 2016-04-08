using System.Collections.Concurrent;
using Darts.Domain;
using Darts.Domain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace Darts.Tests
{
	[TestFixture]
	public class Turn_should
	{
		private Turn turn301;

		[SetUp]
		public void SetUp()
		{
			turn301 = new Turn(301);
		}

		[Test]
		public void NotBeFinished_AfterCreation()
		{
			Assert.IsFalse(turn301.Finished);
		}
		[Test]
		public void NotBeBust_AfterCreation()
		{
			Assert.IsFalse(turn301.Bust);
		}

		[Test]
		public void NotBeFinished_AfterTwoThrows()
		{
			var res = turn301
				.WithAdditionalThrow(ThrowResult.InnerBull)
				.WithAdditionalThrow(ThrowResult.InnerBull);
			Assert.IsFalse(res.Finished);
		}

		[Test]
		public void BeBust_IfLastThrowIsNotDouble()
		{
			var turn2 = new Turn(2);
			turn2 = turn2.WithAdditionalThrow(new ThrowResult(2, SectionArea.Single));
			Assert.IsTrue(turn2.Finished);
			Assert.IsTrue(turn2.Bust);
			Assert.AreEqual(2, turn2.ScoreAfter);
		}

		[Test]
		public void BeBust_IfOverscore()
		{
			var turn2 = new Turn(2);
			var res = turn2.WithAdditionalThrow(new ThrowResult(3, SectionArea.Double));
			Assert.IsTrue(res.Finished);
			Assert.IsTrue(res.Bust);
			Assert.AreEqual(2, res.ScoreAfter);
		}

		[Test]
		public void BeBust_IfRemainsOne()
		{
			var turn2 = new Turn(2);
			var res = turn2.WithAdditionalThrow(new ThrowResult(1, SectionArea.Single));
			Assert.IsTrue(res.Finished);
			Assert.IsTrue(res.Bust);
			Assert.AreEqual(2, res.ScoreAfter);
		}

		[Test]
		public void NotBeBust_IfFinishWithDouble()
		{
			var turn2 = new Turn(2);
			var res = turn2.WithAdditionalThrow(new ThrowResult(1, SectionArea.Double));
			Assert.IsTrue(res.Finished);
			Assert.IsFalse(res.Bust);
			Assert.AreEqual(0, res.ScoreAfter);
		}

		[Test]
		public void NotBeBust_IfFinishWithDoubleAfterCoubleOfThrows()
		{
			var turn2 = new Turn(4);
			var res = turn2.WithAdditionalThrow(ThrowResult.Outside)
			.WithAdditionalThrow(new ThrowResult(2, SectionArea.Single))
			.WithAdditionalThrow(new ThrowResult(1, SectionArea.Double));
			Assert.IsTrue(res.Finished);
			Assert.IsFalse(res.Bust);
			Assert.AreEqual(0, res.ScoreAfter);
		}

		[Test]
		public void BeSerializable()
		{
			var turn = new Turn(301)
				.WithAdditionalThrow(ThrowResult.Outside)
				.WithAdditionalThrow(ThrowResult.Single(1))
				.WithAdditionalThrow(ThrowResult.Double(2));
			var res = JsonConvert.SerializeObject(turn, new ThrowJsonConverter());
			Assert.AreEqual(@"{""Throws"":[""0"",""1"",""D2""],""ScoreBefore"":301,""ScoreAfter"":296,""Finished"":true,""Bust"":false}", res, res);
			var deserialized = JsonConvert.DeserializeObject<Turn>(res, new ThrowJsonConverter());
			Assert.AreEqual(turn, deserialized);
		}
	}
}