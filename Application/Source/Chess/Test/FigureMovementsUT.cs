using Api.Domain;
using Api.Domain.Models.Figures;

namespace Test;

[TestFixture]
public class FigureMovementsUT
{
	private List<Figure> _emptyBoard;

	[SetUp]
	public void Setup()
	{
		_emptyBoard = [];
	}

	[Test]
	public void Pawn_ValidMove_ForwardOneStep()
	{
		var pawn = new Pawn { Color = ePlayerColor.White, Position = "e2" };
		Assert.That(pawn.IsValidMove(_emptyBoard, pawn, "e3"), Is.True);
	}

	[Test]
	public void Pawn_InvalidMove_Backwards()
	{
		var pawn = new Pawn { Color = ePlayerColor.White, Position = "e2" };
		Assert.That(pawn.IsValidMove(_emptyBoard, pawn, "e1"), Is.False);
	}

	[Test]
	public void Knight_ValidMove_LShape()
	{
		var knight = new Knight { Color = ePlayerColor.White, Position = "g1" };
		Assert.That(knight.IsValidMove(_emptyBoard, knight, "f3"), Is.True);
	}

	[Test]
	public void Knight_InvalidMove_Straight()
	{
		var knight = new Knight { Color = ePlayerColor.White, Position = "g1" };
		Assert.That(knight.IsValidMove(_emptyBoard, knight, "g3"), Is.False);
	}

	[Test]
	public void Bishop_ValidMove_Diagonal()
	{
		var bishop = new Bishop { Color = ePlayerColor.White, Position = "c1" };
		Assert.That(bishop.IsValidMove(_emptyBoard, bishop, "g5"), Is.True);
	}

	[Test]
	public void Bishop_InvalidMove_Straight()
	{
		var bishop = new Bishop { Color = ePlayerColor.White, Position = "c1" };
		Assert.That(bishop.IsValidMove(_emptyBoard, bishop, "c4"), Is.False);
	}

	[Test]
	public void Queen_ValidMove_Horizontal()
	{
		var queen = new Queen { Color = ePlayerColor.White, Position = "d1" };
		Assert.That(queen.IsValidMove(_emptyBoard, queen, "h1"), Is.True);
	}

	[Test]
	public void Queen_ValidMove_Diagonal()
	{
		var queen = new Queen { Color = ePlayerColor.White, Position = "d1" };
		Assert.That(queen.IsValidMove(_emptyBoard, queen, "h5"), Is.True);
	}

	[Test]
	public void Queen_InvalidMove_LShape()
	{
		var queen = new Queen { Color = ePlayerColor.White, Position = "d1" };
		Assert.That(queen.IsValidMove(_emptyBoard, queen, "e3"), Is.False);
	}

	[Test]
	public void King_ValidMove_OneStepAnyDirection()
	{
		var king = new King { Color = ePlayerColor.White, Position = "e1" };
		Assert.Multiple(() =>
		{
			Assert.That(king.IsValidMove(_emptyBoard, king, "e2"), Is.True);
			Assert.That(king.IsValidMove(_emptyBoard, king, "d1"), Is.True);
			Assert.That(king.IsValidMove(_emptyBoard, king, "f2"), Is.True);
		});
	}

	[Test]
	public void King_InvalidMove_TwoSteps()
	{
		var king = new King { Color = ePlayerColor.White, Position = "e1" };
		Assert.That(king.IsValidMove(_emptyBoard, king, "e3"), Is.False);
	}

	[Test]
	public void Rook_CanMoveVertically_NoObstruction()
	{
		var rook = new Rook { Position = "d4", Color = ePlayerColor.White };
		var board = new List<Figure> { rook };

		Assert.Multiple(() =>
		{
			Assert.That(rook.IsValidMove(board, rook, "d7"), Is.True);
			Assert.That(rook.IsValidMove(board, rook, "d1"), Is.True);
		});
	}

	[Test]
	public void Rook_CannotMoveDiagonally()
	{
		var rook = new Rook { Position = "d4", Color = ePlayerColor.White };
		var board = new List<Figure> { rook };

		Assert.Multiple(() =>
		{
			Assert.That(rook.IsValidMove(board, rook, "e5"), Is.False);
			Assert.That(rook.IsValidMove(board, rook, "c3"), Is.False);
		});
	}

	[Test]
	public void Rook_CannotMoveThroughObstruction()
	{
		var rook = new Rook { Position = "d4", Color = ePlayerColor.White };
		var blockingPawn = new Pawn { Position = "d5", Color = ePlayerColor.White };
		var board = new List<Figure> { rook, blockingPawn };

		Assert.That(rook.IsValidMove(board, rook, "d6"), Is.False);
	}

	[Test]
	public void Rook_CanCaptureOpponent()
	{
		var rook = new Rook { Position = "d4", Color = ePlayerColor.White };
		var enemyPawn = new Pawn { Position = "d7", Color = ePlayerColor.Black };
		var board = new List<Figure> { rook, enemyPawn };

		Assert.That(rook.IsValidMove(board, rook, "d7"), Is.True);
	}

	[Test]
	public void Rook_CannotCaptureOwnPiece()
	{
		var rook = new Rook { Position = "d4", Color = ePlayerColor.White };
		var friendlyPawn = new Pawn { Position = "d7", Color = ePlayerColor.White };
		var board = new List<Figure> { rook, friendlyPawn };

		Assert.That(rook.IsValidMove(board, rook, "d7"), Is.False);
	}
}