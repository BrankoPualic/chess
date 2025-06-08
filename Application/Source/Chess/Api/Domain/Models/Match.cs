namespace Api.Domain.Models;

public class Match
{
	public Guid Id { get; set; }

	public string PlayerWhite { get; set; }

	public string PlayerBlack { get; set; }

	public ePlayerColor PlayerTurn { get; set; }

	public List<Figure> Board { get; set; } = [];
}