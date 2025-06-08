using Api.Domain;
using Api.Domain.Models;

namespace Api.Hubs.Trackers;

public class BoardTracker
{
	public List<Figure> InitializeBoard()
	{
		var rows = new int[] { 1, 2, 7, 8 };
		var cols = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

		var board = new List<Figure>();

		foreach (var row in rows)
		{
			foreach (var col in cols)
			{
				Figure figure = (row, col) switch
				{
					(2 or 7, _) => new Pawn(),
					(_, 'a' or 'h') => new Rock(),
					(_, 'b' or 'g') => new Knight(),
					(_, 'c' or 'f') => new Bishop(),
					(_, 'd') => new Queen(),
					(_, 'e') => new King(),
					_ => throw new InvalidOperationException("Invalid piece")
				};

				figure.Id = Guid.NewGuid();
				figure.Color = row is 1 or 2 ? ePlayerColor.White : ePlayerColor.Black;
				figure.Position = $"{col}{row}";
				figure.PreviousPosition = $"{col}{row}";

				board.Add(figure);
			}
		}

		return board;
	}

	public void MoveFigure(Match match, Figure movedFigure, string newPosition)
	{
		var board = match.Board;
		var previousFigureState = board.Where(_ => _.Type == movedFigure.Type && _.Color == movedFigure.Color && _.Position == movedFigure.Position).FirstOrDefault();

		if (previousFigureState == null)
			throw new InvalidOperationException("Figure doesn't exist");

		previousFigureState.Position = newPosition;
		match.PlayerTurn = match.PlayerTurn == ePlayerColor.White ? ePlayerColor.Black : ePlayerColor.White;
	}

	public bool IsValidMove(List<Figure> board, Figure movedFigure, string newPosition, Figure lastMovedFigure = null)
	{
		var validator = GetFigureInstance(movedFigure.Type);
		// Reuse the data from movedFigure
		validator.Color = movedFigure.Color;
		validator.Position = movedFigure.Position;
		validator.PreviousPosition = movedFigure.PreviousPosition;

		return validator.IsValidMove(board, movedFigure, newPosition, lastMovedFigure);
	}

	public static Figure GetFigureInstance(eFigureType type)
	=> type switch
	{
		eFigureType.Pawn => new Pawn(),
		eFigureType.Rock => new Rock(),
		eFigureType.Bishop => new Bishop(),
		eFigureType.Knight => new Knight(),
		eFigureType.Queen => new Queen(),
		eFigureType.King => new King(),
		_ => throw new InvalidOperationException("Invalid figure type")
	};
}