import { eFigureType, ePlayerColor } from "./enumerators";

export class Figure {
    type: eFigureType;
    color: ePlayerColor;
    position: string;
}

export class Match {
    id: string;
    playerWhite: string;
    playerBlack: string;
    playerTurn: ePlayerColor;
    board: Figure[];
}