using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.BLL.DTO;

namespace TicTacToe.BLL.Interfaces
{
    public interface IPlayerService
    {
        PlayerDTO GetPlayerInfo(Guid? playerId = null, String name = "Anonymous");
    }
}
