import * as signalR from "@microsoft/signalr";
import * as $ from "jquery";
import "./css/main.css";
import page from "page";

import homeHtml from "./pages/home.html";
import queueHtml from "./pages/queue.html";
import matchHtml from "./pages/match.html";

$(() => {
    routing();
    signalr_Matchmaking();
});

function signalr_Matchmaking(): void {
    const matchmakingConnection = new signalR.HubConnectionBuilder()
        .withUrl('/hub/matchmaking')
        .build();

    $('#start').on('click', () => matchmakingConnection.send('findMatch'));

    matchmakingConnection.on('waitingForMatch', () => page('/queue'));
    matchmakingConnection.on('matchFound', (match: any) => page('/match'));

    matchmakingConnection.start().catch((err) => console.error(err));
}

// Routing
function routing(): void {
    page("/", () => showView("homePage", homeHtml));
    page("/queue", () => showView("queuePage", queueHtml));
    page("/match", () => showView("matchPage", matchHtml));

    function showView(id: string, html: string): void {
        const views = $('.view');
        views.each((i, v) => { v.style.display = 'none' });

        const el = $(`#${id}`);

        el.html(html);
        el.css('display', 'block');
    }

    page();
}