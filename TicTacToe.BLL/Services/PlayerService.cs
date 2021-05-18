using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.BLL.DTO;
using TicTacToe.BLL.Interfaces;
using TicTacToe.DAL.EF;
using TicTacToe.DAL.Entities;

namespace TicTacToe.BLL.Services
{
    public class PlayerService : IPlayerService
    {
        private TicTacToeContext _dbContext;

        private MapperConfiguration _mapConfig;
        private IMapper _mapper;

        public PlayerService(TicTacToeContext dbContext)
        {
            if(dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }
            _dbContext = dbContext;

            _mapConfig = new MapperConfiguration(mc => {
                mc.CreateMap<Player, PlayerDTO>().ReverseMap();
            });
            _mapper = _mapConfig.CreateMapper();
        }

        public PlayerDTO GetPlayerInfo(Guid? playerId = null, string name = "Anonymous")
        {
            if (playerId != null)
            {
                var player = _dbContext.Players.Find(playerId);
                if(player == null)
                {
                    player = new Player
                    {
                        Id = Guid.NewGuid(),
                        Name = name,
                        CreateTime = DateTime.UtcNow
                    };

                    _dbContext.Players.Add(player);
                    _dbContext.SaveChanges();
                }
               
                return _mapper.Map<Player, PlayerDTO>(player);
            }
            else
            {
                var player = new Player
                {
                    Id = Guid.NewGuid(),
                    Name = String.IsNullOrEmpty(name) ? "Anonymous" : name,
                    CreateTime = DateTime.UtcNow
                };

                _dbContext.Players.Add(player);
                _dbContext.SaveChanges();

                return _mapper.Map<Player, PlayerDTO>(player);
            }
        }
    }
}
