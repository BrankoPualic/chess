using Api.Domain.Models;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs;

public interface IMatchmakingHub
{
	Task WaitingForMatch();

	Task MatchFound(Match match);

	Task Conceded();

	Task Victory();

	Task Cancelled();
}

public class MatchmakingHub(MatchTracker matchTracker) : Hub<IMatchmakingHub>
{
	public async Task FindMatch()
	{
		var player = Context.ConnectionId;

		matchTracker.TryMatchPlayer(player, out var match, out var opponent);

		if (match == null)
		{
			await Clients.Client(player).WaitingForMatch();
		}
		else
		{
			await Clients.Clients(player, opponent!).MatchFound(match);
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
}