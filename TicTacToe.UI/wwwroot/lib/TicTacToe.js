let playerId;
let playerPiece;
let gameId;

const getCookieValue = (name) => (
    document.cookie.match('(^|;)\\s*' + name + '\\s*=\\s*([^;]+)')?.pop() || ''
)

function changePlayerData(playerId)
{
    $.post("http://localhost:21842/api/player/info/" + playerId)
        .done(function (player)
        {
            $("#gamesCount").text(player.gamesCount);
            $("#winsCount").text(player.winsCount);
            $("#failuresCount").text(player.failuresCount);
        })
        .fail(function (data)
        {
            alert("Error:" + data.title);
        });
}

function startGame(playerId) {
    $(".block").empty();
    $(".block").addClass("block-empty");
    $.post("http://localhost:21842/api/game/startNew/" + playerId)
        .done(function (game) {
            gameId = game.id;
            playerPiece = game.playerPiece;
            $("#playerPiece").text(playerPiece);
            for (let i = 0; i < game.gameBoard.length; i++) {
                if (game.gameBoard[i] != null) {
                    $(`#piece-${i}`).text(game.gameBoard[i]);
                    $(`#piece-${i}`).removeClass("block-empty");
                }
            }
        })
        .fail(function (data) {
            alert("Error:" + data.title);
        });
}

function makeMove(position) {
    $.post(`http://localhost:21842/api/game/makeMove/${gameId}/${playerId}/${position}`)
        .done(function (game) {
            for (let i = 0; i < game.gameBoard.length; i++) {
                if (game.gameBoard[i] != null && $(`#piece-${i}`).text() == "") {
                    $(`#piece-${i}`).text(game.gameBoard[i]);
                    $(`#piece-${i}`).removeClass("block-empty");
                }
            }

            if (game.isCompleted) {
                changePlayerData(playerId);
                startGame(playerId);
            }
        })
        .fail(function (data) {
            alert("Error:" + data.title);
        });
}

$(document).ready(function ()
{
    playerId = getCookieValue("playerId");
    if (playerId == '') {
        $.post("http://localhost:21842/api/player/create")
            .done(function (player) {
                document.cookie = "playerId=" + player.id;
                playerId = player.id;
                changePlayerData(player.id);
                startGame(player.id);
            })
            .fail(function (data) {
                alert("Error:" + data.title);
            });
    } else {
        changePlayerData(playerId);
        startGame(playerId);
    }  
});

$(".block").on("click", function () {
    if ($(this).text() == "") {
        $(this).text(playerPiece);
        $(this).removeClass("block-empty");
        makeMove(this.id.substring(6))
    }
});

$("#resetGame").on("click", function () {
    startGame(playerId);
})