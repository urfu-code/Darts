using System;
using Darts.Domain.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Darts.Tests
{
	[TestFixture]
	public class Leg_should
	{
		[Test]
		public void NotFinish_IfBothPlayersHasNonZeroScore()
		{
			var leg = new Leg("id", 20);
			leg.AddThrow(ThrowResult.Single(10));
			Assert.IsFalse(leg.Finished);
			Assert.AreEqual(-1, leg.WinnerIndex);
		}

		[Test]
		public void Finish_IfOnlyOneHasNonZeroScore()
		{
			var leg = new Leg("id", 20);
			leg.AddThrow(ThrowResult.Triple(20)); //0 bust!
			leg.AddThrow(ThrowResult.Double(10)); //1 finished
			Assert.IsTrue(leg.Finished);
			Assert.AreEqual(1, leg.WinnerIndex);
		}

		[Test]
		public void BeSerializable()
		{
			var leg = new Leg("1", 20);
			leg.AddThrow(ThrowResult.Single(10));//#1
			leg.AddThrow(ThrowResult.Triple(20));//#1 bust!
			leg.AddThrow(ThrowResult.Double(10));//#2 finished

			var text = JsonConvert.SerializeObject(leg, Formatting.Indented, new ThrowJsonConverter());
			Console.WriteLine(text);
			var deserialized = JsonConvert.DeserializeObject<Leg>(text, new ThrowJsonConverter());
			var text2 = JsonConvert.SerializeObject(deserialized, Formatting.Indented, new ThrowJsonConverter());
			Console.WriteLine(text2);
			Assert.AreEqual(text, text2);

		}
	}
}