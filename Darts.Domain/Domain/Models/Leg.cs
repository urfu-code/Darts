using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Darts.Domain.Models
{
	public class Leg
	{
		public Leg(string id, int initialScore)
		{
			if (id == null) throw new ArgumentNullException("id");
			if (initialScore <= 0) throw new ArgumentOutOfRangeException("initialScore");
			Id = id;
			InitialScore = initialScore;
			var emptyLeg = new SinglePlayerLeg(initialScore);
			players = new List<SinglePlayerLeg> { emptyLeg, emptyLeg };
			WinnerIndex = -1;
			CurrentPlayerIndex = 0;
		}

		public string Id { get; private set; }
		
		public int InitialScore { get; set; }

		[JsonProperty]
		private readonly List<SinglePlayerLeg> players;

		[JsonIgnore]
		public IReadOnlyList<SinglePlayerLeg> Players
		{
			get { return players.AsReadOnly(); }
		}

		[JsonProperty]
		public int CurrentPlayerIndex { get; private set; }

		[JsonProperty]
		public int WinnerIndex { get; private set; }

		public bool Finished
		{
			get { return WinnerIndex >= 0; }
		}

		[JsonIgnore]
		public SinglePlayerLeg CurrentPlayer
		{
			get { return players[CurrentPlayerIndex]; }
			private set { players[CurrentPlayerIndex] = value; }
		}

		public void AddThrow(ThrowResult throwResult)
		{
			if (Finished) throw new InvalidOperationException("Leg is finished");
			CurrentPlayer = CurrentPlayer.WithAdditionalThrow(throwResult);
			if (!CurrentPlayer.Turns.Last().Finished) return;
			if (CurrentPlayer.Score == 0)
				WinnerIndex = CurrentPlayerIndex;
			else
				CurrentPlayerIndex = (CurrentPlayerIndex + 1) % 2;
		}
	
		public IReadOnlyList<Turn> GetPlayerTurns(int playerIndex)
		{
			return players[playerIndex].Turns;
		}

		#region Entity semantics

		protected bool Equals(Leg other)
		{
			return string.Equals(Id, other.Id);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Leg) obj);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("Leg({0})", Id);
		}

		#endregion
	}
}