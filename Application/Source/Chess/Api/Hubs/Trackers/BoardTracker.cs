using Api.Hubs.Requests;

namespace Api.Hubs.Trackers;

public class BoardTracker
{
	public List<FigureDto> InitializeBoard()
	{
		var rows = new int[] { 1, 2, 7, 8 };
		var cols = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

		var board = new List<FigureDto>();

		foreach (var row in rows)
		{
			foreach (var col in cols)
			{
				Figure figure = (row, col) switch
				{
					(2 or 7, _) => new Pawn(),
					(_, 'a' or 'h') => new Rook(),
					(_, 'b' or 'g') => new Knight(),
					(_, 'c' or 'f') => new Bishop(),
					(_, 'd') => new Queen(),
					(_, 'e') => new King(),
					_ => throw new InvalidOperationException("Invalid piece")
				};

				figure.Id = Guid.NewGuid();
				figure.Color = row is 1 or 2 ? ePlayerColor.White : ePlayerColor.Black;
				figure.Position = $"{col}{row}";
				figure.PreviousPosition = $"{col}{row}";

				board.Add(figure.ToDto());
			}
		}

		return board;
	}

	public void MoveFigure(MoveRequest request)
	{
		var figureOnBoard = request.Match.Board.FirstOrDefault(_ => _.Id == request.MovedFigure.Id);

		if (figureOnBoard == null)
			throw new InvalidOperationException("Figure doesn't exist");

		// Capture by En Passant
		if (request.MovedFigure.Type == eFigureType.Pawn &&
		  (request.MovedFigure.ToModel() as Pawn).IsEnPassantMove(request.NewPosition, request.LastMovedFigure?.ToModel()))
		{
			var enPassantTarget = request.Match.Board.FirstOrDefault(_ => _.Position == request.LastMovedFigure.Position);

			if (enPassantTarget != null)
				request.Match.CaptureFigure(enPassantTarget);
		}

		// Capture by position
		var figureCaptured = request.Match.Board.FirstOrDefault(_ => _.Position == request.NewPosition && _.Color != request.MovedFigure.Color);
		if (figureCaptured != null)
			request.Match.CaptureFigure(figureCaptured);

		figureOnBoard.PreviousPosition = figureOnBoard.Position;
		figureOnBoard.Position = request.NewPosition;

		request.MovedFigure.PreviousPosition = figureOnBoard.PreviousPosition;
		request.MovedFigure.Position = request.NewPosition;

		request.Match.PlayerTurn = request.Match.PlayerTurn == ePlayerColor.White
			? ePlayerColor.Black
			: ePlayerColor.White;
	}

	public bool IsValidMove(MoveRequest request) => request.MovedFigure.ToModel().IsValidMove(
		GetBoardFiguresFromRequest(request),
		request.NewPosition,
		request.LastMovedFigure?.ToModel()
	);

	public bool IsKingInCheckAfterMove(MoveRequest request)
		=> request.MovedFigure.ToModel().IsKingInCheckAfterMove(GetBoardFiguresFromRequest(request), request.NewPosition);

	public void CheckForCheckmate(MoveRequest request)
	{
		var boardAfterMove = GetBoardFiguresFromRequest(request);
		var opponentKing = boardAfterMove.FirstOrDefault(_ => _.Type == eFigureType.King && _.Color != request.MovedFigure.Color);

		var isCheckmate = opponentKing?.IsCheckmate(boardAfterMove, request.MovedFigure.ToModel()) == true;

		if (isCheckmate)
		{
			request.Match.IsCheckmate = true;
			request.Match.PlayerVictorious = request.Match.PlayerTurn == ePlayerColor.White
				? request.Match.PlayerBlack
				: request.Match.PlayerWhite;
		}
	}

	private static List<Figure> GetBoardFiguresFromRequest(MoveRequest request)
		=> request.Match.Board.Select(_ => _.ToModel()).ToList();
}