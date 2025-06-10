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

	public static Figure ToModel(this FigureDto data)
	{
		var model = GetFigureInstance(data.Type);
		model.Id = data.Id;
		model.Color = data.Color;
		model.Position = data.Position;
		model.PreviousPosition = data.PreviousPosition;
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