namespace Api.Domain.Models.Figures;

public class King : Figure
{
	public override eFigureType Type => eFigureType.King;

	public override bool IsValidMove(List<Figure> board, string newPosition, Figure lastMovedFigure = null)
	{
		var (oldRow, oldCol) = (Position.Last(), Position.First());
		var (newRow, newCol) = (newPosition.Last(), newPosition.First());

		int rowDiff = Math.Abs(oldRow - newRow);
		int colDiff = Math.Abs(oldCol - newCol);

		if (rowDiff > 1 || colDiff > 1)
			return false;

		// Prevent moving next to enemy king
		foreach (var enemyKing in board.Where(_ => _.Type == eFigureType.King && _.Color != Color))
		{
			var (ekRow, ekCol) = (enemyKing.Position.Last(), enemyKing.Position.First());
			if (Math.Abs(newRow - ekRow) <= 1 && Math.Abs(newCol - ekCol) <= 1)
				return false;
		}

		return !IsTargetOfTheSameColor(board, newPosition);
	}
}