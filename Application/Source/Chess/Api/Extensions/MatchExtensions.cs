namespace Api.Extensions;

public static class MatchExtensions
{
	public static void CaptureFigure(this MatchDto data, FigureDto figure)
	{
		data.Board.Remove(figure);

		if (figure.Color == ePlayerColor.White)
			data.WhiteCaptures.Add(figure);
		else
			data.BlackCaptures.Add(figure);
	}

	public static List<string> GetAllBoardSquares()
	{
		var squares = new List<string>();

		for (char c = 'a'; c <= 'h'; c++)
			for (var r = 1; r <= 8; r++)
				squares.Add($"{c}{r}");

		return squares;
	}
}