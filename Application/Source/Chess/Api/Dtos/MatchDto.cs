using Api.Domain;

namespace Api.Dtos;

public class MatchDto
{
	public Guid Id { get; set; }

	public string PlayerWhite { get; set; }

	public string PlayerBlack { get; set; }

	public ePlayerColor PlayerTurn { get; set; }

	public List<FigureDto> Board { get; set; } = [];
}