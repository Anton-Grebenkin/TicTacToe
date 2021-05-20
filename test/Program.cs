using Microsoft.EntityFrameworkCore;
using System;
using TicTacToe.BLL.Interfaces;
using TicTacToe.BLL.Services;
using TicTacToe.BLL.DTO;
using TicTacToe.DAL.EF;
using Newtonsoft.Json;
using TicTacToe.BLL.GameUtils;
using System.Collections.Generic;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            var contextOptions = new DbContextOptionsBuilder<TicTacToeContext>()
            .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TicTacToe;Trusted_Connection=True;")
            .Options;
            TicTacToeContext context = new TicTacToeContext(contextOptions);

            IPlayerService playerService = new PlayerService(context);
            IGameService gameService = new GameService(context, new GameBot());

            var player = playerService.GetPlayerInfo();

            //var game = gameService.StartGame(player.Id);

            //game = gameService.MakeMove(player.Id, game.Id, 4);
            //game = gameService.MakeMove(player.Id, game.Id, 1);
            //game = gameService.MakeMove(player.Id, game.Id, 5);
            //game = gameService.MakeMove(player.Id, game.Id, 8);
            ////game = gameService.MakeMove(player.Id, game.Id, 8);

            //game = gameService.MakeMove(player.Id, game.Id, 0);

        }
    }
}
