using Api.Domain.Models;
using Api.Hubs.Trackers;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs;

public interface IMatchmakingHub
{
	Task WaitingForMatch(string player);

	Task MatchFound(Match match, string player);

	Task Conceded();

	Task Victory();

	Task Cancelled();

	Task Moved(Match match);
}

public class MatchmakingHub(MatchTracker matchTracker, BoardTracker boardTracker) : Hub<IMatchmakingHub>
{
	public async Task FindMatch()
	{
		var player = Context.ConnectionId;

		matchTracker.TryMatchPlayer(player, out var match, out var opponent);

		if (match == null)
		{
			await Clients.Client(player).WaitingForMatch(player);
		}
		else
		{
			await Clients.Clients(player, opponent!).MatchFound(match, player);
		}
	}

	public async Task ConcedeMatch(Match match)
	{
		var player = Context.ConnectionId;

		if (matchTracker.TryRemoveMatch(match.Id))
		{
			if (player == match.PlayerWhite)
			{
				await Clients.Client(match.PlayerWhite).Conceded();
				await Clients.Client(match.PlayerBlack).Victory();
			}
			else
			{
				await Clients.Client(match.PlayerBlack).Conceded();
				await Clients.Client(match.PlayerWhite).Victory();
			}
		}
	}

	public async Task Cancel()
	{
		var player = Context.ConnectionId;

		matchTracker.TryCancelMatchmaking(player);

		await Clients.Client(player).Cancelled();
	}

	public async Task Move(Match match, Figure movedFigure, string newPosition, Figure lastMovedFigure = null)
	{
		// Validate move
		if (!boardTracker.IsValidMove(match.Board, movedFigure, newPosition, lastMovedFigure))
			throw new InvalidOperationException("Your move is invalid");

		// Process move
		boardTracker.MoveFigure(match, movedFigure, newPosition);

		// Send back updated match details
		await Clients.Clients(match.PlayerBlack, match.PlayerWhite).Moved(match);
	}
}