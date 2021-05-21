function ApiClient(url)
{
    this.player = "test";
    this.CreatePlayer = function ()
    {
        $.post(url + "/player/create")
            .done(function (data) {
                console.log("test");
                this.player = new Player(data.id, data.name, data.gamesCount, data.winsCount, data.failuresCount);
                return this.player;
                //let player = new Player(data.id, data.name, data.gamesCount, data.winsCount, data.failuresCount);
                //return player;
                //return new Player(data.id, data.name, data.gamesCount, data.winsCount, data.failuresCount);
            })
            .fail(function (data) {
                throw new ApiException(data.responseJSON.status, data.responseJSON.title)
            });
    };
}

function Player(id, name, gamesCount, winsCount, failuresCount)
{
    this.id = id;
    this.name = name;
    this.gamesCount = gamesCount;
    this.winsCount = winsCount;
    this.failuresCount = failuresCount;
}

function ApiException(status, title)
{
    this.status = status;
    this.title = title;
}

console.log("hi");
$("#test").on("click", function () {
    console.log("hi2");
    var client = new ApiClient("http://localhost:21842/api");
    client.CreatePlayer();
});

const getCookieValue = (name) => (
    document.cookie.match('(^|;)\\s*' + name + '\\s*=\\s*([^;]+)')?.pop() || ''
)

$(document).ready(function () {
    console.log(getCookieValue("playerId"));
    var apiClient = new ApiClient("http://localhost:21842/api");
    if (getCookieValue("playerId") == '') {
        var player = apiClient.CreatePlayer();
        var player = apiClient.player;
        document.cookie = "playerId=" + player.id;
        console.log(document.cookie);
    }
});