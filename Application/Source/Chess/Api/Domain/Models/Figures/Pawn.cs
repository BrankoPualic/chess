namespace Api.Domain.Models.Figures;

public class Pawn : Figure
{
	public override eFigureType Type => eFigureType.Pawn;

	public override bool IsValidMove(List<Figure> board, string newPosition, Figure lastMovedFigure = null)
	{
		var (oldRow, oldCol) = (Position.Last(), Position.First());
		var (newRow, newCol) = (newPosition.Last(), newPosition.First());

		int direction = Color == ePlayerColor.White ? 1 : -1;
		int startRow = Color == ePlayerColor.White ? 2 : 7;

		// Forward move
		if (newCol == oldCol)
		{
			if (newRow == oldRow + direction && board.All(_ => _.Position != newPosition))
				return true;

			if (oldRow - '0' == startRow && newRow == oldRow + 2 * direction &&
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
			if (target != null && target.Color != Color)
				return true;

			// En passant
			if (lastMovedFigure != null &&
				lastMovedFigure.Type == eFigureType.Pawn &&
				lastMovedFigure.Color != Color &&
				lastMovedFigure.Position == $"{newCol}{oldRow}" && // beside this pawn
				Math.Abs(lastMovedFigure.Position.Last() - oldRow) == 0 && // same row
				lastMovedFigure.PreviousPosition.Last() - lastMovedFigure.Position.Last() == 2 * direction) // moved 2
			{
				return true;
			}
		}

		return false;
	}
}