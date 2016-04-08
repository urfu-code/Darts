using System.Collections.Generic;
using System.IO;
using System.Linq;
using Darts.Domain.Models;
using Newtonsoft.Json;

namespace Darts.Domain.Repositories
{
	public class MatchRepository : IMatchRepository
	{
		private readonly DirectoryInfo matchesDirectory;

		public MatchRepository(DirectoryInfo matchesDirectory)
		{
			this.matchesDirectory = matchesDirectory;
		}

		public List<Match> GetMatches()
		{
			return matchesDirectory.GetFiles("*.json")
				.Select(f => f.FullName)
				.Select(File.ReadAllText)
				.Select(DeserializeMatch)
				.ToList();
		}

		public Match FindMatchById(string id)
		{
			var filename = GetFilename(id);
			if (!File.Exists(filename)) return null;
			return DeserializeMatch(File.ReadAllText(filename));
		}

		public void SaveOrUpdate(Match match)
		{
			File.WriteAllText(GetFilename(match.Id), Serialize(match));
		}

		private string GetFilename(string matchId)
		{
			return Path.Combine(matchesDirectory.FullName, matchId + ".json");
		}

		private static string Serialize(Match match)
		{
			return JsonConvert.SerializeObject(match, Formatting.Indented, new ThrowJsonConverter());
		}

		private static Match DeserializeMatch(string text)
		{
			return JsonConvert.DeserializeObject<Match>(text, new ThrowJsonConverter());
		}
	}
}
