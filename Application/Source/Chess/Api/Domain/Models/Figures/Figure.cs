namespace Api.Domain.Models.Figures;

public abstract class Figure
{
	public abstract eFigureType Type { get; }

	public Guid Id { get; set; }

	public ePlayerColor Color { get; set; }

	public string Position { get; set; }

	public string PreviousPosition { get; set; }

	public abstract bool IsValidMove(List<Figure> board, string newPosition, Figure lastMovedFigure = null);

	public bool IsKingInCheckAfterMove(List<Figure> board, string newPosition)
	{
		var clonedBoard = board.Select(_ => _.MemberwiseClone() as Figure).ToList();
		var figureToMove = clonedBoard.First(_ => _.Id == Id);
		figureToMove.PreviousPosition = figureToMove.Position;
		figureToMove.Position = newPosition;

		clonedBoard.RemoveAll(_ => _.Position == newPosition && _.Color != figureToMove.Color);

		var king = clonedBoard.FirstOrDefault(_ => _.Type == eFigureType.King && _.Color == Color);
		return clonedBoard.Any(_ => _.Color != king.Color && _.IsValidMove(clonedBoard, king.Position));
	}

	public bool IsCheckmate(List<Figure> board, Figure lastMovedFigure = null)
	{
		var currentPlayerFigures = board.Where(_ => _.Color == Color).ToList();

		foreach (var figure in currentPlayerFigures)
		{
			foreach (var targetSquare in MatchExtensions.GetAllBoardSquares())
			{
				if (figure.IsValidMove(board, targetSquare, lastMovedFigure))
				{
					var clonedBoard = board.Select(_ => _.MemberwiseClone() as Figure).ToList();
					var clonedFigure = clonedBoard.First(_ => _.Id == figure.Id);
					clonedFigure.Position = targetSquare;

					if (!clonedFigure.IsKingInCheckAfterMove(clonedBoard, targetSquare))
						return false;
				}
			}
		}

		return true;
	}

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