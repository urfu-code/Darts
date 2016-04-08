using System;
using System.IO;
using System.Linq;
using Darts.Domain.Models;
using Darts.Domain.Repositories;

namespace Darts.App
{
	public class DartsConsoleUi
	{
		private readonly MatchRepository repo;

		public DartsConsoleUi()
		{
			repo = new MatchRepository(new DirectoryInfo("matches"));
		}

		public void Run()
		{
			while (true)
			{
				var choice = PromptMainMenuChoice();
				if (choice == 'L')
					ListMatches();
				if (choice == 'N')
					NewMatch();
				if (choice == 'D')
					MatchDetails();
				if (choice == 'Q')
					break;
			}
			// ReSharper disable once FunctionNeverReturns
		}

		private void MatchDetails()
		{
			ListMatches();
			var id = Prompt("Enter match id");
			var match = repo.FindMatchById(id);
			if (match == null)
				Console.WriteLine("Unknown match with id " + id);
			else
				ShowMatchSummary(match);
		}

		private static void ShowMatchSummary(Match match)
		{
			Console.WriteLine("Total".PadLeft(20) + " | " + string.Join("  ", match.Legs.Select((leg, i) => i + 1)));
			var playerIndex = 0;
			foreach (var player in match.PlayerNames)
			{
				var winsCount = match.Legs.Count(l => l.Finished && l.WinnerIndex == playerIndex);
				// ReSharper disable once AccessToModifiedClosure (playerIndex)
				var legsScores = string.Join("  ", match.Legs.Select(l => l.Finished && l.WinnerIndex == playerIndex ? "1" : "0"));
				Console.WriteLine(player.PadLeft(10) + " " + winsCount.ToString().PadLeft(9) + " | " + legsScores);
				playerIndex++;
			}
		}

		private void NewMatch()
		{
			var players1 = Prompt("Enter player 1 name");
			var players2 = Prompt("Enter player 2 name");
			var initialScore = Prompt("Enter initial score", IsValidInt, int.Parse, "301");
			var legsCount = Prompt("Enter legs count", IsValidInt, int.Parse, "3");
			var match = new Match(DateTime.Now.Ticks.ToString(), new[]{players1, players2}, initialScore, legsCount);
			EnterMatch(match);
			repo.SaveOrUpdate(match);
		}

		private void EnterMatch(Match match)
		{
			while (!match.Finished)
			{
				ShowCurrentLegDetails(match);
				var throwResult = Prompt("Enter next throw result", ThrowResult.IsValid, ThrowResult.Parse, "");
				match.AddThrow(throwResult);
			}
			Console.WriteLine("Match is finished! Player {0} wins!", match.Winner);
			ShowMatchSummary(match);
			Console.WriteLine();
		}

		private void ShowCurrentLegDetails(Match match)
		{
			var leg = match.CurrentLeg;
			var legIndex = match.Legs.Count - 1;
			foreach (string name in match.PlayerNames)
			{
				var legPlayerIndex = match.GetLegPlayerIndex(name, legIndex);
				var scores = new[] {match.InitialScore}.Concat(leg.GetPlayerTurns(legPlayerIndex).Select(t => t.ScoreAfter));
				Console.WriteLine(name.PadLeft(10) + " " + string.Join(" ", scores.Select(s => s.ToString().PadLeft(3))));
			}
			if (!leg.Finished)
				Console.WriteLine("Current player: " + match.GetPlayerName(legIndex, leg.CurrentPlayerIndex) + ", score: " +
				                  leg.CurrentPlayer.Score);
			else
			{
				Console.WriteLine("Finished");
			}
		}

		private bool IsValidInt(string text)
		{
			int v;
			return int.TryParse(text, out v);
		}

		private string Prompt(string userPrompt)
		{
			return Prompt(userPrompt, s => true, s => s, "");
		}

		private T Prompt<T>(string userPrompt, Func<string, bool> isValid, Func<string, T> parse, string defaultValue)
		{
			while(true)
			{
				Console.Write(userPrompt + (defaultValue != "" ? (" (" + defaultValue + ")") : "") + ": ");
				var text = Console.ReadLine();
				if (string.IsNullOrEmpty(text)) text = defaultValue;
				if (isValid(text)) return parse(text);
			}
		}

		private char PromptMainMenuChoice()
		{
			Console.WriteLine("L. List of stored matches");
			Console.WriteLine("N. New match");
			Console.WriteLine("D. Details of the specific match");
			Console.WriteLine("Q. Quit");
			var key = Console.ReadKey(true);
			return char.ToUpper(key.KeyChar);
		}

		private void ListMatches()
		{
			Console.WriteLine();
			Console.WriteLine("List of matches:");
			foreach (var match in repo.GetMatches())
				Console.WriteLine("Id: {0}, Players: {1}", match.Id, string.Join(", ", match.PlayerNames));
			Console.WriteLine();
		}
	}
}