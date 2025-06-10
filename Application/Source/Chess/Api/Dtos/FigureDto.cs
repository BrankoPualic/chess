using Api.Domain;

namespace Api.Dtos;

public class FigureDto
{
	public Guid Id { get; set; }

	public eFigureType Type { get; set; }

	public ePlayerColor Color { get; set; }

	public string Position { get; set; }

	public string PreviousPosition { get; set; }
}