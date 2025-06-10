"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.MoveRequest = exports.MatchDto = exports.FigureDto = void 0;
var FigureDto = /** @class */ (function () {
    function FigureDto() {
    }
    return FigureDto;
}());
exports.FigureDto = FigureDto;
var MatchDto = /** @class */ (function () {
    function MatchDto() {
    }
    return MatchDto;
}());
exports.MatchDto = MatchDto;
var MoveRequest = /** @class */ (function () {
    function MoveRequest(match, movedFigure, newPosition, lastMovedFigure) {
        this.match = match;
        this.movedFigure = movedFigure;
        this.newPosition = newPosition;
        this.lastMovedFigure = lastMovedFigure;
    }
    return MoveRequest;
}());
exports.MoveRequest = MoveRequest;
