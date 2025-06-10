using Api.Domain;
using Api.Dtos;
using System.Collections.Concurrent;

namespace Api.Hubs.Trackers;

public class MatchTracker(BoardTracker boardTracker)
{
	private static readonly LinkedList<string> _queue = [];
	private static readonly Lock _queueLock = new();
	private static readonly ConcurrentDictionary<Guid, MatchDto> _matches = [];
	private static readonly Random _random = new();
	private static readonly Lock _randomLock = new();

	public bool TryMatchPlayer(string connectionId, out MatchDto? match, out string? opponent)
	{
		lock (_queueLock)
		{
			if (_queue.Count == 0)
			{
				opponent = null;
			}
			else
			{
				opponent = _queue.First?.Value;
				_queue.RemoveFirst();
			}

			if (opponent == null)
			{
				_queue.AddLast(connectionId);
				match = null;
				return false;
			}
		}

		bool whiteFirst;
		lock (_randomLock)
		{
			whiteFirst = _random.Next(2) == 0;
		}

		match = new()
		{
			Id = Guid.NewGuid(),
			PlayerWhite = whiteFirst ? opponent : connectionId,
			PlayerBlack = whiteFirst ? connectionId : opponent,
			PlayerTurn = ePlayerColor.White,
			Board = boardTracker.InitializeBoard()
		};

		_matches.TryAdd(match.Id, match);
		return true;
	}

	public bool TryCancelMatchmaking(string connectionId)
	{
		lock (_queueLock)
		{
			var player = _queue.Find(connectionId);
			if (player == null)
				return false;

			_queue.Remove(player);
			return true;
		}
	}

	public bool TryRemoveMatch(Guid matchId) => _matches.TryRemove(matchId, out _);
}