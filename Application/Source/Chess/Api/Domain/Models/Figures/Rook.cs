namespace Api.Domain.Models.Figures;

public class Rook : Figure
{
	public override eFigureType Type => eFigureType.Rook;

	public override bool IsValidMove(List<Figure> board, string newPosition, Figure lastMovedFigure = null)
	{
		var (oldRow, oldCol) = (Position.Last(), Position.First());
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
		if (HasObstruction(board, rowStep, colStep, startRow, endRow, startCol, endCol))
			return false;

		// Check if target square is occupied by a friendly piece
		return !IsTargetOfTheSameColor(board, newPosition);
	}
}