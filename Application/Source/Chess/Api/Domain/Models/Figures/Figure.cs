namespace Api.Domain.Models.Figures;

public abstract class Figure
{
	public abstract eFigureType Type { get; }

	public Guid Id { get; set; }

	public ePlayerColor Color { get; set; }

	public string Position { get; set; }

	public string PreviousPosition { get; set; }

	public abstract bool IsValidMove(List<Figure> board, string newPosition, Figure lastMovedFigure = null);

	protected bool IsTargetOfTheSameColor(List<Figure> board, string newPosition)
	{
		var targetFigure = board.FirstOrDefault(_ => _.Position == newPosition);
		return targetFigure != null && targetFigure.Color == Color;
	}

	protected bool HasObstruction(List<Figure> board, int rowStep, int colStep, int startRow, int endRow, int startCol, int endCol)
	{
		int r = startRow + rowStep;
		char c = (char)(startCol + colStep);

		while (r != endRow || c != endCol)
		{
			string pos = $"{c}{r}";
			if (board.Any(_ => _.Position == pos))
				return true;

			if (r != endRow) r += rowStep;
			if (c != endCol) c = (char)(c + colStep);
		}

		return false;
	}
}