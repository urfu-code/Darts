using System;
using System.Collections.Generic;
using System.Linq;
using Darts.Infrastructure;
using Newtonsoft.Json;

namespace Darts.Domain.Models
{
	public class Match
	{
		public Match(string id, IReadOnlyList<string> playerNames, int initialScore, int maxLegsCount)
		{
			if (id == null) throw new ArgumentNullException("id");
			if (playerNames == null || playerNames.Count != 2) throw new ArgumentException("Should be two players exactly");
			if (initialScore <= 0) throw new ArgumentOutOfRangeException("initialScore");
			if (maxLegsCount <= 0) throw new ArgumentOutOfRangeException("maxLegsCount");
			Id = id;
			PlayerNames = playerNames;
			InitialScore = initialScore;
			MaxLegsCount = maxLegsCount;
			StartNewLeg();
		}

		public string Id { get; private set; }
		public IReadOnlyList<string> PlayerNames { get; private set; }
		public int InitialScore { get; private set; }
		public int MaxLegsCount { get; private set; }
		
		[JsonProperty]
		private readonly List<Leg> legs = new List<Leg>();
		
		[JsonIgnore]
		public IReadOnlyList<Leg> Legs {
			get { return legs.AsReadOnly(); }
		}
	
		public bool Finished
		{
			get { return legs.Count >= MaxLegsCount && legs[MaxLegsCount - 1].Finished; }
		}

		[JsonIgnore]
		public Leg CurrentLeg
		{
			get
			{
				return legs.Last();
			}
		}

		[JsonIgnore]
		public string Winner
		{
			get
			{
				return Legs.Select((leg, legIndex) => GetPlayerName(legIndex, leg.WinnerIndex))
					.GroupBy(name => name)
					.OrderByDescending(g => g.Count())
					.First().Key;
			}
		}

		public void AddThrow(ThrowResult throwResult)
		{
			if (Finished) throw new InvalidOperationException("Match is finished");
			CurrentLeg.AddThrow(throwResult);
			if (CurrentLeg.Finished && Legs.Count < MaxLegsCount) StartNewLeg();
		}

		private void StartNewLeg()
		{
			legs.Add(new Leg(Guid.NewGuid().ToString("N"), InitialScore));
		}

		public string GetPlayerName(int legIndex, int legPlayerIndex)
		{
			return PlayerNames[(legIndex + legPlayerIndex) % PlayerNames.Count];
		}

		public int GetLegPlayerIndex(string playerName, int legIndex)
		{
			return (PlayerNames.IndexOf(playerName) + PlayerNames.Count - legIndex) % PlayerNames.Count;
		}

		#region Entity semantics
		protected bool Equals(Match other)
		{
			return string.Equals(Id, other.Id);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Match) obj);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("Match({0})", Id);
		}

		#endregion
	}
}