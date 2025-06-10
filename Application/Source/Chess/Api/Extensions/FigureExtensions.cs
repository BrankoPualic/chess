using Api.Domain;
using Api.Domain.Models.Figures;
using Api.Dtos;

namespace Api.Extensions;

public static class FigureExtensions
{
	public static Figure GetFigureInstance(eFigureType type)
	=> type switch
	{
		eFigureType.Pawn => new Pawn(),
		eFigureType.Rook => new Rook(),
		eFigureType.Bishop => new Bishop(),
		eFigureType.Knight => new Knight(),
		eFigureType.Queen => new Queen(),
		eFigureType.King => new King(),
		_ => throw new InvalidOperationException("Invalid figure type")
	};

	public static Figure ToModel(this FigureDto dto)
	{
		var model = GetFigureInstance(dto.Type);
		model.Id = dto.Id;
		model.Color = dto.Color;
		model.Position = dto.Position;
		model.PreviousPosition = dto.PreviousPosition;
		return model;
	}

	public static FigureDto ToDto(this Figure model) => new()
	{
		Id = model.Id,
		Type = model.Type,
		Color = model.Color,
		Position = model.Position,
		PreviousPosition = model.PreviousPosition,
	};
}