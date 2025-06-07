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
    $(document).on('click', '#cancel', () => matchmakingConnection.send('cancel'));

    matchmakingConnection.on('waitingForMatch', () => page('/queue'));
    matchmakingConnection.on('matchFound', (match: any) => page('/match'));
    matchmakingConnection.on('cancelled', () => page('/'));

    matchmakingConnection.start().catch((err) => console.error(err));
}

// Routing
function routing(): void {
    page("/", () => showView("homePage", homeHtml));
    page("/queue", () => showView("queuePage", queueHtml));
    page("/match", () => view_RenderMatch());

    page();
}

function showView(id: string, html: string): void {
    const views = $('.view');
    views.each((i, v) => { v.style.display = 'none' });

    const container = $(`#${id}`);

    container.html(html);
    container.css('display', 'block');
}

function view_RenderMatch(): void {
    showView('matchPage', matchHtml);
    board_Init();
}

// Board

function board_Init(): void {
    const container = $(`#board`);

    let board = `<table><tbody>`;

    for (let i = 8; i > 0; i--) {
        board += `<tr id="row_${i}" class="board-row">`;

        for (let j = 97; j < 105; j++) { // ASCII (a, b, c, d, e, f, g, h)
            const colorClass = (i % 2 + j % 2) === 1 ? 'light-cell' : 'dark-cell';
            board += `<td id="col_${String.fromCharCode(j)}" class="column ${colorClass}"></td>`
        }

        board += `</tr>`;
    }

    board += `</tbody></table>`;

    container.html(board);
}