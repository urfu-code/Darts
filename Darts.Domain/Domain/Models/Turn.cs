using System;
using System.Collections.Immutable;
using System.Linq;
using Darts.Infrastructure;

namespace Darts.Domain.Models
{
	public class Turn
	{
		public Turn(int scoreBefore, ImmutableList<ThrowResult> throws = null)
		{
			if (scoreBefore <= 0) throw new ArgumentOutOfRangeException("scoreBefore");
			ScoreBefore = scoreBefore;
			Throws = throws ?? ImmutableList<ThrowResult>.Empty;
			var gainScore = Throws.Sum(t => t.Score);
			Bust = gainScore == ScoreBefore - 1
						|| gainScore > ScoreBefore
						|| gainScore == ScoreBefore && Throws.Last().SectionArea != SectionArea.Double;
			ScoreAfter = Bust ? ScoreBefore : ScoreBefore - Throws.Sum(t => t.Score);
			Finished = Throws.Count == 3 || Bust || ScoreAfter == 0;
		}

		public ImmutableList<ThrowResult> Throws { get; private set; }
		public int ScoreBefore { get; private set; }
		public int ScoreAfter { get; private set; }
		public bool Finished { get; private set; }
		public bool Bust { get; private set; }

		public Turn WithAdditionalThrow(ThrowResult throwResult)
		{
			if (throwResult == null) throw new ArgumentNullException("throwResult");
			if (Finished) throw new InvalidOperationException("Turn is over already.");
			return new Turn(ScoreBefore, Throws.Add(throwResult));
		}


		#region value semantics

		protected bool Equals(Turn other)
		{
			return Throws.SequenceEqual(other.Throws) && ScoreBefore == other.ScoreBefore;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Turn) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Throws.ElementwiseHashcode()*397) ^ ScoreBefore;
			}
		}

		#endregion
	}
}