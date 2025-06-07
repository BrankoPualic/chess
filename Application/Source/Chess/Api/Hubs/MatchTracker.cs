using Api.Domain;
using Api.Domain.Models;
using System.Collections.Concurrent;

namespace Api.Hubs;

public class MatchTracker
{
	private static readonly Queue<string> _queue = [];
	private static object _queueLock = new();
	private static readonly ConcurrentDictionary<Guid, Match> _matches = [];
	private static readonly Random _random = new();
	private static readonly object _randomLock = new();

	public bool TryMatchPlayer(string connectionId, out Match? match, out string? opponent)
	{
		lock (_queueLock)
		{
			opponent = _queue.Count > 0 ? _queue.Dequeue() : null;
			if (opponent == null)
			{
				_queue.Enqueue(connectionId);
				match = null;
				return false;
			}
		}

		bool whiteFirst;
		lock (_randomLock)
		{
			whiteFirst = _random.Next(2) == 0;
		}

		match = new Match
		{
			Id = Guid.NewGuid(),
			PlayerWhite = whiteFirst ? opponent : connectionId,
			PlayerBlack = whiteFirst ? connectionId : opponent,
			PlayerTurn = ePlayerTurn.White
		};

		_matches.TryAdd(match.Id, match);
		return true;
	}

	public bool TryRemoveMatch(Guid matchId) => _matches.TryRemove(matchId, out _);
}
