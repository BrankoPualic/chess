"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var signalR = require("@microsoft/signalr");
var $ = require("jquery");
require("./css/main.css");
var page_1 = require("page");
var home_html_1 = require("./pages/home.html");
var queue_html_1 = require("./pages/queue.html");
var match_html_1 = require("./pages/match.html");
$(function () {
    routing();
    signalr_Matchmaking();
});
function signalr_Matchmaking() {
    var matchmakingConnection = new signalR.HubConnectionBuilder()
        .withUrl('/hub/matchmaking')
        .build();
    $('#start').on('click', function () { return matchmakingConnection.send('findMatch'); });
    $(document).on('click', '#cancel', function () { return matchmakingConnection.send('cancel'); });
    matchmakingConnection.on('waitingForMatch', function () { return (0, page_1.default)('/queue'); });
    matchmakingConnection.on('matchFound', function (match) { return (0, page_1.default)('/match'); });
    matchmakingConnection.on('cancelled', function () { return (0, page_1.default)('/'); });
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
    var container = $("#board");
    var board = "<table><tbody>";
    for (var i = 8; i > 0; i--) {
        board += "<tr id=\"row_".concat(i, "\" class=\"board-row\">");
        // ASCII (a, b, c, d, e, f, g, h)
        for (var j = 97; j < 105; j++) {
            var colorClass = (i % 2 + j % 2) === 1 ? 'light-cell' : 'dark-cell';
            board += "<td id=\"col_".concat(String.fromCharCode(j), "\" class=\"column ").concat(colorClass, "\"></td>");
        }
        board += "</tr>";
    }
    board += "</tbody></table>";
    container.html(board);
}
