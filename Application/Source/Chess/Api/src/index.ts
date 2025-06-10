import * as signalR from "@microsoft/signalr";
import * as $ from "jquery";
import "./css/main.css";
import page from "page";

import homeHtml from "./pages/home.html";
import queueHtml from "./pages/queue.html";
import matchHtml from "./pages/match.html";

import { ePlayerColor, eFigureType } from "./enumerators";
import { MatchDto, FigureDto, MoveRequest } from "./models";

const matchmakingConnection = new signalR.HubConnectionBuilder()
    .withUrl('/hub/matchmaking')
    .build();

$(() => {
    routing();
    signalr_Matchmaking();
});

let currentMatch: MatchDto;
let playerId: string;
let selectedFigure: FigureDto;
let lastMovedFigure: FigureDto = null;

function signalr_Matchmaking(): void {
    $('#start').on('click', () => matchmakingConnection.send('findMatch'));
    $(document).on('click', '#cancel', () => matchmakingConnection.send('cancel'));

    matchmakingConnection.on('waitingForMatch', (player: string) => {
        playerId = player;
        page('/queue')
    });
    matchmakingConnection.on('matchFound', (match: MatchDto, player: string) => {
        currentMatch = match;
        if (!playerId)
            playerId = player;

        page('/match')
    });
    matchmakingConnection.on('cancelled', () => page('/'));

    matchmakingConnection.on('moved', (match: MatchDto, movedFigure: FigureDto) => {
        currentMatch = match;
        lastMovedFigure = movedFigure;
        board_Init()
    });

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
    board_Render();
    cell_Process();
    figure_Process();
}

function board_Render(): void {
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

            body += `<td id="col_${pos}" class="cell ${colorClass}">
                        <span class="board-figure" data-type="${figure?.type}" data-color="${figure?.color}">${html}</span>
                    </td>`;
        }

        body += `</tr>`;
    }

    body += `</tbody></table>`;
    container.html(body);
}

function figure_Process(): void {
    $('.board-figure').each((i, el) => {
        const dataset = el.dataset;
        const figureType = Number(dataset['type']);
        const figureColor = Number(dataset['color']);
        const playerColor = playerId === currentMatch.playerWhite ? ePlayerColor.White : ePlayerColor.Black;

        if (figureColor != playerColor) {
            el.classList.add('opponent-figure');
            return;
        }
        else {
            el.classList.add('my-figure');
        }

        const figure = currentMatch.board.find(_ => _.color == figureColor && _.type == figureType);
        if (!figure)
            return;

        el.addEventListener('click', (e) => {
            if (playerColor !== currentMatch.playerTurn)
                return;

            $('.selected').removeClass('selected');
            el.parentElement.classList.add('selected');
            selectedFigure = figure;
            e.stopPropagation();
        });
    })
}

function cell_Process(): void {
    $('.cell').each((i, el) => {
        el.addEventListener('click', () => {
            const selected = $('.selected');
            if (!selected)
                return;

            const figure = currentMatch.board.find(_ => _.position == selected.attr('id').split('_')[1]);
            const newPosition = el.id.split('_')[1];
            const moveRequest = new MoveRequest(currentMatch, figure, newPosition, lastMovedFigure);
            matchmakingConnection.invoke('Move', moveRequest)
                .catch(_ => console.error(_));

            $('.selected').removeClass('selected');
        })
    })
}

const FigureHtml: Record<ePlayerColor, Record<eFigureType, string>> = {
    [ePlayerColor.White]: {
        [eFigureType.Pawn]: '&#9817;',
        [eFigureType.Rook]: '&#9814;',
        [eFigureType.Knight]: '&#9816;',
        [eFigureType.Bishop]: '&#9815;',
        [eFigureType.Queen]: '&#9813;',
        [eFigureType.King]: '&#9812;',
    },
    [ePlayerColor.Black]: {
        [eFigureType.Pawn]: '&#9823;',
        [eFigureType.Rook]: '&#9820;',
        [eFigureType.Knight]: '&#9822;',
        [eFigureType.Bishop]: '&#9821;',
        [eFigureType.Queen]: '&#9819;',
        [eFigureType.King]: '&#9818;',
    }
};

function getBoardFigureHtml(figure: FigureDto | undefined): string {
    return figure ? FigureHtml[figure.color]?.[figure.type] ?? '' : '';
}