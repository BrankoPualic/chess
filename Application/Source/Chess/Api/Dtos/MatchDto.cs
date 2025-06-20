﻿namespace Api.Dtos;

public class MatchDto
{
	public Guid Id { get; set; }

	public string PlayerWhite { get; set; }

	public string PlayerBlack { get; set; }

	public ePlayerColor PlayerTurn { get; set; }

	public bool IsCheckmate { get; set; }

	public string PlayerVictorious { get; set; }

	public List<FigureDto> Board { get; set; } = [];

	public List<FigureDto> BlackCaptures { get; set; } = [];

	public List<FigureDto> WhiteCaptures { get; set; } = [];
}