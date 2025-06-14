﻿using Api.Domain;
using Api.Dtos;
using Api.Extensions;
using Api.Hubs.Trackers;

namespace Test;

public class BoardUT
{
	private List<FigureDto> _board = [];
	private BoardTracker _boardTracker;

	[SetUp]
	public void Setup()
	{
		_boardTracker = new BoardTracker();
		_board = _boardTracker.InitializeBoard();
	}

	[Test]
	public void Board_ShouldBeInitializedProperly()
	{
		Assert.That(_board, Has.Count.EqualTo(32));

		// White
		Assert.Multiple(() =>
		{
			Assert.That(_board.Where(_ => _.Color == ePlayerColor.White).Count(), Is.EqualTo(16));
			Assert.That(_board.Where(_ => _.Color == ePlayerColor.White && _.Type == eFigureType.Queen).ToList(), Has.Count.EqualTo(1));
			Assert.That(_board.Where(_ => _.Color == ePlayerColor.White && _.Type == eFigureType.King).ToList(), Has.Count.EqualTo(1));
			Assert.That(_board.Where(_ => _.Color == ePlayerColor.White && _.Type == eFigureType.Rook).ToList(), Has.Count.EqualTo(2));
			Assert.That(_board.Where(_ => _.Color == ePlayerColor.White && _.Type == eFigureType.Bishop).ToList(), Has.Count.EqualTo(2));
			Assert.That(_board.Where(_ => _.Color == ePlayerColor.White && _.Type == eFigureType.Knight).ToList(), Has.Count.EqualTo(2));
			Assert.That(_board.Where(_ => _.Color == ePlayerColor.White && _.Type == eFigureType.Pawn).ToList(), Has.Count.EqualTo(8));
			Assert.That(_board.Where(_ => _.Color == ePlayerColor.White && _.Type == eFigureType.Pawn).All(_ => _.Position.EndsWith('2')), Is.True);
		});

		// Black
		Assert.Multiple(() =>
		{
			Assert.That(_board.Where(_ => _.Color == ePlayerColor.Black).ToList(), Has.Count.EqualTo(16));
			Assert.That(_board.Where(_ => _.Color == ePlayerColor.Black && _.Type == eFigureType.Queen).ToList(), Has.Count.EqualTo(1));
			Assert.That(_board.Where(_ => _.Color == ePlayerColor.Black && _.Type == eFigureType.King).ToList(), Has.Count.EqualTo(1));
			Assert.That(_board.Where(_ => _.Color == ePlayerColor.Black && _.Type == eFigureType.Rook).ToList(), Has.Count.EqualTo(2));
			Assert.That(_board.Where(_ => _.Color == ePlayerColor.Black && _.Type == eFigureType.Bishop).ToList(), Has.Count.EqualTo(2));
			Assert.That(_board.Where(_ => _.Color == ePlayerColor.Black && _.Type == eFigureType.Knight).ToList(), Has.Count.EqualTo(2));
			Assert.That(_board.Where(_ => _.Color == ePlayerColor.Black && _.Type == eFigureType.Pawn).ToList(), Has.Count.EqualTo(8));
			Assert.That(_board.Where(_ => _.Color == ePlayerColor.Black && _.Type == eFigureType.Pawn).All(_ => _.Position.EndsWith('7')), Is.True);
		});
	}

	[Test]
	public void Board_GetAllSquares()
	{
		var squares = MatchExtensions.GetAllBoardSquares();

		Assert.Multiple(() =>
		{
			Assert.That(squares, Has.Count.EqualTo(64));
			Assert.That(squares.First(), Is.EqualTo("a1"));
			Assert.That(squares.Last(), Is.EqualTo("h8"));
		});
	}
}