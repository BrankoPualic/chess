namespace Api.Domain.Models;

public class Match
{
	public Guid Id { get; set; }

	public string PlayerWhite { get; set; }

	public string PlayerBlack { get; set; }

	public ePlayerTurn PlayerTurn { get; set; }
}