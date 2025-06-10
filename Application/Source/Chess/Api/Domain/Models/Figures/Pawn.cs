namespace Api.Domain.Models.Figures;

public class Pawn : Figure
{
	public override eFigureType Type => eFigureType.Pawn;

	public override bool IsValidMove(List<Figure> board, string newPosition, Figure lastMovedFigure = null)
	{
		var (oldCol, oldRow) = Position;
		var (newCol, newRow) = newPosition;

		int direction = Color == ePlayerColor.White ? 1 : -1;
		int startRow = Color == ePlayerColor.White ? 2 : 7;

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
			if (target != null && target.Color != Color)
				return true;

			if (IsEnPassantMove(newPosition, lastMovedFigure))
				return true;
		}

		return false;
	}

	public bool IsEnPassantMove(string newPosition, Figure lastMovedFigure = null)
	{
		if (lastMovedFigure == null)
			return false;

		var (_, oldRow) = Position;
		var (newCol, newRow) = newPosition;
		int direction = Color == ePlayerColor.White ? 1 : -1;

		return lastMovedFigure.Type == eFigureType.Pawn &&
		   lastMovedFigure.Color != Color &&
		   lastMovedFigure.Position == $"{newCol}{oldRow}" &&
		   Math.Abs(lastMovedFigure.PreviousPosition.Last() - lastMovedFigure.Position.Last()) == 2 * direction &&
		   newRow == oldRow + direction;
	}
}