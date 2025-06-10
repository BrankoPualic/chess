namespace Api.Extensions;

public static class PositionExtensions
{
	public static void Deconstruct(this string position, out char col, out int row)
	{
		if (position.Length <= 1)
			throw new InvalidOperationException("Position is invalid");

		col = position[0];
		row = int.TryParse(position.AsSpan(1), out int _row) ? _row : 1;
	}
}