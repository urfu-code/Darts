using System;
using System.Collections.Immutable;
using System.Linq;
using Darts.Infrastructure;

namespace Darts.Domain.Models
{
	public class SinglePlayerLeg
	{
		public SinglePlayerLeg(int initialScore, ImmutableList<Turn> turns = null)
		{
			if (initialScore <= 0) throw new ArgumentOutOfRangeException("initialScore");
			InitialScore = initialScore;
			Turns = turns ?? ImmutableList<Turn>.Empty;
		}

		public ImmutableList<Turn> Turns { get; private set; }
		public int InitialScore { get; private set; }

		public int Score
		{
			get { return Turns.Any() ? Turns.Last().ScoreAfter : InitialScore; }
		}

		public SinglePlayerLeg WithAdditionalThrow(ThrowResult throwResult)
		{
			if (Score == 0) throw new InvalidOperationException("Leg is over");
			return new SinglePlayerLeg(InitialScore, AddThrowIntoTurns(throwResult));
		}

		private ImmutableList<Turn> AddThrowIntoTurns(ThrowResult result)
		{
			if (!Turns.Any() || Turns.Last().Finished)
				return Turns
					.Add(new Turn(Score).WithAdditionalThrow(result));
			return Turns
				.RemoveAt(Turns.Count - 1)
				.Add(Turns.Last().WithAdditionalThrow(result));
		}

		#region value semantics
		protected bool Equals(SinglePlayerLeg other)
		{
			return Turns.SequenceEqual(other.Turns) && InitialScore == other.InitialScore;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((SinglePlayerLeg)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Turns.ElementwiseHashcode() * 397) ^ InitialScore;
			}
		}
		#endregion
	}
}