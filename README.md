# TicTacToe
C# NET 5.0 Tic Tac Toe-playing with bot web API.

## Usage 
Example usage of this API.

### Create player.
```
POST /api/player/create?name=
```
parametr name not requaried.
Response:
```
{
    "id": "6cad4863-acaf-404e-a419-1d8c5803d7ce",
    "name": "Anonymous",
    "gamesCount": 0,
    "winsCount": 0,
    "failuresCount": 0
}
```

### Player games info.
```
POST /api/player/info/{playerId}
```
Response:
```
{
    "id": "6cad4863-acaf-404e-a419-1d8c5803d7ce",
    "name": "Anonymous",
    "gamesCount": 0,
    "winsCount": 0,
    "failuresCount": 0
}
```

### Start new game.
```
POST /api/game/startNew/{playerId}
```
Response:
```
{
    "id": "0d028b1a-413b-4486-a2f1-7310a929730b",
    "playerId": "6cad4863-acaf-404e-a419-1d8c5803d7ce",
    "playerPiece": "X",
    "isCompleted": false,
    "gameBoard": [
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null,
        null
    ],
    "winNumbers": [],
    "winnerId": null
}
```

### Make play move.
```
POST /api/game/makeMove/{gameId}/{playerId}/{position}
```
Response:
```
{
    "id": "0d028b1a-413b-4486-a2f1-7310a929730b",
    "playerId": "6cad4863-acaf-404e-a419-1d8c5803d7ce",
    "playerPiece": "X",
    "isCompleted": false,
    "gameBoard": [
        "X",
        null,
        null,
        null,
        "O",
        null,
        null,
        null,
        null
    ],
    "winNumbers": [],
    "winnerId": null
}
```
