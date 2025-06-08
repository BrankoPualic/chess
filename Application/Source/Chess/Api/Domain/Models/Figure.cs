namespace Api.Domain.Models;

public abstract class Figure
{
	protected readonly int[] _rows = [1, 2, 3, 4, 5, 6, 7, 8];
	protected readonly char[] _columns = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];

	public abstract eFigureType Type { get; }

	public Guid Id { get; set; }

	public ePlayerColor Color { get; set; }

	public string Position { get; set; }

	public string PreviousPosition { get; set; }

	public abstract bool IsValidMove(List<Figure> board, Figure figure, string newPosition, Figure lastMovedFigure = null);

	protected bool IsTargetOfTheSameColor(List<Figure> board, Figure figure, string newPosition)
	{
		var targetFigure = board.FirstOrDefault(_ => _.Position == newPosition);
		return targetFigure != null && targetFigure.Color == figure.Color;
	}
}

public class Pawn : Figure
{
	public override eFigureType Type => eFigureType.Pawn;

	public override bool IsValidMove(List<Figure> board, Figure figure, string newPosition, Figure lastMovedFigure = null)
	{
		var (oldRow, oldCol) = (figure.Position.Last(), figure.Position.First());
		var (newRow, newCol) = (newPosition.Last(), newPosition.First());
		var color = figure.Color;

		int direction = color == ePlayerColor.White ? 1 : -1;
		int startRow = color == ePlayerColor.White ? 2 : 7;

		// Forward move
		if (newCol == oldCol)
		{
			if (newRow == oldRow + direction && board.All(_ => _.Position != newPosition))
				return true;

			if (oldRow == startRow && newRow == oldRow + 2 * direction &&
				board.All(_ => _.Position != newPosition) &&
				board.All(_ => _.Position != $"{oldCol}{oldRow + direction}"))
				return true;

			return false;
		}

		// Diagonal capture
		if (Math.Abs(newCol - oldCol) == 1 && newRow == oldRow + direction)
		{
			// Regular capture
			var target = board.FirstOrDefault(_ => _.Position == newPosition);
			if (target != null && target.Color != color)
				return true;

			// En passant
			if (lastMovedFigure != null &&
				lastMovedFigure.Type == eFigureType.Pawn &&
				lastMovedFigure.Color != color &&
				lastMovedFigure.Position == $"{newCol}{oldRow}" && // beside this pawn
				Math.Abs(lastMovedFigure.Position.Last() - oldRow) == 0 && // same row
				(lastMovedFigure.PreviousPosition.Last() - lastMovedFigure.Position.Last()) == 2 * direction) // moved 2
			{
				return true;
			}
		}

		return false;
	}
}

public class Rock : Figure
{
	public override eFigureType Type => eFigureType.Rock;

	public override bool IsValidMove(List<Figure> board, Figure figure, string newPosition, Figure lastMovedFigure = null)
	{
		var (oldRow, oldCol) = (figure.Position.Last(), figure.Position.First());
		var (newRow, newCol) = (newPosition.Last(), newPosition.First());

		// Check that the move is strictly vertical or horizontal
		if (oldRow != newRow && oldCol != newCol)
			return false;

		// Determine direction of movement
		int rowStep = Math.Sign(newRow - oldRow);
		int colStep = Math.Sign(newCol - oldCol);

		// Convert to integers for looping
		int startRow = oldRow - '0';
		int endRow = newRow - '0';
		int startCol = oldCol;
		int endCol = newCol;

		// Check for obstructions between old and new positions (excluding target square)
		if (oldCol == newCol) // Vertical move
		{
			for (int r = startRow + rowStep; r != endRow; r += rowStep)
			{
				string pos = $"{oldCol}{r}";
				if (board.Any(_ => _.Position == pos))
					return false;
			}
		}
		else if (oldRow == newRow) // Horizontal move
		{
			for (char c = (char)(startCol + colStep); c != newCol; c = (char)(c + colStep))
			{
				string pos = $"{c}{oldRow}";
				if (board.Any(f => f.Position == pos))
					return false;
			}
		}

		// Check if target square is occupied by a friendly piece
		return !IsTargetOfTheSameColor(board, figure, newPosition);
	}
}

public class Knight : Figure
{
	public override eFigureType Type => eFigureType.Knight;

	public override bool IsValidMove(List<Figure> board, Figure figure, string newPosition, Figure lastMovedFigure = null)
	{
		var (oldRow, oldCol) = (figure.Position.Last(), figure.Position.First());
		var (newRow, newCol) = (newPosition.Last(), newPosition.First());

		var validMoves = new[]
		{
			($"{(char)(oldCol + 2)}{oldRow + 1}"),
			($"{(char)(oldCol + 2)}{oldRow - 1}"),
			($"{(char)(oldCol - 2)}{oldRow + 1}"),
			($"{(char)(oldCol - 2)}{oldRow - 1}"),
			($"{(char)(oldCol + 1)}{oldRow + 2}"),
			($"{(char)(oldCol + 1)}{oldRow - 2}"),
			($"{(char)(oldCol - 1)}{oldRow + 2}"),
			($"{(char)(oldCol - 1)}{oldRow - 2}")
		};

		if (!validMoves.Contains(newPosition))
			return false;

		return !IsTargetOfTheSameColor(board, figure, newPosition);
	}
}

public class Bishop : Figure
{
	public override eFigureType Type => eFigureType.Bishop;

	public override bool IsValidMove(List<Figure> board, Figure figure, string newPosition, Figure lastMovedFigure = null)
	{
		var (oldRow, oldCol) = (figure.Position.Last(), figure.Position.First());
		var (newRow, newCol) = (newPosition.Last(), newPosition.First());

		if (Math.Abs(oldCol - newCol) != Math.Abs(oldRow - newRow))
			return false;

		// Determine direction of movement
		int rowStep = Math.Sign(newRow - oldRow);
		int colStep = Math.Sign(newCol - oldCol);

		// Convert to integers for looping
		int startRow = oldRow - '0';
		int endRow = newRow - '0';
		int startCol = oldCol;
		int endCol = newCol;

		// Check for obstructions between old and new positions (excluding target square)
		int r = startRow + rowStep;
		char c = (char)(startCol + colStep);
		while (r != endRow)
		{
			string pos = $"{c}{r}";
			if (board.Any(_ => _.Position == pos))
				return false;

			r += rowStep;
			c = (char)(c + colStep);
		}

		// Check if target square is occupied by a friendly piece
		return !IsTargetOfTheSameColor(board, figure, newPosition);
	}
}

public class Queen : Figure
{
	public override eFigureType Type => eFigureType.Queen;

	public override bool IsValidMove(List<Figure> board, Figure figure, string newPosition, Figure lastMovedFigure = null)
	{
		var (oldRow, oldCol) = (figure.Position.Last(), figure.Position.First());
		var (newRow, newCol) = (newPosition.Last(), newPosition.First());

		// Check that the move is strictly vertical, horizontal or diagonal
		if (oldRow != newRow && oldCol != newCol && Math.Abs(oldCol - newCol) != Math.Abs(oldRow - newRow))
			return false;

		// Determine direction of movement
		int rowStep = Math.Sign(newRow - oldRow);
		int colStep = Math.Sign(newCol - oldCol);

		// Convert to integers for looping
		int startRow = oldRow - '0';
		int endRow = newRow - '0';
		int startCol = oldCol;
		int endCol = newCol;

		// Check for obstructions between old and new positions (excluding target square)
		if (oldCol == newCol) // Vertical move
		{
			for (int r = startRow + rowStep; r != endRow; r += rowStep)
			{
				string pos = $"{oldCol}{r}";
				if (board.Any(_ => _.Position == pos))
					return false;
			}
		}
		else if (oldRow == newRow) // Horizontal move
		{
			for (char c = (char)(startCol + colStep); c != newCol; c = (char)(c + colStep))
			{
				string pos = $"{c}{oldRow}";
				if (board.Any(f => f.Position == pos))
					return false;
			}
		}
		else // Diagonal move
		{
			int r = startRow + rowStep;
			char c = (char)(startCol + colStep);
			while (r != endRow)
			{
				string pos = $"{c}{r}";
				if (board.Any(_ => _.Position == pos))
					return false;

				r += rowStep;
				c = (char)(c + colStep);
			}
		}

		// Check if target square is occupied by a friendly piece
		return !IsTargetOfTheSameColor(board, figure, newPosition);
	}
}

public class King : Figure
{
	public override eFigureType Type => eFigureType.King;

	public override bool IsValidMove(List<Figure> board, Figure figure, string newPosition, Figure lastMovedFigure = null)
	{
		var (oldRow, oldCol) = (figure.Position.Last(), figure.Position.First());
		var (newRow, newCol) = (newPosition.Last(), newPosition.First());

		int rowDiff = Math.Abs(oldRow - newRow);
		int colDiff = Math.Abs(oldCol - newCol);

		if (rowDiff > 1 || colDiff > 1)
			return false;

		// Prevent moving next to enemy king
		foreach (var enemyKing in board.Where(_ => _.Type == eFigureType.King && _.Color != figure.Color))
		{
			var (ekRow, ekCol) = (enemyKing.Position.Last(), enemyKing.Position.First());
			if (Math.Abs(newRow - ekRow) <= 1 && Math.Abs(newCol - ekCol) <= 1)
				return false;
		}

		return !IsTargetOfTheSameColor(board, figure, newPosition);
	}
}