using Api.Domain;
using Api.Domain.Models;

namespace Api.Hubs.Trackers;

public class BoardTracker
{
	public List<Figure> InitializeBoard()
	{
		var rows = new int[] { 1, 2, 7, 8 };
		var cols = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

		var board = new List<Figure>();

		foreach (var row in rows)
		{
			foreach (var col in cols)
			{
				var figure = new Figure
				{
					Color = row is 1 or 2 ? ePlayerColor.White : ePlayerColor.Black,
					Position = $"{col}{row}",
					Type = row switch
					{
						2 or 7 => eFigureType.Pawn,
						_ => col switch
						{
							'a' or 'h' => eFigureType.Rock,
							'b' or 'g' => eFigureType.Knight,
							'c' or 'f' => eFigureType.Bishop,
							'd' => eFigureType.Queen,
							'e' => eFigureType.King,
							_ => throw new InvalidOperationException("Invalid Column"),
						}
					}
				};

				board.Add(figure);
			}
		}

		return board;
	}
}