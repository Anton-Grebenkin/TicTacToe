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
    [Route("api/player")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private IPlayerService _playerService;

        private MapperConfiguration _mapConfig;
        private IMapper _mapper;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;

            _mapConfig = new MapperConfiguration(mc => {
                mc.CreateMap<PlayerDTO, PlayerResponseModel>();
            });
            _mapper = _mapConfig.CreateMapper();
        }

        [HttpPost]
        [Route("info/{playerId}")]
        public IActionResult GetPlayerInfo(Guid playerId)
        {
            try
            {
                var player = _playerService.GetPlayerInfo(playerId);
                var result = _mapper.Map<PlayerDTO, PlayerResponseModel>(player);
                return Ok(result);
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
            catch (Exception ex)
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
        [Route("create")]
        public IActionResult CreatePlayer(string name)
        {
            try
            {
                var player = _playerService.GetPlayerInfo(name: name);
                var result = _mapper.Map<PlayerDTO, PlayerResponseModel>(player);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(412,
                   new ProblemDetails()
                   {
                       Status = 412,
                       Title = ex.Message
                   });
            }
        }
    }
}
