namespace Api.Domain.Models.Figures;

public class Knight : Figure
{
	public override eFigureType Type => eFigureType.Knight;

	public override bool IsValidMove(List<Figure> board, string newPosition, Figure lastMovedFigure = null)
	{
		var (oldRow, oldCol) = (Position.Last() - '0', Position.First());
		var (newRow, newCol) = (newPosition.Last() - '0', newPosition.First());

		var validMoves = new[]
		{
			$"{(char)(oldCol + 2)}{oldRow + 1}",
			$"{(char)(oldCol + 2)}{oldRow - 1}",
			$"{(char)(oldCol - 2)}{oldRow + 1}",
			$"{(char)(oldCol - 2)}{oldRow - 1}",
			$"{(char)(oldCol + 1)}{oldRow + 2}",
			$"{(char)(oldCol + 1)}{oldRow - 2}",
			$"{(char)(oldCol - 1)}{oldRow + 2}",
			$"{(char)(oldCol - 1)}{oldRow - 2}"
		};

		if (!validMoves.Contains(newPosition))
			return false;

		return !IsTargetOfTheSameColor(board, newPosition);
	}
}