using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.BLL.DTO;
using TicTacToe.BLL.Exceptions;
using TicTacToe.BLL.Interfaces;
using TicTacToe.DAL.EF;
using TicTacToe.DAL.Entities;

namespace TicTacToe.BLL.Services
{
    public class GameService : IGameService
    {
        private TicTacToeContext _dbContext;
        private IGameBot _gameBot;

        private MapperConfiguration _mapConfig;
        private IMapper _mapper;

        public GameService(TicTacToeContext dbContext, IGameBot gameBot)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }
            if (gameBot == null)
            {
                throw new ArgumentNullException("gameBot");
            }
            _dbContext = dbContext;
            _gameBot = gameBot;

            _mapConfig = new MapperConfiguration(mc => {
                mc.CreateMap<DAL.Entities.Game, GameDTO>().ReverseMap();
                mc.CreateMap<Player, PlayerDTO>().ReverseMap();
                mc.CreateMap<Movement, MovementDTO>().ReverseMap();
                mc.CreateMap<GamePlayer, GamePlayerDTO>().ForMember(gp => gp.Piece, opt => opt.MapFrom(gp => gp.Piece == "X" ? Pieces.X : Pieces.O));
                mc.CreateMap<GamePlayerDTO, GamePlayer>().ForMember(gp => gp.Piece, opt => opt.MapFrom(gp => gp.Piece.ToString()));
            });
            _mapper = _mapConfig.CreateMapper();
        }

        public GameDTO MakeMove(Guid playerId, Guid gameId, short position)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                var player = _dbContext.Players.Find(playerId);
                   //.AsQueryable()
                   //.Include(p => p.PlayerGames.Where(pg => pg.GameId == gameId))
                   //.ThenInclude(pg => pg.Game)
                   //.ThenInclude(g => g.Movements)
                   //.Where(p => p.Id == playerId)
                   //.FirstOrDefault();

                if (player == null)
                {
                    transaction.Rollback();
                    throw new NotFoundException("Player");
                }
                var game = _dbContext.Games
                    .AsQueryable()
                    .Include(g => g.GamePlayers)
                    .Include(g => g.Movements)
                    .Where(g => g.Id == gameId && g.GamePlayers.Where(gp => gp.PlayerId == playerId).Any())
                    .FirstOrDefault();

                if (game == null)
                {
                    transaction.Rollback();
                    throw new NotFoundException("Game");
                }

                if (game.IsCompleted)
                {
                    transaction.Rollback();
                    throw new GameException("The game is already over.");
                }

                var gameDto = _mapper.Map<DAL.Entities.Game, GameDTO>(game);

                var playerMovement = new MovementDTO
                {
                    PlayerId = playerId,
                    GameId = gameId,
                    Position = position,
                    Number = (short)game.Movements.Count()
                };
                gameDto.Movements.Add(playerMovement);
                var gameIsValid = BLL.GameUtils.Game.GameIsValid(gameDto.Movements, out string message);
                if (!gameIsValid)
                {
                    transaction.Rollback();
                    throw new GameException(message);
                }
                _dbContext.Movements.Add(_mapper.Map<MovementDTO, Movement>(playerMovement));
                var winnerId = BLL.GameUtils.Game.GetWinnerId(gameDto.Movements, out short?[] winNumbers );
                if (winnerId != Guid.Empty)
                {
                    gameDto.WinnerId = winnerId;
                    gameDto.WinNumbers = winNumbers;
                }
                gameDto.IsCompleted = winnerId != Guid.Empty || !GameUtils.Game.MovementsIsLeft(gameDto.Movements);
                if (!gameDto.IsCompleted)
                {
                    var botMovement = _gameBot.MakeMove(gameDto.Movements);
                    botMovement.GameId = gameDto.Id;
                    gameDto.Movements.Add(botMovement);

                    gameIsValid = BLL.GameUtils.Game.GameIsValid(gameDto.Movements, out message);
                    if (!gameIsValid)
                    {
                        transaction.Rollback();
                        throw new GameException(message);
                    }
                    _dbContext.Movements.Add(_mapper.Map<MovementDTO, Movement>(botMovement));
                    winnerId = BLL.GameUtils.Game.GetWinnerId(gameDto.Movements, out winNumbers);
                    if (winnerId != Guid.Empty)
                    {
                        gameDto.WinnerId = winnerId;
                        gameDto.WinNumbers = winNumbers;
                    }
                    gameDto.IsCompleted = winnerId != Guid.Empty || !GameUtils.Game.MovementsIsLeft(gameDto.Movements);
                }

                game.IsCompleted = gameDto.IsCompleted;
                game.WinnerId = gameDto.WinnerId;
                _dbContext.Games.Update(game);
                _dbContext.SaveChanges();
                transaction.Commit();

                return gameDto;
            }
        }

        public GameDTO StartGame(Guid playerId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                if (!_dbContext.Players.AsQueryable().Where(p => p.Id == playerId).Any())
                {
                    transaction.Rollback();
                    throw new NotFoundException("Player");
                }

                //TODO перенести создание бота
                if(!_dbContext.Players.AsQueryable().Where(p => p.Id == _gameBot.Id).Any())
                {
                    _dbContext.Players.Add(new Player { Id = _gameBot.Id, Name = _gameBot.Name, CreateTime = DateTime.UtcNow  });
                    _dbContext.SaveChanges();
                }

                var playerGames = _dbContext.Games
                    .AsQueryable()
                    .Include(g => g.GamePlayers)
                    .Where(g => g.GamePlayers.Where(gp => gp.PlayerId == playerId).Any())
                    .OrderByDescending(g => g.CreateTime)
                    .Take(2)
                    .ToList();

                var game = new GameDTO();
                var gamePlayer = new GamePlayerDTO();
                var gameBot = new GamePlayerDTO();

                gamePlayer.PlayerId = playerId;
                gamePlayer.GameId = game.Id;

                gameBot.PlayerId = _gameBot.Id;
                gameBot.GameId = game.Id;

                if (playerGames.Count() > 0)
                {
                    var lastGame = playerGames[0];
                    if (!lastGame.IsCompleted)
                    {
                        _dbContext.Games.Remove(lastGame);
                        if (playerGames.Count() > 1)
                        {
                            lastGame = playerGames[1];
                        }
                    }

                    var piece = lastGame.GamePlayers.Where(gp => gp.PlayerId == playerId).First().Piece;
                    Enum.TryParse(piece, out Pieces lastPiece);

                    if (lastPiece == Pieces.X)
                    {
                        gamePlayer.Piece = Pieces.O;
                        gameBot.Piece = Pieces.X;
                        var movement = _gameBot.MakeMove(game.Movements);
                        game.Movements.Add(_gameBot.MakeMove(game.Movements));
                    }
                    else
                    {
                        gamePlayer.Piece = Pieces.X;
                        gameBot.Piece = Pieces.O;
                    }
                }
                else
                {                
                    gamePlayer.Piece = Pieces.X;
                    gameBot.Piece = Pieces.O;
                  
                }

                game.GamePlayers.Add(gamePlayer);
                game.GamePlayers.Add(gameBot);
                _dbContext.Games.Add(_mapper.Map<GameDTO, DAL.Entities.Game>(game));
                _dbContext.SaveChanges();
                transaction.Commit();
                return game;
            }
        }
    }
}
