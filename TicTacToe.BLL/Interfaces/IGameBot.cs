using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.BLL.DTO;

namespace TicTacToe.BLL.Interfaces
{
    public interface IGameBot
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        MovementDTO MakeMove(IEnumerable<MovementDTO> movements);
    }
}
