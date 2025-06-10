"use strict";
var _a, _b, _c;
Object.defineProperty(exports, "__esModule", { value: true });
var signalR = require("@microsoft/signalr");
var $ = require("jquery");
require("./css/main.css");
var page_1 = require("page");
var home_html_1 = require("./pages/home.html");
var queue_html_1 = require("./pages/queue.html");
var match_html_1 = require("./pages/match.html");
var enumerators_1 = require("./enumerators");
var models_1 = require("./models");
var matchmakingConnection = new signalR.HubConnectionBuilder()
    .withUrl('/hub/matchmaking')
    .build();
$(function () {
    routing();
    signalr_Matchmaking();
});
var currentMatch;
var playerId;
var selectedFigure;
var lastMovedFigure = null;
function signalr_Matchmaking() {
    $('#start').on('click', function () { return matchmakingConnection.send('findMatch'); });
    $(document).on('click', '#cancel', function () { return matchmakingConnection.send('cancel'); });
    matchmakingConnection.on('waitingForMatch', function (player) {
        playerId = player;
        (0, page_1.default)('/queue');
    });
    matchmakingConnection.on('matchFound', function (match, player) {
        currentMatch = match;
        if (!playerId)
            playerId = player;
        (0, page_1.default)('/match');
    });
    matchmakingConnection.on('cancelled', function () { return (0, page_1.default)('/'); });
    matchmakingConnection.on('moved', function (match) {
        currentMatch = match;
        lastMovedFigure = selectedFigure;
        board_Init();
    });
    matchmakingConnection.start().catch(function (err) { return console.error(err); });
}
// Routing
function routing() {
    (0, page_1.default)("/", function () { return showView("homePage", home_html_1.default); });
    (0, page_1.default)("/queue", function () { return showView("queuePage", queue_html_1.default); });
    (0, page_1.default)("/match", function () { return view_RenderMatch(); });
    (0, page_1.default)();
}
function showView(id, html) {
    var views = $('.view');
    views.each(function (i, v) { v.style.display = 'none'; });
    var container = $("#".concat(id));
    container.html(html);
    container.css('display', 'block');
}
function view_RenderMatch() {
    showView('matchPage', match_html_1.default);
    board_Init();
}
// Board
function board_Init() {
    board_Render();
    cell_Process();
    figure_Process();
}
function board_Render() {
    var container = $("#board");
    var boardMap = new Map(currentMatch.board.map(function (_) { return [_.position, _]; }));
    var body = "<table><tbody>";
    for (var i = 8; i > 0; i--) {
        body += "<tr id=\"row_".concat(i, "\" class=\"board-row\">");
        // ASCII (a, b, c, d, e, f, g, h)
        for (var j = 97; j < 105; j++) {
            var colChar = String.fromCharCode(j);
            var pos = "".concat(colChar).concat(i);
            var colorClass = (i % 2 + j % 2) === 1 ? 'light-cell' : 'dark-cell';
            var figure = boardMap.get(pos);
            var html = getBoardFigureHtml(figure);
            body += "<td id=\"col_".concat(pos, "\" class=\"cell ").concat(colorClass, "\">\n                        <span class=\"board-figure\" data-type=\"").concat(figure === null || figure === void 0 ? void 0 : figure.type, "\" data-color=\"").concat(figure === null || figure === void 0 ? void 0 : figure.color, "\">").concat(html, "</span>\n                    </td>");
        }
        body += "</tr>";
    }
    body += "</tbody></table>";
    container.html(body);
}
function figure_Process() {
    $('.board-figure').each(function (i, el) {
        var dataset = el.dataset;
        var figureType = Number(dataset['type']);
        var figureColor = Number(dataset['color']);
        var playerColor = playerId === currentMatch.playerWhite ? enumerators_1.ePlayerColor.White : enumerators_1.ePlayerColor.Black;
        if (figureColor != playerColor) {
            el.classList.add('opponent-figure');
            return;
        }
        else {
            el.classList.add('my-figure');
        }
        var figure = currentMatch.board.find(function (_) { return _.color == figureColor && _.type == figureType; });
        if (!figure)
            return;
        el.addEventListener('click', function (e) {
            if (playerColor !== currentMatch.playerTurn)
                return;
            $('.selected').removeClass('selected');
            el.parentElement.classList.add('selected');
            selectedFigure = figure;
            e.stopPropagation();
        });
    });
}
function cell_Process() {
    $('.cell').each(function (i, el) {
        el.addEventListener('click', function () {
            var selected = $('.selected');
            if (!selected)
                return;
            var figure = currentMatch.board.find(function (_) { return _.position == selected.attr('id').split('_')[1]; });
            var newPosition = el.id.split('_')[1];
            var moveRequest = new models_1.MoveRequest(currentMatch, figure, newPosition, lastMovedFigure);
            matchmakingConnection.invoke('Move', moveRequest)
                .catch(function (_) { return console.error(_); });
            $('.selected').removeClass('selected');
        });
    });
}
var FigureHtml = (_a = {},
    _a[enumerators_1.ePlayerColor.White] = (_b = {},
        _b[enumerators_1.eFigureType.Pawn] = '&#9817;',
        _b[enumerators_1.eFigureType.Rook] = '&#9814;',
        _b[enumerators_1.eFigureType.Knight] = '&#9816;',
        _b[enumerators_1.eFigureType.Bishop] = '&#9815;',
        _b[enumerators_1.eFigureType.Queen] = '&#9813;',
        _b[enumerators_1.eFigureType.King] = '&#9812;',
        _b),
    _a[enumerators_1.ePlayerColor.Black] = (_c = {},
        _c[enumerators_1.eFigureType.Pawn] = '&#9823;',
        _c[enumerators_1.eFigureType.Rook] = '&#9820;',
        _c[enumerators_1.eFigureType.Knight] = '&#9822;',
        _c[enumerators_1.eFigureType.Bishop] = '&#9821;',
        _c[enumerators_1.eFigureType.Queen] = '&#9819;',
        _c[enumerators_1.eFigureType.King] = '&#9818;',
        _c),
    _a);
function getBoardFigureHtml(figure) {
    var _a, _b;
    return figure ? (_b = (_a = FigureHtml[figure.color]) === null || _a === void 0 ? void 0 : _a[figure.type]) !== null && _b !== void 0 ? _b : '' : '';
}
