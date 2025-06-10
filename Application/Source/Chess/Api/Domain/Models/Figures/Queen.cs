namespace Api.Domain.Models.Figures;

public class Queen : Figure
{
	public override eFigureType Type => eFigureType.Queen;

	public override bool IsValidMove(List<Figure> board, string newPosition, Figure lastMovedFigure = null)
	{
		var (oldCol, oldRow) = Position;
		var (newCol, newRow) = newPosition;

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

		if (HasObstruction(board, rowStep, colStep, startRow, endRow, startCol, endCol))
			return false;

		// Check if target square is occupied by a friendly piece
		return !IsTargetOfTheSameColor(board, newPosition);
	}
}