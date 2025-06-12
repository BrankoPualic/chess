import { eFigureType, ePlayerColor } from "./enumerators";

export class FigureDto {
    id: string;
    type: eFigureType;
    color: ePlayerColor;
    position: string;
    previousPosition: string;
}

export class MatchDto {
    id: string;
    playerWhite: string;
    playerBlack: string;
    playerTurn: ePlayerColor;
    isCheckmate: boolean;
    playerVictorious: string;
    board: FigureDto[];
    blackCaptures: FigureDto[];
    whiteCaptures: FigureDto[];
}

export class MoveRequest {
    match: MatchDto;
    movedFigure: FigureDto;
    newPosition: string;
    lastMovedFigure: FigureDto;

    constructor();
    constructor(match: MatchDto, movedFigure: FigureDto, newPosition: string, lastMovedFigure?: FigureDto);
    constructor(match?: MatchDto, movedFigure?: FigureDto, newPosition?: string, lastMovedFigure?: FigureDto) {
        this.match = match;
        this.movedFigure = movedFigure;
        this.newPosition = newPosition;
        this.lastMovedFigure = lastMovedFigure;
    }
}