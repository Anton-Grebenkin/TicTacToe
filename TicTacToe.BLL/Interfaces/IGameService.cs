using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.BLL.DTO;

namespace TicTacToe.BLL.Interfaces
{
    public interface IGameService
    {
        GameDTO StartGame(Guid playerId);
        GameDTO MakeMove(Guid playerId, Guid gameId, Int16 position);
    }
}
