using System.Collections.Generic;
using Darts.Domain.Models;

namespace Darts.Domain.Repositories
{
	public interface IMatchRepository
	{
		List<Match> GetMatches();
		Match FindMatchById(string id);
		void SaveOrUpdate(Match match);
	}
}