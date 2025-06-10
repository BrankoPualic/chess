namespace Api.Hubs.Requests;

public class MoveRequest
{
	public MatchDto Match { get; set; }

	public FigureDto MovedFigure { get; set; }

	public string NewPosition { get; set; }

	public FigureDto LastMovedFigure { get; set; } = null;
}