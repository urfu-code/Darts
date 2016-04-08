using System;
using System.IO;
using Darts.Domain.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Darts.Tests
{
	[TestFixture]
	public class Match_should
	{
		private Match emptyMatch;

		[SetUp]
		public void SetUp()
		{
			emptyMatch = new Match("1", new[] { "p1", "p2" }, 301, 2);
		}

		[Test]
		public void NotBeFinished_AfterCreation()
		{
			Assert.IsFalse(emptyMatch.Finished);
		}
		[Test]
		public void CreateFirstEmptyLeg_AfterCreation()
		{
			CollectionAssert.IsEmpty(emptyMatch.CurrentLeg.GetPlayerTurns(0));
		}
		[Test]
		public void ChangePlayer_AfterTurnFinished()
		{
			var match = new Match("1", new[] { "p1", "p2" }, 301, 2);
			match.AddThrow(ThrowResult.Single(1));
			match.AddThrow(ThrowResult.Single(2));
			match.AddThrow(ThrowResult.Single(3));
			// Player 1 turn is finished

			match.AddThrow(ThrowResult.Single(4));

			Assert.AreEqual(1, match.CurrentLeg.GetPlayerTurns(0).Count);
			Assert.AreEqual(1, match.CurrentLeg.GetPlayerTurns(1).Count);
		}

		[Test]
		public void StartNewLeg_AfterLegFinished()
		{
			var match = new Match("1", new[] { "p1", "p2" }, 20, 2);
			match.AddThrow(ThrowResult.Double(10));
			match.AddThrow(ThrowResult.Double(10));
			Assert.AreEqual(2, match.Legs.Count);
			Assert.IsTrue(match.CurrentLeg.Finished);
		}

		[Test]
		public void BeSerializable()
		{
			var match = new Match("1", new[] { "p1", "p2" }, 20, 2);
			match.AddThrow(ThrowResult.Double(10));
			match.AddThrow(ThrowResult.Double(10));

			var text = JsonConvert.SerializeObject(match, Formatting.Indented, new ThrowJsonConverter());
			Console.WriteLine(text);
			var deserializedMatch = JsonConvert.DeserializeObject<Match>(text, new ThrowJsonConverter());
			var text2 = JsonConvert.SerializeObject(deserializedMatch, Formatting.Indented, new ThrowJsonConverter());
			Console.WriteLine(text2);
			Assert.AreEqual(text, text2);
		}
	}
}