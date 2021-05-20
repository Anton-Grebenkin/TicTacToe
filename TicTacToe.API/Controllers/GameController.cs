using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToe.API.Models.Response;
using TicTacToe.BLL.DTO;
using TicTacToe.BLL.Exceptions;
using TicTacToe.BLL.Interfaces;

namespace TicTacToe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private IGameService _gameService;

        private MapperConfiguration _mapConfig;
        private IMapper _mapper;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;

            _mapConfig = new MapperConfiguration(mc => {
                mc.CreateMap<GameDTO, GameResponseModel>().ForMember(g => g.WinNumbers, opt => opt.MapFrom(g => g.WinNumbers.Contains(null) ? null : g.WinNumbers));

                //{
                //    //string[] board = new string[9];
                //    //foreach (var movement in g.Movements)
                //    //{
                //    //    board[movement.Position] = g.GamePlayers.Where(gp => gp.PlayerId == movement.PlayerId).First().Piece == Pieces.X ? "X" : "O";
                //    //}
                //    //return board;
                //}));
            });
            _mapper = _mapConfig.CreateMapper();
        }

        [HttpPost]
        [Route("startNew/{playerId}")]
        public IActionResult StartNewGame(Guid playerId)
        {
            try
            {
                var game = _gameService.StartGame(playerId);
                var result = _mapper.Map<GameDTO, GameResponseModel>(game);
                result.PlayerId = playerId;
                if(game.Movements.Count >= 1)
                {
                    result.PlayerPiece = game.Movements.Where(m => m.Number == 0).First().PlayerId == playerId ? "X" : "O";
                }
                else
                {
                    result.PlayerPiece = "X";
                }
              
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return StatusCode(404,
                    new ProblemDetails()
                    {
                        Status = 404,
                        Title = ex.Message
                    });
            }
            catch(Exception ex)
            {
                return StatusCode(412,
                  new ProblemDetails()
                  {
                      Status = 412,
                      Title = ex.Message
                  });
            }
        }

        [HttpPost]
        [Route("makeMove/{gameId}/{playerId}/{position}")]
        public IActionResult StartNewGame(Guid playerId, Guid gameId, short position)
        {
            try
            {
                var game = _gameService.MakeMove(playerId, gameId, position);
                var result = _mapper.Map<GameDTO, GameResponseModel>(game);
                result.PlayerId = playerId;
                //TODO переделать выбор фигуры.
                result.PlayerPiece = game.Movements.Where(m => m.Number == 0).First().PlayerId == playerId ? "X" : "O";
                return Ok(result);
            }
            catch(GameException ex)
            {
                return StatusCode(412,
                    new ProblemDetails()
                    {
                        Status = 412,
                        Title = ex.Message
                    });
            }
            catch(NotFoundException ex)
            {
                return StatusCode(404,
                    new ProblemDetails()
                    {
                        Status = 404,
                        Title = ex.Message
                    });
            }
        }
    }
}
