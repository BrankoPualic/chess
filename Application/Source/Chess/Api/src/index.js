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
    //$('#cancel').on('click', () => matchmakingConnection.send('cancel'));
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
    (0, page_1.default)("/match", function () { return showView("matchPage", match_html_1.default); });
    function showView(id, html) {
        var views = $('.view');
        views.each(function (i, v) { v.style.display = 'none'; });
        var el = $("#".concat(id));
        el.html(html);
        el.css('display', 'block');
    }
    (0, page_1.default)();
}
