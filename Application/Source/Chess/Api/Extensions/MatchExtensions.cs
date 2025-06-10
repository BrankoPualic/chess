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
}