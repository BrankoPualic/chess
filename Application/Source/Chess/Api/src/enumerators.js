"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.eFigureType = exports.ePlayerColor = void 0;
var ePlayerColor;
(function (ePlayerColor) {
    ePlayerColor[ePlayerColor["White"] = 1] = "White";
    ePlayerColor[ePlayerColor["Black"] = 2] = "Black";
})(ePlayerColor || (exports.ePlayerColor = ePlayerColor = {}));
var eFigureType;
(function (eFigureType) {
    eFigureType[eFigureType["Pawn"] = 1] = "Pawn";
    eFigureType[eFigureType["Rook"] = 2] = "Rook";
    eFigureType[eFigureType["Knight"] = 3] = "Knight";
    eFigureType[eFigureType["Bishop"] = 4] = "Bishop";
    eFigureType[eFigureType["Queen"] = 5] = "Queen";
    eFigureType[eFigureType["King"] = 6] = "King";
})(eFigureType || (exports.eFigureType = eFigureType = {}));
