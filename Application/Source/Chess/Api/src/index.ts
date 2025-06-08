import * as signalR from "@microsoft/signalr";
import * as $ from "jquery";
import "./css/main.css";
import page from "page";

import homeHtml from "./pages/home.html";
import queueHtml from "./pages/queue.html";
import matchHtml from "./pages/match.html";

import { ePlayerColor, eFigureType } from "./enumerators";
import { Match, Figure } from "./models";

$(() => {
    routing();
    signalr_Matchmaking();
});

let currentMatch: Match;

function signalr_Matchmaking(): void {
    const matchmakingConnection = new signalR.HubConnectionBuilder()
        .withUrl('/hub/matchmaking')
        .build();

    $('#start').on('click', () => matchmakingConnection.send('findMatch'));
    $(document).on('click', '#cancel', () => matchmakingConnection.send('cancel'));

    matchmakingConnection.on('waitingForMatch', () => page('/queue'));
    matchmakingConnection.on('matchFound', (match: Match) => {
        currentMatch = match;
        page('/match')
    });
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
    const boardMap = new Map(currentMatch.board.map(_ => [_.position, _]));

    let body = `<table><tbody>`;

    for (let i = 8; i > 0; i--) {
        body += `<tr id="row_${i}" class="board-row">`;

        // ASCII (a, b, c, d, e, f, g, h)
        for (let j = 97; j < 105; j++) {
            const colChar = String.fromCharCode(j);
            const pos = `${colChar}${i}`;
            const colorClass = (i % 2 + j % 2) === 1 ? 'light-cell' : 'dark-cell';
            const figure = boardMap.get(pos);
            const html = getBoardFigureHtml(figure);

            body += `<td id="col_${colChar}" class="column ${colorClass}">
                        <span class="board-figure">${html}</span>
                    </td>`;
        }

        body += `</tr>`;
    }

    body += `</tbody></table>`;
    container.html(body);
}

const FigureHtml: Record<ePlayerColor, Record<eFigureType, string>> = {
    [ePlayerColor.White]: {
        [eFigureType.Pawn]: '&#9817;',
        [eFigureType.Rock]: '&#9814;',
        [eFigureType.Knight]: '&#9816;',
        [eFigureType.Bishop]: '&#9815;',
        [eFigureType.Queen]: '&#9813;',
        [eFigureType.King]: '&#9812;',
    },
    [ePlayerColor.Black]: {
        [eFigureType.Pawn]: '&#9823;',
        [eFigureType.Rock]: '&#9820;',
        [eFigureType.Knight]: '&#9822;',
        [eFigureType.Bishop]: '&#9821;',
        [eFigureType.Queen]: '&#9819;',
        [eFigureType.King]: '&#9818;',
    }
};

function getBoardFigureHtml(figure: Figure | undefined): string {
    return figure ? FigureHtml[figure.color]?.[figure.type] ?? '' : '';
}